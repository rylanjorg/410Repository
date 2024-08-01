// URP Custom Function equivalent of LightingWater
float4 LightingWater(float3 Albedo, float Alpha, float3 Normal, float Smoothness, float3 lightDir, float3 viewDir, float atten, float3 lightColor, float SpecularFactor)
{
    float4 col = float4(Albedo, Alpha);
    
    // Calculate the dot product of the surface normal and light direction
    float ndotl = saturate(dot(Normal, normalize(lightDir)));
    
    // Apply attenuation to the light based on the dot product
    float shadow = ndotl * atten;
    col.rgb *= shadow;

    // Calculate the half vector and its dot product with the surface normal
    float3 h = normalize(lightDir + viewDir);
    float nh = max(0, dot(Normal, h));
    
    // Calculate specular highlights
    float spec = step(0.01, pow(nh, lerp(48.0, 500, Smoothness)));
    
    // Apply light color to the result
    col.rgb *= lightColor * Smoothness * SpecularFactor * spec;
    
    return col;
}
