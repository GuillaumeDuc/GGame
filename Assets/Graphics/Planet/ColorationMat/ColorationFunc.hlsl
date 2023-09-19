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

float4 getRegions (float3 position, float regionScale, float baseNoise, float details, float mountains) {
  float maskNoise = clamp(baseNoise, -1, 1);
  float region1Noise = clamp(snoise((position + 5) * regionScale), -1, 1);
  float region2Noise = clamp(snoise((position + 7) * regionScale), -1, 1);
  region1Noise += region1Noise <= 0 ? 0 : details;
  region2Noise += region2Noise <= 0 ? 0 : details;
  float totalNoises = region1Noise + region2Noise;

  if (region1Noise > 0 && region2Noise > 0 && maskNoise > 0) {
    return clamp(lerp(getGradient(region1Noise, 1), getGradient(region2Noise, 2), region2Noise), 0, 1) * (1 - mountains);
  }
  else if (region1Noise > 0 && maskNoise > 0) {
    return getGradient(region1Noise, 1) * (1 - mountains);
  } else if (region2Noise > 0 && maskNoise > 0) {
    return getGradient(region2Noise, 2) * (1 - mountains);
  }
  return getOcean(maskNoise);
}

float evaluate(float3 position, int numLayers, float strength, float baseRoughness, float roughness, float persistence, float minValue) {
  float noiseValue = 0;
  float frequency = baseRoughness;
  float amplitude = 1;

  for (int i = 0; i < numLayers; i++) {
    float v = snoise(position * frequency);
    noiseValue += (v + 1) * .5f * amplitude;
    frequency *= roughness;
    amplitude *= persistence;
  }

  noiseValue = noiseValue - minValue;
  return noiseValue * strength;
}

float getMountains (float3 position, float mountainScale, float baseNoise) {
  float mountains = (1 - abs(cnoise((position + 10) * mountainScale))) - (1 - baseNoise);
  return clamp(mountains, 0, 1) / 2;
}

float getDetails (float3 position, float detailScale, float baseNoise) {
  float sn = clamp(snoise((position + 2) * detailScale) + snoise((position + 4) * detailScale * 6) + snoise((position + 3) * detailScale * 8), -1, 1);
  sn = clamp(((1 - (sn - baseNoise)) * baseNoise), 0, 1) / 5;
  return sn;
}

void ColorationFunc_float (
  float3 position,
  float scale,
  float baseStrength,
  float regionScale,
  float mountainScale,
  float detailScale,
  out float4 color,
  out float3 smooth,
  out float4 outEv
  ) {
  // float testnoise = evaluate(position, 5, .5f, 5, 5, 0.6f, 1.3f);
  // outEv = getGradient(testnoise, 1);
  float baseNoise = cnoise(position * scale) + baseStrength;
  float mountains = getMountains(position, mountainScale, baseNoise);
  float details = getDetails(position, detailScale, baseNoise);
  float4 regions = getRegions(position, regionScale, baseNoise, details, mountains);

  color = regions;
  smooth = 1 - baseNoise;
}