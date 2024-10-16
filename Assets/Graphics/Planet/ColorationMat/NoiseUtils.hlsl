#ifndef NoiseUtils
#define NoiseUtils 1
float4 mod289(float4 x)
{
  return x - floor(x / 289.0) * 289.0;
}

float3 mod289(float3 x)
{
  return x - floor(x / 289.0) * 289.0;
}

float4 permute(float4 x)
{
  return mod289(((x*34.0)+1.0)*x);
}

float4 taylorInvSqrt(float4 r)
{
  return (float4)1.79284291400159 - r * 0.85373472095314;
}

float3 fade(float3 t) {
  return t*t*t*(t*(t*6.0-15.0)+10.0);
}

float3 mod(float3 x, float3 y)
{
  return x - y * floor(x / y);
}
#endif
