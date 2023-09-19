#ifndef CustomFresnel
#define CustomFresnel 1

void CustomFresnel_float(float3 WorldPos, float4 DefaultFresnel, out half4 Color) {
#if SHADERGRAPH_PREVIEW
   Color = DefaultFresnel;
#else
#if SHADOWS_SCREEN
   half4 clipPos = TransformWorldToHClip(WorldPos);
   half4 shadowCoord = ComputeScreenPos(clipPos);
#else
   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif
   Light mainLight = GetMainLight(shadowCoord);
   float4 f = clamp(DefaultFresnel - (1 - (mainLight.distanceAttenuation * mainLight.shadowAttenuation)), 0, 1);
   Color = f;
#endif
}
#endif