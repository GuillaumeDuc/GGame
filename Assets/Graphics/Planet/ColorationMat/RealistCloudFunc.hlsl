#include "SimplexNoise.hlsl"
#include "VoronoiNoise.hlsl"

// Multi-octave fractal Brownian motion built on top of simplex noise.
// Each octave adds finer detail at a higher frequency (lacunarity) and a
// lower contribution (persistence), which is what gives natural phenomena
// (clouds, terrain, smoke) their "self-similar at every zoom level" look,
// instead of the single soft blob you get from one raw noise sample.
float RealistCloudFBM(float3 position, float lacunarity, float persistence)
{
  float amplitude = 0.5;
  float frequency = 1.0;
  float sum = 0.0;
  float normalization = 0.0;

  // 5 octaves is a good detail/perf tradeoff for a per-pixel planet shader.
  // The loop bound must be a compile-time constant so Shader Graph can
  // unroll it like any other HLSL custom function.
  [unroll]
  for (int i = 0; i < 5; i++)
  {
    sum += amplitude * snoise(position * frequency);
    normalization += amplitude;
    amplitude *= persistence;
    frequency *= lacunarity;
  }

  return sum / normalization; // back to roughly [-1, 1]
}

// Realistic planetary cloud coverage.
//
// position            : sample position (object/world space point on/around the planet sphere)
// cloudScale           : base frequency of the cloud masses (lower = bigger continents of cloud)
// coverage             : 0..1 overall sky fraction covered by clouds (0 = clear, 1 = overcast)
// sharpness            : contrast of the cloud edges (higher = harder-edged cumulus, lower = hazy)
// domainWarpScale      : frequency of the warp field used to bend the cloud shapes
// domainWarpStrength   : how much the warp field distorts the sampling position (turbulence/wind shear)
// erosionScale         : frequency of the cellular (Worley) noise used to eat away cloud edges
// erosionStrength      : 0..1 amount of cauliflower-like erosion applied to cloud edges
// bandingStrength      : 0..1 blend factor for latitude cloud banding (0 = ignore, 1 = fully banded)
// bandingFrequency     : number of wet/dry latitude bands wrapped around the planet
void RealistCloudFunc_float (
  float3 position,
  float cloudScale,
  float coverage,
  float sharpness,
  float domainWarpScale,
  float domainWarpStrength,
  float erosionScale,
  float erosionStrength,
  float bandingStrength,
  float bandingFrequency,
  out float3 Out
) {
  // 1) Domain warp: offset the sample position with its own low-frequency
  //    noise field before evaluating the cloud shape. This bends the cloud
  //    masses the way wind shear bends real cloud fronts, avoiding the
  //    "perfectly round noise blob" look.
  float3 warp = float3(
    snoise(position * domainWarpScale + float3(0.0, 0.0, 0.0)),
    snoise(position * domainWarpScale + float3(19.1, 7.3, 3.7)),
    snoise(position * domainWarpScale + float3(41.2, 2.1, 9.4))
  );
  float3 warpedPosition = position + warp * domainWarpStrength;

  // 2) Base cloud silhouette from multi-octave FBM, remapped to [0, 1].
  float baseShape = RealistCloudFBM(warpedPosition * cloudScale, 2.0, 0.5);
  baseShape = baseShape * 0.5 + 0.5;

  // 3) Coverage remap: shifting the density by (coverage - 1) before
  //    contrasting it with "sharpness" makes coverage behave like an
  //    overcast-amount slider, a common trick in real-time volumetric cloud
  //    rendering (e.g. Perlin-Worley coverage remaps used for planet/sky clouds).
  float density = saturate((baseShape + coverage - 1.0) * sharpness);

  // 4) Erosion: carve cellular (Worley) detail into the edges so cloud
  //    borders look fluffy/cauliflower-like instead of a smooth gradient.
  float cellDistance = saturate(vnoise(warpedPosition, erosionScale, 50).x);
  density = saturate(density - cellDistance * erosionStrength * density);

  // 5) Latitude banding: real atmospheres organise cloud cover into bands
  //    driven by global circulation cells - moist convection near the
  //    equator (ITCZ), dry subtropical belts around +-30 degrees, and
  //    temperate storm tracks further towards the poles. Approximate that
  //    from the "polar axis" component of the sample position.
  float latitude = asin(clamp(normalize(position).y, -1.0, 1.0)); // [-pi/2, pi/2]
  float bandPattern = 0.5 + 0.5 * cos(latitude * bandingFrequency * 2.0);
  float bandMask = lerp(1.0, bandPattern, bandingStrength);

  density = saturate(density * bandMask);

  Out = density.xxx;
}
