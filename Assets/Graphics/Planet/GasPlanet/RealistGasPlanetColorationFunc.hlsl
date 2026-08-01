#include "../ColorationMat/SimplexNoise.hlsl"
#include "../ColorationMat/PerlinNoise.hlsl"
#include "../ColorationMat/VoronoiNoise.hlsl"
#include "../ColorationMat/GradientFunc.hlsl"

float4 getGasColorByType(int planetType, float value, float3 rgbOffset, float3 rgbAmp, float3 rgbFreq, float3 rgbPhase) {
  switch (planetType) {
    case 0:
      return getDefaultHotGas(value, rgbOffset);
    case 1:
      return getDefaultColdGas(value, rgbOffset);
    case 2:
    default:
      return getDefaultType(value, rgbOffset, rgbAmp, rgbFreq, rgbPhase);
  }
}

float RealistGasFBM(float3 position, float baseFrequency, float turbulenceScale) {
  float amplitude = 0.5;
  float frequency = max(baseFrequency, 0.001);
  float sum = 0.0;
  float normalization = 0.0;
  float pixelFootprint = max(length(ddx(position)), length(ddy(position))) * turbulenceScale;

  [unroll]
  for (int i = 0; i < 4; i++) {
    // Fade out detail that is smaller than a pixel instead of letting it alias.
    float octaveFootprint = pixelFootprint * frequency;
    float octaveWeight = 1.0 - smoothstep(0.35, 1.25, octaveFootprint);
    sum += snoise(position * frequency) * amplitude * octaveWeight;
    normalization += amplitude * octaveWeight;
    frequency *= 2.0;
    amplitude *= 0.5;
  }

  return sum / max(normalization, 0.001);
}

// Realistic gas giant surface: latitude bands (Jupiter/Saturn style belts
// and zones) that meander and shear instead of sitting on perfectly straight
// parallels, plus rare storm vortices tinted from the planet's own palette.
//
// planetType         : 0 hot gas / 1 cold gas / 2 custom palette (rgbAmp/rgbFreq/rgbPhase)
// position           : sample position (object/world point on/around the planet sphere)
// bandFrequency      : number of latitude bands wrapped around the planet
// bandWarpScale      : frequency of the noise field used to bend the bands
// bandWarpStrength   : how much the bands meander/shear (0 = perfectly straight)
// bandMotionSpeed    : speed of the atmospheric deformation
// bandMotionStrength : 0 = static bands, higher values add moving shear
// turbulenceScale    : frequency of the fine cloud-top turbulence noise
// turbulenceStrength : 0..1 amount of fine turbulence layered onto the bands
// stormScale         : frequency of the Worley cell grid used to seed storms (lower = fewer, bigger cells)
// stormThreshold     : 0..1 rarity gate - only cells with a random id above this host a storm
// stormStrength      : 0..1 how strongly a storm's tint/swirl shows through
void RealistGasPlanetColorationFunc_float (
  int planetType,
  float3 position,
  float bandFrequency,
  float bandWarpScale,
  float bandWarpStrength,
  float bandMotionSpeed,
  float bandMotionStrength,
  float time,
  float turbulenceScale,
  float turbulenceStrength,
  float stormScale,
  float stormThreshold,
  float stormStrength,
  float3 rgbOffset,
  float3 rgbAmp,
  float3 rgbFreq,
  float3 rgbPhase,
  out float4 color
) {
  // 1) Latitude from the planet's polar axis. Working from the 3D surface
  //    position (not a 2D UV) keeps the bands seamless at the poles and
  //    across the longitude wrap - same trick used for RealistCloudFunc.
  float3 direction = normalize(position);
  float latitude = asin(clamp(direction.y, -1.0, 1.0)); // [-pi/2, pi/2]

  // 2) Warp the latitude with a low-frequency 3D noise field so belts/zones
  //    meander and shear like real jet streams instead of perfectly
  //    straight parallels.
  // Move the noise sample along the local east-west direction. The offset
  // reverses in the opposite hemisphere, which produces Coriolis-style shear
  // without breaking the latitude bands at the poles or longitude seam.
  float3 east = float3(-direction.z, 0.0, direction.x);
  float eastLength = max(length(east), 0.001);
  east /= eastLength;
  float coriolisShear = sin(latitude) * bandWarpStrength;
  float3 flowPosition = position + east * coriolisShear;
  float motionPhase = time * bandMotionSpeed;
  float motionOffset = sin(motionPhase) * bandMotionStrength;
  float3 movingFlowPosition = flowPosition
                            + east * sin(latitude) * motionOffset * 0.75
                            + direction * cos(motionPhase * 0.37) * bandMotionStrength * 0.2;
  float warp = RealistGasFBM(movingFlowPosition, bandWarpScale, bandWarpScale);
  float motionWarp = RealistGasFBM(movingFlowPosition + east * motionOffset * 0.5,
                                   bandWarpScale * 0.6,
                                   bandWarpScale * 0.6);
  float warpedLatitude = latitude + warp * bandWarpStrength * 0.35;
  warpedLatitude += motionWarp * bandMotionStrength * 0.35;

  // 3) Two sine harmonics of different frequency/phase give bands of
  //    irregular width - Jupiter's belts and zones are not evenly spaced,
  //    unlike a single raw sine wave.
  float bandPattern = sin(warpedLatitude * bandFrequency) * 0.65
                     + sin(warpedLatitude * bandFrequency * 2.3 + 1.7) * 0.35;

  // 4) Fine cloud-top turbulence layered on top of the bands.
  float turbulence = RealistGasFBM(movingFlowPosition, turbulenceScale, turbulenceScale) * turbulenceStrength;

  float value = saturate(bandPattern * 0.5 + 0.5 + turbulence);

  color = getGasColorByType(planetType, value, rgbOffset, rgbAmp, rgbFreq, rgbPhase);

  // 5) Sparse storm vortices (a "Great Red Spot" style feature): a Worley
  //    cell grid seeds candidate storm locations, stormThreshold keeps only
  //    a few of them, distance-to-center drives a swirling ring pattern,
  //    and the tint is pulled from the same palette so it always matches
  //    the planet's color scheme instead of a hardcoded storm color.
  float3 cell = vnoise(position, stormScale, 50.0);
  float cellId = cell.z;
  float cellDistance = saturate(cell.x);
  float hasStorm = step(stormThreshold, cellId);
  float stormMask = hasStorm * saturate(1.0 - cellDistance);
  float stormSwirl = sin(cellDistance * 20.0) * 0.5 + 0.5;

  float4 stormColor = getGasColorByType(planetType, saturate(1.0 - value + stormSwirl * 0.2), rgbOffset, rgbAmp, rgbFreq, rgbPhase);
  color = lerp(color, stormColor, stormMask * stormStrength);
}
