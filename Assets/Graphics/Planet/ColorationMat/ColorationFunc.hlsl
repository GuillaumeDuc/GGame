#include "SimplexNoise.hlsl"
#include "PerlinNoise.hlsl"
#include "VoronoiNoise.hlsl"
#include "GradientFunc.hlsl"

float4 getOneRegion (float3 position, float regionScale, float baseNoise, int planetType, float3 rgbOffset) {
  float maskNoise = clamp(baseNoise, -1, 1);

  if (maskNoise > 0) {
    switch(planetType) {
      case 1:
        return getDefaultSnow(maskNoise, rgbOffset);
      case 2:
        return getDefaultDesert(maskNoise, rgbOffset);
      default:
      case 3:
        return getDefaultForest(maskNoise, rgbOffset);
    }
  }
  return getDefaultWater(maskNoise, (0, 0, 0));
}

float4 getRegions (float3 position, float regionScale, float baseNoise, float3 rgbOffset) {
  float maskNoise = clamp(baseNoise, -1, 1);
  
  float region1Noise = clamp(evaluate(position + 10, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region2Noise = clamp(evaluate(position + 20, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);
  float region3Noise = clamp(evaluate(position + 30, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1);

  if (region1Noise > 0 && region2Noise > 0 && maskNoise > 0) {
    return clamp(lerp(getDefaultForest(region1Noise, rgbOffset), getDefaultDesert(region2Noise, rgbOffset), region2Noise), 0, 1);
  } else if (region1Noise > 0 && maskNoise > 0) {
    return getDefaultForest(region1Noise, rgbOffset);
  } else if (region2Noise > 0 && maskNoise > 0) {
    return getDefaultDesert(region2Noise, rgbOffset);
  } else if (region3Noise > 0 && maskNoise > 0) {
    return getDefaultSnow(region3Noise, rgbOffset);
  }
  return getDefaultWater(maskNoise, (0, 0, 0));
}

float getMountains (float3 position, float mountainScale, float baseNoise) {
  float mountains = (1 - abs(cnoise((position + 10) * mountainScale))) - (1 - baseNoise);
  return clamp(mountains, 0, 1) / 2;
}

float4 getColorationByType(int planetType, float3 position, float regionScale, float baseNoise, float3 rgbOffset) {
  // 0 : temperate, 1 : snow, 2 : desert, 3 : jungle, 4 : ocean
  switch(planetType) {
    case 0:
      return getRegions(position, regionScale, baseNoise, rgbOffset);
    case 1:
    case 2:
    case 3:
    default:
      return getOneRegion(position, regionScale, baseNoise, planetType, rgbOffset);
    case 4:
      return getDefaultWater(clamp(baseNoise, -1, 1), rgbOffset);
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
  float3 rgbOffset,
  out float4 color,
  out float3 smooth
  ) {
  float baseNoise = clamp(evaluate(position, numLayers, strength, baseRoughness, roughness, persistence, minValue), -1, 1);

  color = getColorationByType(planetType, position, regionScale, baseNoise, rgbOffset);
  smooth = planetType == 4 ? 0 : 1 - baseNoise;
}