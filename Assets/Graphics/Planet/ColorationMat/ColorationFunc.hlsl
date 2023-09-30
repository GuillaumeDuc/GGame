#include "SimplexNoise.hlsl"
#include "PerlinNoise.hlsl"
#include "VoronoiNoise.hlsl"
#include "GradientFunc.hlsl"

float4 getOcean (float noise) {
  float4 oc = getGradient(noise, 0);
  if (noise <= 0) {
    oc *= (1 - abs(noise));
  }
  return oc;
}

float4 getSnowRegions (float3 position, float regionScale, float baseNoise) {
  float maskNoise = clamp(baseNoise, -1, 1);
  float region1Noise = clamp(evaluate(position + 5, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);

  if (region1Noise > 0 && maskNoise > 0) {
    return getGradient(region1Noise, 3);
  }
  return getOcean(maskNoise);
}

float4 getRegions (float3 position, float regionScale, float baseNoise) {
  float maskNoise = clamp(baseNoise, -1, 1);
  
  float region1Noise = clamp(evaluate(position + 10, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region2Noise = clamp(evaluate(position + 20, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region3Noise = clamp(evaluate(position + 30, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);

  if (region1Noise > 0 && region2Noise > 0 && maskNoise > 0) {
    return clamp(lerp(getGradient(region1Noise, 1), getGradient(region2Noise, 2), region2Noise), 0, 1);
  } else if (region1Noise > 0 && maskNoise > 0) {
    return getGradient(region1Noise, 1);
  } else if (region2Noise > 0 && maskNoise > 0) {
    return getGradient(region2Noise, 2);
  } else if (region3Noise > 0 && maskNoise > 0) {
    return getGradient(region3Noise, 3);
  }
  return getOcean(maskNoise);
}

float getMountains (float3 position, float mountainScale, float baseNoise) {
  float mountains = (1 - abs(cnoise((position + 10) * mountainScale))) - (1 - baseNoise);
  return clamp(mountains, 0, 1) / 2;
}

float4 getColorationByType(int planetType, float3 position, float regionScale, float baseNoise) {
  switch(planetType)
  {
    case 0:
      return getRegions(position, regionScale, baseNoise);
    case 1:
    default:
      return getSnowRegions(position, regionScale, baseNoise);  
  }
}

void ColorationFunc_float (
  float3 position,
  int planetType,
  int numLayers,
  float strength,
  float baseRoughness,
  float roughness,
  float persistence,
  float minValue,
  float regionScale,
  out float4 color,
  out float3 smooth
  ) {
  float baseNoise = clamp(evaluate(position, numLayers, strength, baseRoughness, roughness, persistence, minValue), -1, 1);

  color = getColorationByType(planetType, position, regionScale, baseNoise);
  smooth = 1 - baseNoise;
}