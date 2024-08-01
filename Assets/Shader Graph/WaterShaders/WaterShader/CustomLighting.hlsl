

float4 LightingCustom(float3 normal, float specular, half3 lightDir, half atten, float3 Albedo, float Alpha, float3 mainLight)
{
    half3 h = normalize(lightDir + normal);
    half diff = max(0, dot(normal, lightDir));

    // Increase the specular strength by multiplying it with a constant
    half spec = pow(max(0, dot(normal, h)), specular) * 2.0;


    half4 c;
    c.rgb = (Albedo * mainLight.rgb * diff + mainLight.rgb * spec) * atten;
    c.a = Alpha;
    return c;
}