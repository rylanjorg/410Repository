#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED






// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

struct CustomLightingData 
{
    // Position and orientation
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;
    float4 shadowCoord;

    // Surface attributes
    float3 albedo;
    float rimAmount;
    float smoothness;
    float ambientOcclusion;
    float specularScale;
    float shadowSaturation;
    float shadowLightness;
    float shadowHue;
    float customShadow;
    float3 inputNoise;

    // Baked lighting
    float3 bakedGI;
    float4 shadowMask;
};

float PosterizeFloat(float In, float Steps)
{
    return floor(In / (1 / Steps)) * (1 / Steps);
}

float3 RGBtoHSL(float3 color) 
{

    float3 hsl = 0;
    float maxVal = max(max(color.r, color.g), color.b);
    float minVal = min(min(color.r, color.g), color.b);
    float delta = maxVal - minVal;
    
    hsl.z = (maxVal + minVal) / 2.0; // Luminance
    
    if (delta != 0)
    {
        if (hsl.z < 0.5)
        {
            hsl.y = delta / (maxVal + minVal); // Saturation
        }
        else
        {
            hsl.y = delta / (2.0 - maxVal - minVal); // Saturation
        }
        
        float deltaR = (((maxVal - color.r) / 6.0) + (delta / 2.0)) / delta;
        float deltaG = (((maxVal - color.g) / 6.0) + (delta / 2.0)) / delta;
        float deltaB = (((maxVal - color.b) / 6.0) + (delta / 2.0)) / delta;
        
        if (color.r == maxVal)
        {
            hsl.x = deltaB - deltaG; // Hue
        }
        else if (color.g == maxVal)
        {
            hsl.x = (1.0 / 3.0) + deltaR - deltaB; // Hue
        }
        else if (color.b == maxVal)
        {
            hsl.x = (2.0 / 3.0) + deltaG - deltaR; // Hue
        }
        
        if (hsl.x < 0.0)
        {
            hsl.x += 1.0;
        }
        else if (hsl.x > 1.0)
        {
            hsl.x -= 1.0;
        }
    }

    return hsl;
}

float hue2rgb(float p, float q, float t)
{
    if (t < 0.0f) t += 1.0f;
    if (t > 1.0f) t -= 1.0f;
    if (t < 1.0f/6.0f) return p + (q - p) * 6.0f * t;
    if (t < 1.0f/2.0f) return q;
    if (t < 2.0f/3.0f) return p + (q - p) * (2.0f/3.0f - t) * 6.0f;
    return p;
}

float3 HSLtoRGB(float h, float s, float l)
{
    float r, g, b;

    if(s == 0.0f)
    {
        r = g;
        g = b;
        b = l; // achromatic
    }
    else
    {
       

        float q = l < 0.5f ? l * (1.0f + s) : l + s - l * s;
        float p = 2.0f * l - q;
        r = hue2rgb(p, q, h + 1.0f/3.0f);
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 1.0f/3.0f);
    }

    return float3(r,g,b);

}


#ifndef SHADERGRAPH_PREVIEW
float3 CustomGlobalIllumination(CustomLightingData d) 
{
    float3 indirectDiffuse = d.albedo * d.bakedGI * d.ambientOcclusion;

    float3 reflectVector = reflect(-d.viewDirectionWS, d.normalWS);
    // This is a rim light term, making reflections stronger along
    // the edges of view
    float fresnel = Pow4(1 - saturate(dot(d.viewDirectionWS, d.normalWS)));
    // This function samples the baked reflections cubemap
    // It is located in URP/ShaderLibrary/Lighting.hlsl
    float3 indirectSpecular = GlossyEnvironmentReflection(reflectVector,
        RoughnessToPerceptualRoughness(1 - d.smoothness),
        d.ambientOcclusion) * fresnel;

    return indirectDiffuse + indirectSpecular;
}



float3 CustomLightHandling(CustomLightingData d, Light light) 
{
    float shadow = light.shadowAttenuation;
    float3 radiance = light.color * (light.distanceAttenuation * shadow); //* light.shadowAttenuation);

    //Posterize the diffuse 
    float NdotL = dot(d.normalWS, light.direction);
    float smoothNdotL = smoothstep(0.0f, 0.5, NdotL);
    smoothNdotL += (d.inputNoise.x * smoothNdotL);
    smoothNdotL = saturate(smoothNdotL);
    float diffuse = PosterizeFloat(smoothNdotL, 4.0f); 
    float3 diffuseColor = d.albedo * radiance * diffuse;

    // rim
    float4 rimDot = 1 - dot(d.viewDirectionWS, d.normalWS);
    float rimIntensity = rimDot * NdotL;
    rimIntensity = smoothstep(d.rimAmount - 0.01, d.rimAmount + 0.01, rimIntensity);
    float3 rimColor = rimIntensity * light.color * diffuse;
    

    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    specularDot = min(specularDot, 1.0);
    float specularIntensity = pow(specularDot, d.smoothness) * diffuse;
    specularIntensity += (d.inputNoise.y * specularIntensity);
    specularIntensity = saturate(specularIntensity);
    specularIntensity = PosterizeFloat(specularIntensity, 4.0f);
    //float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
    float3 specularColor = specularIntensity * d.specularScale * light.color * diffuseColor;

    

    //float3 color = diffuseColor; //(diffuseColor + specularColor + rimColor); //lerp((1-diffuse),  1.0f, (diffuseColor + specularColor + rimColor));*/

    return diffuseColor + specularColor;
}




