#include "SimplexNoise.hlsl"
#include "PerlinNoise.hlsl"
#include "VoronoiNoise.hlsl"
#include "GradientFunc.hlsl"

float4 getOneRegion (float3 position, float regionScale, float baseNoise, int planetType) {
  float maskNoise = clamp(baseNoise, -1, 1);

  if (maskNoise > 0) {
    switch(planetType) {
      case 1:
        return getDefaultSnow(maskNoise);
      case 2:
        return getDefaultDesert(maskNoise);
      default:
      case 3:
        return getDefaultForest(maskNoise);
    }
  }
  return getDefaultWater(maskNoise);
}

float4 getRegions (float3 position, float regionScale, float baseNoise) {
  float maskNoise = clamp(baseNoise, -1, 1);
  
  float region1Noise = clamp(evaluate(position + 10, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region2Noise = clamp(evaluate(position + 20, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region3Noise = clamp(evaluate(position + 30, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);

  if (region1Noise > 0 && region2Noise > 0 && maskNoise > 0) {
    return clamp(lerp(getGradient(region1Noise, 1), getGradient(region2Noise, 2), region2Noise), 0, 1);
  } else if (region1Noise > 0 && maskNoise > 0) {
    return getDefaultForest(region1Noise);
  } else if (region2Noise > 0 && maskNoise > 0) {
    return getDefaultDesert(region2Noise);
  } else if (region3Noise > 0 && maskNoise > 0) {
    return getDefaultSnow(region3Noise);
  }
  return getDefaultWater(maskNoise);
}

float getMountains (float3 position, float mountainScale, float baseNoise) {
  float mountains = (1 - abs(cnoise((position + 10) * mountainScale))) - (1 - baseNoise);
  return clamp(mountains, 0, 1) / 2;
}

float4 getColorationByType(int planetType, float3 position, float regionScale, float baseNoise) {
  // 0 : temperate, 1 : snow, 2 : desert, 3 : jungle
  switch(planetType) {
    case 0:
      return getRegions(position, regionScale, baseNoise);
    case 1:
    case 2:
    case 3:
    default:
      return getOneRegion(position, regionScale, baseNoise, planetType);
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