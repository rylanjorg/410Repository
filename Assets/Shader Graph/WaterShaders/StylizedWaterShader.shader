Shader "Custom/StylizedWaterShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _IntersectionTexture ("Intersection Texture", 2D) = "white" {}
        _IntersectionTextureOffset ("IntersectionTextureOffset", Range(0,1)) = 0.6
        _FoamNoise ("Foam Noise", 2D) = "white" {}
        _Speed ("Scroll Speed", Range(0,1)) = 0.5
        _IntersectionPow ("IntersectionTex Pow", Range(0,4)) = 0.5
        _SpecularFactor ("Specular Factor", Range(0,10)) = 1.0
        [Normal] _Normal ("Water Normal", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Blend SrcAlpha OneMinusSrcAlpha
        Zwrite Off
        LOD 200

        GrabPass{
            "_GrabTexture"
        }

        CGPROGRAM
        #pragma surface surf Water fullforwardshadows vertex:vert alpha:fade
        #pragma target 5.0

        sampler2D _MainTex;


        struct Input
        {
            float2 uv_MainTex;
            float4 grabPassUV;
            float2 texcoord;
            float3 worldPos;
            //float3 orNormals;
        };


        struct SurfaceOutputWater 
        {
            fixed3 Albedo;
            half3 Emission;
            fixed3 Normal;
            half Smoothness;
            half Alpha;
            //fixed3 orNormals;
        };

        half _Glossiness;
        fixed4 _Color;
        fixed4 _FoamColor;
        float _Speed;
        float _IntersectionPow;
        float _IntersectionTextureOffset;
        float _SpecularFactor;

        float4 _IntersectionCamProperties;

        sampler2D _IntersectionTexture;
        sampler2D _GrabTexture;
        sampler2D _FoamNoise;
        sampler2D _Normal;

        float4 _FoamNoise_ST;

        float4 LightingWater(SurfaceOutputWater s, float3 lightDir, half3 viewDir, half atten)
        {
            float4 col = float4(s.Albedo, s.Alpha);
            //dot(s.Normal, lightDir) returns how much the surface normal points towards the light
            //It has a value of 0 where the surface is paralell to the direction to the light, 
            //is has a value of 1 where the light points towards the light 
            //and a value of -1 where the surface points away.

            //Returns dot(s.Normal, lightDir) saturated to the range [0,1] as follows:

            //1) Returns 0 if x is less than 0; else
            //2) Returns 1 if x is greater than 1; else
            //3) Returns x otherwise.
            float ndotl = saturate(dot(s.Normal, normalize(lightDir)));
            float shadow = ndotl * atten;
            //multiply the color by the saturated light value
            col.rgb *= shadow;

           half3 h = normalize(lightDir + viewDir);
           float nh = max(0, dot(s.Normal,h));
           float spec = pow(nh,lerp(48.0,500,s.Smoothness));
           col.rgb = spec * shadow * _LightColor0 * s.Smoothness * _SpecularFactor;

           return col;

        }

 


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.grabPassUV = ComputeGrabScreenPos(UnityObjectToClipPos(v.vertex));
            o.texcoord = v.texcoord;
            //o.orNormals = UnityObjectToWorldNormal(v.normal);
        }

        void surf (Input IN, inout SurfaceOutputWater o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            

            float2 intersectionRTUVs = IN.worldPos.xz - _IntersectionCamProperties.xz;
            intersectionRTUVs= intersectionRTUVs/(_IntersectionCamProperties.w * 2);
            intersectionRTUVs += 0.5;

            fixed intersectionTex = _IntersectionTextureOffset - pow(tex2D(_IntersectionTexture, intersectionRTUVs).a, _IntersectionPow);
            float intersection = saturate((sin((1.0 - intersectionTex) * 40.0 + _Time.y * 2) * 0.5 + 0.5) * pow(1.0 - intersectionTex, 15.0) + pow(1.0 - intersectionTex,10.0));

            float2 scrolledTexcoord = IN.texcoord * _FoamNoise_ST.xy + float2(0,_Speed * _Time.y); //+ _Time.y + _FoamNoise_ST.zw;
            float foamTex = tex2D(_FoamNoise, scrolledTexcoord).x;
            float foam = smoothstep(intersection, intersection - 0.1, foamTex);


            o.Normal = UnpackNormal(tex2D(_Normal, IN.texcoord));


            fixed4 grabCol = tex2Dproj(_GrabTexture, IN.grabPassUV);

            o.Albedo = lerp(grabCol.rgb,lerp(c.rgb, _FoamColor,foam),max(c.a, foam));

            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
