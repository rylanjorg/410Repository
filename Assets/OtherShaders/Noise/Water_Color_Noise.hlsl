float2 random2d(float2 st)
{
    st = float2(dot(st, float2(127.1, 311.7)),
                dot(st, float2(269.5, 183.3)));
    return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
}

void GenerateNoise_float(float2 p, float noiseScale, float octave, float persistence, float lacunarity, float borderSize, out float t3) 
{
    t3 = 0.0;
    float amplitude = 1.0;
    float frequency = 1.0;
    float total_amplitude = 0.0;
    float2 borderOffset = float2(borderSize, borderSize);

    for (int j = 0; j < floor(octave); j++) 
    {
        float2 f = frac(p * frequency * noiseScale);
        float2 i = floor(p * frequency * noiseScale);
        float2 u = f*f*(3.0 - 2.0*f);
        
        // Add border to texture coordinates
        i += borderOffset;
        f -= borderOffset;

        float a = dot(random2d(i), f);
        float b = dot(random2d(i + float2(1.0, 0.0)), f - float2(1.0, 0.0));
        float c = dot(random2d(i + float2(0.0, 1.0)), f - float2(0.0, 1.0));
        float d = dot(random2d(i + float2(1.0, 1.0)), f - float2(1.0, 1.0));
        float t1 = lerp(a, b, u.x);
        float t2 = lerp(c, d, u.x);
        t3 += amplitude * lerp(t1, t2, u.y);
        total_amplitude += amplitude;
        amplitude *= persistence;
        frequency *= lacunarity;
    }

    t3 /= total_amplitude;
}
