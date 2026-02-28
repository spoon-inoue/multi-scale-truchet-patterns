#version 300 es
precision highp float;

uniform sampler2D layerMap;
uniform vec3 resolution;
uniform float time;

in vec2 vUv;
out vec4 O;

#define h(f2, f) hash(vec3(f2, f))
#define rotUv(uv, a) ((uv - 0.5) * rot(a) + 0.5)

const float PI = acos(-1.);
const float D3 = 1. / 3.;
const float D6 = 1. / 6.;
const float SMOOTH = 0.003;

mat2 rot(float a) { return mat2(cos(a), sin(a), -sin(a), cos(a)); }

// float r(vec2 uv, vec2 center, float r) { return step(distance(uv, center), r); }
float r(vec2 uv, vec2 center, float r) { return smoothstep(r + SMOOTH, r - SMOOTH, distance(uv, center)); }
float r3(vec2 uv, vec2 center) { return r(uv, center, D3); }
float r6(vec2 uv, vec2 center) { return r(uv, center, D6); }
float lh(vec2 uv) { return 1. - step(abs(uv.y * 2. - 1.), D3); }
float lv(vec2 uv) { return lh((uv - 0.5) * rot(PI * 0.5) + 0.5); }

#include './patterns.glsl'

vec3 hash(vec3 v) {
  uvec3 x = floatBitsToUint(v + vec3(0.1, 0.2, 0.3));
  x = (x >> 8 ^ x.yzx) * 0x456789ABu;
  x = (x >> 8 ^ x.yzx) * 0x6789AB45u;
  x = (x >> 8 ^ x.yzx) * 0x89AB4567u;
  return vec3(x) / vec3(-1u);
}

vec2 coveredScale() {
  float textureAspect = 1.;
  float aspect = resolution.x / resolution.y;
  if (textureAspect < aspect) {
    return vec2(aspect / textureAspect, 1);
  } else {
    return vec2(1, textureAspect / aspect);
  }
}

vec4 getLayerPattern(vec2 uv, float layer, float targetDetail) {
  vec2 suv = uv * float(SCALE);
  
  float powTD = pow(2., targetDetail);
  vec2 duv = suv * powTD;
  vec2 fuv = fract(duv);
  vec2 iuv = floor(duv) / float(SCALE) / powTD;
  vec2 px = vec2(1) / float(SCALE) / powTD;

  vec4 col;

  // draw pattern
  if (layer == targetDetail) {
    float h = h(iuv, .1).x;
    float m = mod(floor(iuv.y * 1.5), 6.);

    if (m == 0.) {
      if      (h < 0.25) col += pattern_8(fuv);
      else if (h < 0.50) col += pattern_9(fuv);
      else if (h < 0.75) col += pattern_10(fuv);
      else               col += pattern_11(fuv);
    } else if (m == 1.) {
      if      (h < 0.33) col += pattern_7(fuv);
      else if (h < 0.66) col += pattern_1(fuv);
      else               col += pattern_2(fuv);
    } else if (m == 2.) {
      if      (h < 0.25) col += pattern_1(fuv);
      else if (h < 0.50) col += pattern_3(fuv);
      else if (h < 0.75) col += pattern_1(fuv);
      else               col += pattern_2(fuv);
    } else if (m == 3.) {
      if      (h < 0.50) col += pattern_3(fuv);
      else               col += pattern_4(fuv);
    } else if (m == 4.) {
      if      (h < 0.33) col += pattern_1(fuv);
      else if (h < 0.66) col += pattern_10(fuv);
      else               col += pattern_6(fuv);
    } else {
      if      (h < 0.50) col += pattern_7(fuv);
      else               col += pattern_5(fuv);
    }

    col.a = 1.;
  }

  // draw wing
  vec3 p = vec3(1, 0.5, 0);

  for (int iy = -1; iy <= 1; iy++) {
    for (int ix = -1; ix <= 1; ix++) {
      if (ix == 0 && iy == 0) continue;

      vec2 neighbor_uv = iuv + px * (vec2(ix, iy) + 0.5);
      float neighbor_detail = texture(layerMap, neighbor_uv).r;

      if (layer != neighbor_detail && neighbor_detail == targetDetail) {
        if (ix == -1 && iy == -1) {
          col   += r3(fuv, p.zz);
        } else if (ix == 1 && iy == -1) {
          col   += r3(fuv, p.xz);
        } else if (ix == -1 && iy == 1) {
          col   += r3(fuv, p.zx);
        } else if (ix == 1 && iy == 1) {
          col   += r3(fuv, p.xx);
        } else if (ix == 0 && iy == -1) {
          col   += r3(fuv, p.zz);
          col   += r3(fuv, p.xz);
          col.a += r6(fuv, p.yz);
        } else if (ix == 0 && iy == 1) {
          col   += r3(fuv, p.zx);
          col   += r3(fuv, p.xx);
          col.a += r6(fuv, p.yx);
        } else if (ix == -1 && iy == 0) {
          col   += r3(fuv, p.zx);
          col   += r3(fuv, p.zz);
          col.a += r6(fuv, p.zy);
        } else if (ix == 1 && iy == 0) {
          col   += r3(fuv, p.xx);
          col   += r3(fuv, p.xz);
          col.a += r6(fuv, p.xy);
        }
      }
    } 
  }

  col = clamp(col, 0., 1.);

  if (mod(targetDetail, 2.) == 1.) {
    col.rgb = 1. - col.rgb;
  }

  return col;
}

void main() {
  vec2 uv = (vUv - 0.5) * coveredScale() + 0.5;
  uv.y -= time * 0.1;
  float layer = texture(layerMap, uv).r;

  for(float i; i < float(DETAIL); i++) {
    vec4 pattern = getLayerPattern(uv, layer, i);
    O = mix(O, pattern, pattern.a);
  }
}