#ifndef VoronoiNoise
#define VoronoiNoise 1
inline float3 voronoi_noise_randomVector (float3 UV, float offset){
    float3x3 m = float3x3(15.27, 47.63, 99.41, 89.98, 95.07, 38.39, 33.83, 51.06, 60.77);
    UV = frac(sin(mul(UV, m)) * 46839.32);
    return float3(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5, sin(UV.z*offset)*0.5+0.5);
}

float3 vnoise(float3 position, float scale, float offsetAngle) {
  float3 g = floor(position * scale);
  float3 f = frac(position * scale);
  float3 res = float3(8.0, 8.0, 8.0);
 
  for(int y=-1; y<=1; y++)
  {
    for(int x=-1; x<=1; x++){
      for(int z=-1; z<=1; z++){
        float3 lattice = float3(x, y, z);
        float3 offset = voronoi_noise_randomVector(g + lattice, offsetAngle);
        float3 v = lattice + offset - f;
        float d = dot(v, v);
                
        if(d < res.x) 
        {
          res.y = res.x;
          res.x = d;
          res.z = offset.x;
        } 
        else if (d < res.y)
        {
          res.y = d;
        }
      }
    }
  }
  return res;
}

void VoronoiNoise_float (float3 position, float scale, float offsetAngle, out float3 Out, out float Cells) {
  float3 v = vnoise(position, scale, offsetAngle);
  Out = v.x;
  Cells = v.z;
}
#endif