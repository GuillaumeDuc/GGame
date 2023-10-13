#include "../ColorationMat/SimplexNoise.hlsl"
#include "../ColorationMat/PerlinNoise.hlsl"
#include "../ColorationMat/VoronoiNoise.hlsl"
#include "../ColorationMat/GradientFunc.hlsl"

float4 getColorByPlanetType(int planetType, float baseNoise, float3 rgbOffset, float3 rgbAmp, float3 rgbFreq, float3 rgbPhase) {
  switch(planetType) {
    case 0:
      return getDefaultHotGas(baseNoise, rgbOffset);
    case 1:
      return getDefaultColdGas(baseNoise, rgbOffset);
    case 2:
    default:
      return getDefaultType(baseNoise, rgbOffset, rgbAmp, rgbFreq, rgbPhase);
  }
}

void GasPlanetColorationFunc_float (
  int planetType,
  float3 position,
  float3 absNoise,
  float3 circleNoise,
  float3 snoise,
  float3 rgbOffset,
  float3 rgbAmp,
  float3 rgbFreq,
  float3 rgbPhase,
  out float4 color
  ) {
  float baseNoise = clamp(circleNoise + snoise + absNoise, 0, 1);

  color = getColorByPlanetType(planetType, baseNoise, rgbOffset, rgbAmp, rgbFreq, rgbPhase);
}