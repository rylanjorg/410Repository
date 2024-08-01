

Shader "Unlit/SkyBox"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HorizonColor ("Horizon Color", Color) = (1,1,1,1)
        _SkyColor ("Sky Color", Color) = (1,1,1,1)
        _WindSpeed ("Wind Speed", Float) = 1
        _CloudHeight ("Cloud Height", Float) = 1
        _CloudEdge ("Cloud Edge", Float) = 1
        _CloudNoise("Cloud Noise Texture", 2D) = "white" {}
        _FogHeight("Cloud Edge", Float) = 1
        _CloudColor("Cloud Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define PI 3.1415926538

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 skyBoxUV : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 skyBoxUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _CloudNoise;
            float4 _MainTex_ST;

            float4 _HorizonColor;
            float4 _SkyColor;
            float _WindSpeed;
            float _CloudHeight;
            float _CloudEdge;
            float _FogHeight;
            float4 _CloudColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //World Space position
                float3 worldSpacePositionNormalized = normalize(mul(unity_ObjectToWorld, v.vertex));

                o.skyBoxUV.x = atan2( worldSpacePositionNormalized.r, worldSpacePositionNormalized.b)/(radians(360));
                o.skyBoxUV.y = asin(worldSpacePositionNormalized.g)/(radians(180)/2);
                
                //o.uv = TRANSFORM_TEX(skyboxUV, _MainTex);
                //o.uv = skyboxUV;

                return o;
            }

            float Unity_Posterize_float(float In, float Steps)
            {
                float Out = floor(In / (1 / Steps)) * (1 / Steps);
                return Out;
            }

            float Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax)
            {
                float Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                return Out;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture

                //float2 CloudUV =  
                float CloudTex = saturate(tex2D(_CloudNoise, i.skyBoxUV * float2(5,2) + float2(1,0) * _Time.y * _WindSpeed * 0.01 * -2));
                float CloudTex2 = saturate(tex2D(_CloudNoise, i.skyBoxUV * float2(8,4)+ float2(1,0) * _Time.y * _WindSpeed * 0.01));
                CloudTex = Unity_Remap_float(Unity_Posterize_float(saturate(CloudTex * CloudTex2), 6), float2(0,1),float2(0.5,1));
                
                
                float CloudMask = 1 - smoothstep(_CloudHeight, saturate(_CloudHeight + _CloudEdge), i.skyBoxUV.g + 0.1);
                CloudMask = step(0.9,CloudMask * Unity_Remap_float((CloudTex * CloudTex2), float2(0,1), float2(1,2)));
                //fixed4 col = CloudMask;
                
                //fixed4 col = fixed4(CloudTex,0,0,1);

                //float Fog = pow(saturate(1 - i.skyBoxUV.g), _FogHeight);

                //fixed4 col = tex2D(_MainTex, i.skyBoxUV);
                fixed4 colLerp = lerp(_HorizonColor, _SkyColor, i.skyBoxUV.y);
                fixed4 maskLerp = lerp(colLerp, CloudTex + 0.4 * _CloudColor, CloudMask);
                //fixed4 col = lerp(maskLerp,)

            

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return maskLerp;
            }
            ENDCG
        }
    }
}
