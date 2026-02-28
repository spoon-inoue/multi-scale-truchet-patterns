#version 300 es
precision highp float;

uniform vec3 resolution;
uniform float time;

in vec2 vUv;
out vec4 O;

#define h(f2, f) hash(vec3(f2, f))

vec3 hash(vec3 v) {
  uvec3 x = floatBitsToUint(v + vec3(.1, .2, .3));
  x = (x >> 8 ^ x.yzx) * 0x456789ABu;
  x = (x >> 8 ^ x.yzx) * 0x6789AB45u;
  x = (x >> 8 ^ x.yzx) * 0x89AB4567u;
  return vec3(x) / vec3(-1u);
}


void main() {
  vec2 suv = vUv * float(SCALE);

  vec2 quv = suv, iuv;
  float i, fDetail = float(DETAIL);
  for(; i < float(DETAIL); i++) {
    iuv = floor(quv);
    if (h(iuv, fDetail).x < 0.4 + (i / fDetail) * 0.5) break;
    quv *= 2.0;
  }
  if (i == fDetail) --i;

  O.r = i;
}