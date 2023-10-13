#ifndef GradientFunc
#define GradientFunc 1

float3 grad (float t, float3 a, float3 b, float3 c, float3 d) {
  return a + b * cos(6.28318f * (c * t + d));
}

float4 float3To4 (float3 c) {
  return float4(c.x, c.y, c.z, 0);
}

float4 getDefaultWater(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(0.058, -0.112, 0.718) + rgbOffset,
    float3(0, 0, 0.308),
    float3(1, 1, 0.558),
    float3(0, 0.333, 0.968)
  ), 0, 1));
}

float4 getDefaultForest(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(0.108, 0.338, 0.108) + rgbOffset,
    float3(0.028, 0.078, -0.032),
    float3(1, -0.832, 1),
    float3(0, 0.028, 0.667)
  ), 0, 1));
}

float4 getDefaultDesert(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(0.668, 0.338, 0.058) + rgbOffset,
    float3(-0.032, -0.252, -0.032),
    float3(-0.502, 0.278, 1.000),
    float3(-0.532, -0.532, 0.667)
  ), 0, 1));
}

float4 getDefaultSnow(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(0.998, 0.888, 0.998) + rgbOffset,
    float3(-0.472, -0.192, -0.192),
    float3(0.498, -0.502, 0.498),
    float3(-0.922, -0.082, -0.032)
  ), 0, 1));
}

float4 getDefaultType(float value) {
  return float3To4(clamp(grad(
    value,
    float3(0.448, 0.448, 0.498),
    float3(2.358, -0.532, -0.252),
    float3(0.442, -0.672, -0.442),
    float3(-1.532, -1.032, -0.752)
  ), 0, 1));
}

float4 getDefaultHotGas(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(0.58, 0.12 , -0.16) + rgbOffset,
    float3(0.04, 0.04, 0.09),
    float3(0.79, -0.93, 0.29),
    float3(0.48, 0.49, 0.35)
  ), 0, 1));
}

float4 getDefaultColdGas(float value, float3 rgbOffset) {
  return float3To4(clamp(grad(
    value,
    float3(-1.052, -1.172, 1.558),
    float3(2.088, 2.358, -1.192),
    float3(-0.552, -0.552, -0.332),
    float3(2.528, 2.528, 0.698)
  ), 0, 1));
}

float4 getDefaultType(float value, float3 rgbOffset, float3 rgbAmp, float3 rgbFreq, float3 rgbPhase) {
  return float3To4(clamp(grad(
    value,
    float3(0, 0, 0) + rgbOffset, // DC Offset
    float3(0, 0, 0) + rgbAmp, // Amp
    float3(0, 0, 0) + rgbFreq, // Freq
    float3(0, 0, 0) + rgbPhase // Phase
  ), 0, 1));
}

void GradientFunc_float (float value, float type, out float4 Colors) {
  Colors = getDefaultType(value);
}
#endif