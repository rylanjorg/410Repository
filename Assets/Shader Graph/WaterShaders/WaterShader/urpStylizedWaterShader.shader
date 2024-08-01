

// This shader fills the mesh shape with a color predefined in the code.
Shader "Custom/urpStylizedWaterShader"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _IntersectionTexture ("Intersection Texture", 2D) = "white" {}
        _IntersectionPow("Intersection Power", float) = 1
        _DepthIntersectionThreshold ("Depth Intersection Threshold", float) = 1
        _FoamNoise ("Foam Noise", 2D) = "white" {}
        _SpecularFactor ("Specular Factor", Range(0,10)) = 1.0
        [Normal] _Normal ("Water Normal", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(-2,2)) = 1.0
        _ShadowStrength("Shadow Strength", Range(0,1)) = 1.0

        _ReflectionRoughness("Reflection roughness", Range(0,1)) = 0
        _ReflectionNormalStrength("Reflection normal strength", Range(0,1)) = 0
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue"="Transparent"}

        Blend SrcAlpha OneMinusSrcAlpha
        Zwrite Off
        LOD 200
        
        HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        sampler2D _FoamNoise;
        float4 _FoamNoise_ST;
        sampler2D _IntersectionTexture;
        float4 _IntersectionCamProperties;
        
        sampler2D _CameraDepthTexture;
        float _DepthIntersectionThreshold;
        float _IntersectionPow;

        float CalculateFoam(float3 worldPos, float2 texcoord, float depthDifference)
        {
            float2 intersectionRTUVs = worldPos.xz - _IntersectionCamProperties.xz;
            intersectionRTUVs= intersectionRTUVs/(_IntersectionCamProperties.w * 2);
            intersectionRTUVs += 0.5;

            float depthIntersection = saturate(depthDifference/ _DepthIntersectionThreshold);


            float intersectionVal =  clamp(tex2D(_IntersectionTexture, intersectionRTUVs).r, 0, 1);

            float intersectionTex = ((pow(intersectionVal, _IntersectionPow))) * depthIntersection;
            //float intersection = saturate((sin((1-intersectionTex) * 25.0 - _Time.y * 2.5) * 0.5 + 0.5) * pow(1-intersectionTex, 1.0) + pow(1-intersectionTex,5.0));
            float intersection = saturate((sin((1-intersectionTex) * 40.0 - _Time.y * 2) * 0.5 + 0.5) * pow(1-intersectionTex, 15.0) + pow(1-intersectionTex,3.0));
            
            float2 scrolledTexcoord = texcoord * _FoamNoise_ST.xy + _Time.y * _FoamNoise_ST.zw; //+ _Time.y + _FoamNoise_ST.zw;
            float foamTex = tex2D(_FoamNoise, scrolledTexcoord).x;
           // float foam = step(pow(intersection, _IntersectionPow), foamTex);
            //float foam = step(pow(intersection, _IntersectionPow), foamTex);
            float foam = smoothstep(intersection, intersection - 0.01, foamTex);

            return  foam;
        }

        float4 LightingWater(SurfaceOutputWater s, float3 lightDir, half3 viewDir, half atten)
        {
            float4 col = float4(s.Albedo, s.Alpha);
            //dot(s.Normal, lightDir) returns how much the surface normal points towards the light
            //It has a value of 0 where the surface is paralell to the direction to the light, 
            //is has a value of 1 where the light points towards the light 
            //and a value of -1 where the surface points away.

            //Returns dot(s.Normal, lightDir) saturated to the range [0,1] as follows:
            // Get the main light's properties
            Light mainLight = GetMainLight();

            //1) Returns 0 if x is less than 0; else
            //2) Returns 1 if x is greater than 1; else
            //3) Returns x otherwise.
            float ndotl = saturate(dot(s.Normal, normalize(lightDir)));
            float shadow = ndotl * atten;
            //multiply the color by the saturated light value
            col.rgb *= shadow;

            half3 h = normalize(lightDir + viewDir);
            float nh = max(0, dot(s.Normal,h));
            float spec = step(0.01, pow(nh,lerp(48.0,500,s.Smoothness)));
            col.rgb = spec * shadow * mainLight.color * s.Smoothness * _SpecularFactor;

            return col;
        }

        Pass
        {

        
    
            //#pragma vertex:vert alpha:fade
            //#pragma target 5.0

            /*sampler2D _MainTex;
            sampler2D _Normal;
            float4 _Normal_ST;
            float _NormalStrength;

            struct SurfaceOutputWater 
            {
                float3 Albedo;
                half3 Emission;
                float3 Normal;
                half Smoothness;
                half Alpha;
            };

            half _Glossiness;
            float4 _Color;
            float4 _FoamColor;
            float _SpecularFactor;
            sampler2D _GrabTexture;
            float _ReflectionNormalStrength;
            float _ReflectionRoughness;

            float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
            {
                float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
                float scalar = min(min(factors.x, factors.y), factors.z);
                if(cubemapPosition.w > 0)
                {
                    direction = direction * scalar + (position - cubemapPosition);
                }
                return direction;
            }

            float3 SampleReflectionProbes(float3 worldPos, float3 normal)
            {
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                //float3 reflectionNormal = float3(normal.rg * _ReflectionNormalStrength, lerp(1, normal.b, saturate(_ReflectionNormalStrength)));
                half3 worldRefl = reflect(-worldViewDir,  normal);
                half4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, BoxProjection(worldRefl,worldPos, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin,unity_SpecCube0_BoxMax), _ReflectionRoughness * UNITY_SPECCUBE_LOD_STEPS);
                return skyData.rgb;
            }*/

            
            struct Attributes
            {
                float4 positionOS : POSITION;
                //float2 uv_MainTex : TEXCOORD0;
                //float2 uv_Normal : TEXCOORD1;
                //float4 grabPassUV : TEXCOORD2;
                //float3 worldPos : TEXCOORD3;
                //float4 screenPos : TEXCOORD4;
                //float3 worldNormal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                ////float2 uv_MainTex : TEXCOORD0;
                //float2 uv_Normal : TEXCOORD1;
                //float4 grabPassUV : TEXCOORD2;
                //float3 worldPos : TEXCOORD3;
                //float4 screenPos : TEXCOORD4;
                //float3 worldNormal : NORMAL;
            };


            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                //OUT.uv_MainTex = IN.uv_MainTex;
                //OUT.grabPassUV = IN.grabPassUV;
                //OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS).xyz;
            //OUT.screenPos = ComputeScreenPos(UnityObjectToClipPos(IN.positionOS));
            // OUT.worldNormal = normalize(mul(unity_ObjectToWorld, float4(IN.worldNormal, 0)).xyz);
                // Returning the output.
                return OUT;
            }

            // The fragment shader definition.            
            float4 frag(Varyings IN) : SV_Target
            {
                // Defining the color variable and returning it.
                //float4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            
                //float depthDifference = CalculateDepthDifference(IN.screenPos);
                //float foam = CalculateFoam(IN.worldPos, IN.texcoord, depthDifference);
                //foam = depthIntersection;
                //float foam = smoothstep(intersection, intersection - 0.1, foamTex);

                //float3 normals = UnpackNormalWithScale(tex2D(_Normal, IN.texcoord * _Normal_ST.xy + _Time.xy * _Normal_ST.zw), _NormalStrength);
                //o.Normal = normals;

                //float4 grabCol = tex2Dproj(_GrabTexture, IN.grabPassUV + float4(normals.xy * 2.0 - 1.0,0,0));
                //float3 reflectionNormal = float3(o.Normal.rg * _ReflectionNormalStrength, lerp(1, o.Normal.b, saturate(_ReflectionNormalStrength)));
                //float3 reflectionProbeColor = SampleReflectionProbes(IN.worldPos, WorldNormalVector(IN,reflectionNormal));


                
                //half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);

                //c.rgb += c.rgb * reflectionProbeColor.rgb;

                //o.Albedo = lerp(grabCol.rgb,lerp(c.rgb, _FoamColor,foam),max(c.a, foam));
                //o.Albedo = lerp(grabCol.rgb, c.rgb, c.a);
                //o.Albedo = foam;
                //o.Albedo = tex2D(_Normal, IN.texcoord * _Normal_ST.xy);
                //o.Albedo = intersection;

            

                //o.Albedo = skyColor;
            
                //o.Smoothness = _Glossiness;
                //o.Alpha = 1.0;
                float4 customColor = float4(0.5, 0, 0, 1);
                return customColor;
            }
        }
        ENDHLSL
    }
    
    FallBack "Hidden/Universal Render Pipeline/Lit"
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}