float3 CustomShadowHandling(CustomLightingData d, Light light) 
{
    float NdotL = dot(d.normalWS, light.direction);
    float diffuse = smoothstep(0, 0.01, clamp(NdotL * smoothstep(0, 0.01, light.distanceAttenuation * light.shadowAttenuation), 0.0f, 1.0f));

    float3 hslLight = RGBtoHSL(light.color);
    hslLight.y = d.shadowSaturation;
    hslLight.z = d.shadowLightness;
    float3 shadowColor =  lerp((diffuse), 1.0f,  HSLtoRGB(hslLight.x, hslLight.y, hslLight.z));

    return shadowColor;
}



float Shadow(CustomLightingData d, Light light) 
{
    float NdotL = dot(d.normalWS, light.direction);
    float diffuse = step(0.0001, clamp(NdotL * smoothstep(0, 0.01, light.distanceAttenuation * light.shadowAttenuation), 0.0f, 1.0f));
    //float diffuse = step(0.0001f, clamp(smoothstep(0, 0.01, light.shadowAttenuation), 0.0f, 1.0f));
    return diffuse;
}


#endif



float3 CalculateCustomLighting(CustomLightingData d) 
{
    #ifdef SHADERGRAPH_PREVIEW
         // In preview, estimate diffuse + specular
        float3 lightDir = float3(0.5, 0.5, 0);
        float intensity = saturate(dot(d.normalWS, lightDir)) +
        pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), d.smoothness);
        return d.albedo * intensity;
    #else

       // Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
        Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, d.shadowMask);
        // In mixed subtractive baked lights, the main light must be subtracted
        // from the bakedGI value. This function in URP/ShaderLibrary/Lighting.hlsl takes care of that.
        //MixRealtimeAndBakedGI(mainLight, d.normalWS, d.bakedGI);
        float3 color = 0.0f;//CustomGlobalIllumination(d);
        float shadowMask = 1.0f;
        
        float3 shadowColor = 1.0f;
       
        // Shade the main light
        color += CustomLightHandling(d, mainLight);
        shadowMask *= Shadow(d, mainLight);
        shadowColor *= CustomShadowHandling(d, mainLight);
        
        //color += CustomShadowHandling(d, mainLight);

        #ifdef _ADDITIONAL_LIGHTS
            // Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
            uint numAdditionalLights = GetAdditionalLightsCount();
            for (uint lightI = 0; lightI < numAdditionalLights; lightI++) 
            {
                Light light = GetAdditionalLight(lightI, d.positionWS, d.shadowMask);
                color += CustomLightHandling(d, light);
                shadowMask *= Shadow(d, light);
                shadowColor *= CustomShadowHandling(d, light);
                
            } 

        #endif

        float3 finalColor = color * shadowMask;
        
        finalColor += lerp(shadowColor * (1-shadowMask), color, d.customShadow);
        return finalColor;
        //shadowColor = lerp(0.0f,shadowColor, clamp((1.0f - shadow), 0.0f, 1.0f));

        return color;//lerp((1.0f- shadow), 0.0f, shadowColor);
    #endif
}

void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection, float3 Albedo, float RimAmount, float Smoothness, float AmbientOcclusion, float2 LightmapUV, float specularScale, float ShadowSaturation, float ShadowLightness, float ShadowHue, float CustomShadowScaler, float3 InputNoise, out float3 Color)
{

    CustomLightingData d;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;
    d.positionWS = Position;
    d.ambientOcclusion = AmbientOcclusion; 
    d.rimAmount = RimAmount;
    d.specularScale = specularScale;
    d.shadowSaturation = ShadowSaturation;
    d.shadowLightness = ShadowLightness;
    d.shadowHue = ShadowHue;
    d.customShadow = CustomShadowScaler;
    d.inputNoise = InputNoise;

    #ifdef SHADERGRAPH_PREVIEW
        // In preview, there's no shadows or bakedGI
        d.shadowCoord = 0;
        d.bakedGI = 0;
        d.shadowMask = 0;
    #else
        // Calculate the main light shadow coord
        // There are two types depending on if cascades are enabled
        float4 positionCS = TransformWorldToHClip(Position);
        #if SHADOWS_SCREEN
            d.shadowCoord = ComputeScreenPos(positionCS);
        #else
            d.shadowCoord = TransformWorldToShadowCoord(Position);
        #endif

        // The following URP functions and macros are all located in
        // URP/ShaderLibrary/Lighting.hlsl
        // Technically, OUTPUT_LIGHTMAP_UV, OUTPUT_SH, VertexLighting and ComputeFogFactor
        // should be called in the vertex function of the shader. However, as of
        // 2021.1, we do not have access to custom interpolators in the shader graph.

        // The lightmap UV is usually in TEXCOORD1
        // If lightmaps are disabled, OUTPUT_LIGHTMAP_UV does nothing
        float2 lightmapUV;
        OUTPUT_LIGHTMAP_UV(LightmapUV, unity_LightmapST, lightmapUV);
        // Samples spherical harmonics, which encode light probe data
        float3 vertexSH;
        OUTPUT_SH(Normal, vertexSH);
        // This function calculates the final baked lighting from light maps or probes
        d.bakedGI = SAMPLE_GI(lightmapUV, vertexSH, Normal);
        // This function calculates the shadow mask if baked shadows are enabled
        d.shadowMask = SAMPLE_SHADOWMASK(lightmapUV);
        
    #endif

    Color = CalculateCustomLighting(d);
}

#endif