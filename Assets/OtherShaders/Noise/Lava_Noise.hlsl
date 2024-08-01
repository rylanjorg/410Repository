
float2 random2(float2 st)
{
    st = float2( dot(st,float2(127.1,311.7)),
              dot(st,float2(269.5,183.3)) );
    return -1.0 + 2.0*frac(sin(st)*43758.5453123);
}

float noise(float2 st) 
{
    float2 i = floor(st);
    float2 f = frac(st);

    float2 u = f*f*(3.0-2.0*f);

    return lerp( lerp( dot( random2(i + float2(0.0,0.0) ), f - float2(0.0,0.0) ),
                     dot( random2(i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
                lerp( dot( random2(i + float2(0.0,1.0) ), f - float2(0.0,1.0) ),
                     dot( random2(i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
}

// Define a function that takes inputs and returns an output
void GenerateNoise_float(float2 fragCoord, float2 resolution, float2 time, out float3 OutColor)
{
    float2 st = fragCoord.xy/resolution.xy;
    st.x *= resolution.x/resolution.y;

    float t = 1.0;
    // Uncomment to animate
    t = abs(1.0-sin(time*.1))*5.;
    // Comment and uncomment the following lines:
    st += noise(st*2.)*t; // Animate the coordinate space
    OutColor = float3(1., 1., 1.) * smoothstep(.18,.2,noise(st)); // Big black drops
    OutColor += smoothstep(.15,.2,noise(st*10.)); // Black splatter
    OutColor -= smoothstep(.35,.4,noise(st*10.)); // Holes on splatter

    OutColor = float3(1.-OutColor);
}
