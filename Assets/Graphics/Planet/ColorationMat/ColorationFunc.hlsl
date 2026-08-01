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
  return getDefaultWater(maskNoise, float3(0, 0, 0));
}

float4 getRegions (float3 position, float regionScale, float baseNoise, float3 rgbOffset) {
  float maskNoise = clamp(baseNoise, -1, 1);

  if (maskNoise <= 0) {
    return getDefaultWater(maskNoise, float3(0, 0, 0));
  }

  // Independent low-frequency noise fields lay out where each biome can
  // appear, roughly in [-1, 1].
  float region1Noise = clamp(evaluate(position + 10, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1); // forest
  float region2Noise = clamp(evaluate(position + 20, 5, 3.5, regionScale, 5, .15f, .5f), -1, 1); // desert

  // White is the fallback land color. Colored biome masks are applied after it
  // so they replace white instead of being covered by a final snow blend.
  float4 landColor = getDefaultSnow(maskNoise * 0.35, rgbOffset);

  // smoothstep (instead of a hard > 0 branch) blends neighbouring biomes
  // across a margin instead of cutting a jagged, unnaturally sharp edge.
  float forestWeight = smoothstep(0.0, 0.35, region1Noise);
  float desertWeight = smoothstep(0.0, 0.35, region2Noise);

  landColor = lerp(landColor, getDefaultForest(region1Noise, rgbOffset), forestWeight);
  landColor = lerp(landColor, getDefaultDesert(region2Noise, rgbOffset), desertWeight);

  return saturate(landColor);
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

  float smoothness = planetType == 4 ? 0 : saturate(1 - baseNoise);

  // Solid planets (not pure ocean/gas) get ridged mountain ranges: a rocky
  // tint on high ground plus reduced smoothness there, since exposed
  // rock/scree is rougher than vegetation, sand or ice at the same
  // elevation. Reuses regionScale/rgbOffset so no new inputs are needed on
  // the Custom Function node.
  if (planetType != 4) {
    float mountains = getMountains(position, regionScale * 4.0, baseNoise);
    float3 rockColor = float3(0.35, 0.33, 0.30) + rgbOffset * 0.2;
    color.rgb = lerp(color.rgb, rockColor, mountains);
    smoothness = saturate(smoothness - mountains * 0.5);
  }

  smooth = smoothness;
}