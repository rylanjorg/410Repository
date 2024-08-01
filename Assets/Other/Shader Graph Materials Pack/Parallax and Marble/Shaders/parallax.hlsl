void parallax_float(float iterations, float heightScale, float3 viewDir, Texture2D HeightTex, float2 uv, SamplerState sampleState, bool flip_height, out float2 Out, out float Out_Depth)
{
  // determine required number of layers
  const float minLayers = 30;
  const float maxLayers = 60;
  float numLayers = lerp(maxLayers, minLayers, abs(dot(float3(0, 0, 1), viewDir)));

  float numSteps = min(100, iterations); // numLayers;//60.0f; // How many steps the UV ray tracing should take
  //float height = 1.0;
  //float step = 1.0 / numSteps;
  
  //float2 offset = uv.xy;
  //float4 HeightMap = HeightTex.Sample(sampleState, offset);
  //if(flip_height) HeightMap.r = 1 - HeightMap.r;
  
  float layerDepth = 1.0 / numLayers;
  float currentLayerDepth = 0.0;

  float2 P = (viewDir.xy / viewDir.z) * heightScale;
  float2 deltaTexCoords = P / numLayers;

  //float2 delta = viewDir.xy * heightScale / (viewDir.z * numSteps);

  float2 currentTexCoords = uv.xy;
  float currentDepthMapValue = HeightTex.Sample(sampleState, currentTexCoords).r;
  if(flip_height) currentDepthMapValue.r = 1 - currentDepthMapValue.r;

  
  // find UV offset
  for (float i = 0.0f; i < numSteps; i++) 
  {
    if (currentLayerDepth < currentDepthMapValue) 
    {
      //offset += delta;
      // shift texture coordinates along direction of P
       // Calculate infinite parallax offset
   
      currentTexCoords +=  deltaTexCoords;
      // get depthmap value at current texture coordinates
      currentDepthMapValue = HeightTex.Sample(sampleState, currentTexCoords);
      if(flip_height) currentDepthMapValue.r = 1 - currentDepthMapValue.r;

      //float2 infiniteOffset = viewDir.xy * (1.0 - currentDepthMapValue.r) * 0.01;
      //currentTexCoords += infiniteOffset;

      //currentDepthMapValue *= (1-currentDepthMapValue) * (1-transparency);
      //if(transparency == 0)
       // currentDepthMapValue = 1 - currentDepthMapValue;
      
      

      // get depth of next layer
      currentLayerDepth += layerDepth;  
    } 
    else 
    {
      break;
    }
  }

  // get texture coordinates before collision (reverse operations)
  float2 prevTexCoords = currentTexCoords - deltaTexCoords;

  // get depth after and before collision for linear interpolation
  float afterDepth  = currentDepthMapValue - currentLayerDepth;
  float beforeDepth = HeightTex.Sample(sampleState, prevTexCoords) - currentLayerDepth + layerDepth;

  // interpolation of texture coordinates
  float weight = afterDepth / (afterDepth - beforeDepth);
  float2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);

  Out_Depth = currentLayerDepth;
  Out = finalTexCoords;
}
