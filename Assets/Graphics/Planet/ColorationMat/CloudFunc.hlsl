#include "SimplexNoise.hlsl"
#include "PerlinNoise.hlsl"
#include "VoronoiNoise.hlsl"


void CloudFunc_float (
  float3 position,
  float mainCloudScale,
  float mainCloudStrength,
  float exclusionScale,
  float exclusionStrength,
  float extraCloudScale,
  float extraCloudFade,
  out float3 Out
) {
  float3 excludingMask = snoise(position * exclusionScale) + exclusionStrength;
  float3 mainColor = 1 - abs(cnoise(position * mainCloudScale)) - excludingMask;

  float3 voronoi = mainColor - vnoise(position, extraCloudScale, 50).x;;
  float3 simplex = mainColor - snoise((position + (30, 30, 30)) * extraCloudScale);
  float3 extraCloudNoises = (voronoi + simplex) / extraCloudFade;

  mainColor = (mainColor + extraCloudNoises) / mainCloudStrength;

  Out = clamp(mainColor, 0, 1);
}