// Complex Lit is superset of Lit, but provides
// advanced material properties and is always forward rendered.
// It also has higher hardware and shader model requirements.
/*
Shader "Universal Render Pipeline/Complex Lit"
{
    Properties
    {
        // Specular vs Metallic workflow
        _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax("Scale", Range(0.005, 0.08)) = 0.005
        _ParallaxMap("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DetailMask("Detail Mask", 2D) = "white" {}
        _DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
        _DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
        _DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
        [Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}

        [ToggleUI] _ClearCoat("Clear Coat", Float) = 0.0
        _ClearCoatMap("Clear Coat Map", 2D) = "white" {}
        _ClearCoatMask("Clear Coat Mask", Range(0.0, 1.0)) = 0.0
        _ClearCoatSmoothness("Clear Coat Smoothness", Range(0.0, 1.0)) = 1.0

        // Blending state
        _Surface("__surface", Float) = 0.0
        _Blend("__mode", Float) = 0.0
        _Cull("__cull", Float) = 2.0
        [ToggleUI] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _BlendOp("__blendop", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _SrcBlendAlpha("__srcA", Float) = 1.0
        [HideInInspector] _DstBlendAlpha("__dstA", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _BlendModePreserveSpecular("_BlendModePreserveSpecular", Float) = 1.0
        [HideInInspector] _AlphaToMask("__alphaToMask", Float) = 0.0

        [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
        // Editmode props
        _QueueOffset("Queue offset", Float) = 0.0

        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }

    SubShader
    {
        // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Universal Render Pipeline and Builtin Unity Pipeline
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "ComplexLit"
            "IgnoreProjector" = "True"
        }
        LOD 300

        // ------------------------------------------------------------------
        // Forward only pass.
        // Acts also as an opaque forward fallback for deferred rendering.
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForwardOnly"
            }

            // -------------------------------------
            // Render State Commands
            Blend[_SrcBlend][_DstBlend], [_SrcBlendAlpha][_DstBlendAlpha]
            ZWrite[_ZWrite]
            Cull[_Cull]
            AlphaToMask[_AlphaToMask]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _ _CLEARCOAT _CLEARCOATMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
            #pragma multi_compile _ _LIGHT_LAYERS
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"


            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            //
            // Fill GBuffer data to prevent "holes", just in case someone wants to reuse GBuffer data.
            // Deferred lighting is stenciled out for ComplexLit and rendered as forward.
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite[_ZWrite]
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 4.5

            // Deferred Rendering Path does not support the OpenGL-based graphics API:
            // Desktop OpenGL, OpenGL ES 3.0, WebGL 2.0.
            #pragma exclude_renderers gles3 glcore

            // -------------------------------------
            // Shader Stages
            #pragma vertex LitGBufferPassVertex
            #pragma fragment LitGBufferPassFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            //#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED

            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitGBufferPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        // This pass is used when drawing to a _CameraNormalsTexture texture with the forward renderer or the depthNormal prepass with the deferred renderer.
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }

            // -------------------------------------
            // Render State Commands
            Cull Off

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaLit

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SPECGLOSSMAP
            #pragma shader_feature EDITOR_VISUALIZATION

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "Universal2D"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // -------------------------------------
            // Render State Commands
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex vert
            #pragma fragment frag

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
            ENDHLSL
        }
    }

    //////////////////////////////////////////////////////

    FallBack "Hidden/Universal Render Pipeline/Lit"
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShader"
}
*/