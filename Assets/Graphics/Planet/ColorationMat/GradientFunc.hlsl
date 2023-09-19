#ifndef GradientFunc
#define GradientFunc 1

float3 grad (float t, float3 a, float3 b, float3 c, float3 d) {
  return a + b * cos(6.28318f * (c * t + d));
}

float4 float3To4 (float3 c) {
  return float4(c.x, c.y, c.z, 0);
}

float4 getDefaultWater() {
  return float4(0, .2876f, 1, 0);
}

float4 getDefaultForest(float value) {
  return float3To4(clamp(grad(
    value,
    float3(0.108, 0.338, 0.108),
    float3(0.028, 0.078, -0.032),
    float3(1, -0.832, 1),
    float3(0, 0.028, 0.667)
  ), 0, 1));
}

float4 getDefaultDesert(float value) {
  return float3To4(clamp(grad(
    value,
    float3(0.668, 0.338, 0.058),
    float3(-0.032, -0.252, -0.032),
    float3(-0.502, 0.278, 1.000),
    float3(-0.532, -0.532, 0.667)
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

float4 getGradient (float value, float type) {
  switch(type)
  {
    case 0:
      return getDefaultWater();
    case 1:
      return getDefaultForest(value);  
    default:
      return getDefaultDesert(value);
  }
}

float4 getGradient (float value) {
  return getGradient(value, 0);
}

void GradientFunc_float (float value, float type, out float4 Colors) {
  Colors = getGradient(value, type);
}
#endif