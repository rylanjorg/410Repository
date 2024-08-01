// URP Custom Function equivalent of LightingWater
float CalculateDepthDifference(float4 screenPos, float depth)
{
    //float depth = SampleSceneDepth(screenPos.xy / screenPos.w);
    //depth = Linear01Depth(depth); // Convert to linear depth
    float diff;
    if(unity_OrthoParams.w == 0)
    {
        diff = depth - screenPos.w;
    }
    else
    {
        float near = _ProjectionParams.y;
        float far = _ProjectionParams.z;
        #ifdef UNITY_REVERSED_Z
            float dist = -lerp(near, far, depth);
            float scrDis = -lerp(near, far, screenPos.z);
        #else
            float dist = -lerp(far, near, depth);
            float scrDis = -lerp(far, near, screenPos.z);
        #endif

        diff = dist - scrDis;
    }

    return diff;
}
