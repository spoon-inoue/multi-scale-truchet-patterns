float pattern_1(vec2 uv) {
  // \
  float p = 1.;
  p *= 1. - r(uv, vec2(1, 1), D3 * 2.);
  p += r3(uv, vec2(1, 1));
  p *= 1. - r(uv, vec2(0, 0), D3 * 2.);
  p += r3(uv, vec2(0, 0));
  return p;
}

float pattern_2(vec2 uv) {
  // /
  return pattern_1(rotUv(uv, PI * 0.5));
}

float pattern_3(vec2 uv) {
  // -
  float p = 1.;
  p *= 1. - r6(uv, vec2(0.5, 1));
  p *= 1. - r6(uv, vec2(0.5, 0));
  p *= lh(uv);
  return p;
}

float pattern_4(vec2 uv) {
  // |
  return pattern_3(rotUv(uv, PI * 0.5));
}

float pattern_5(vec2 uv) {
  // +.
  float p = 1.;
  p *= 1. - r6(uv, vec2(0.5, 0));
  p *= 1. - r6(uv, vec2(0.5, 1));
  p *= 1. - r6(uv, vec2(0, 0.5));
  p *= 1. - r6(uv, vec2(1, 0.5));
  return p;
}

float pattern_6(vec2 uv) {
  // x.
  float p;
  p += r3(uv, vec2(0, 0));
  p += r3(uv, vec2(0, 1));
  p += r3(uv, vec2(1, 0));
  p += r3(uv, vec2(1, 1));
  return p;
}

float pattern_7(vec2 uv) {
  // +
  float p = 1.;
  p *= lh(uv);
  p *= lv(uv);
  return p;
} 

float pattern_8(vec2 uv) {
  // fne
  float p = 1.;
  p *= 1. - r6(uv, vec2(0, 0.5));
  p *= 1. - r6(uv, vec2(0.5, 0));
  p *= 1. - r(uv, vec2(1, 1), D3 * 2.);
  p += r3(uv, vec2(1, 1));
  return p;
}

float pattern_9(vec2 uv) {
  // fsw
  return pattern_8(rotUv(uv, PI));
}

float pattern_10(vec2 uv) {
  // fnw
  return pattern_8(rotUv(uv, PI * 0.5));
}

float pattern_11(vec2 uv) {
  // fse
  return pattern_8(rotUv(uv, PI * 1.5));
}

float pattern_12(vec2 uv) {
  // tn
  float p;
  p += r3(uv, vec2(0, 1));
  p += r3(uv, vec2(1, 1));
  p += step(abs(uv.y), D3);
  p *= 1. - r6(uv, vec2(0.5, 0));
  return p;
}

float pattern_13(vec2 uv) {
  // ts
  return pattern_12(rotUv(uv, PI));
}

float pattern_14(vec2 uv) {
  // te
  return pattern_12(rotUv(uv, PI * 1.5));
}

float pattern_15(vec2 uv) {
  // tw
  return pattern_12(rotUv(uv, PI * 0.5));
}

float select_pattern(vec2 uv, float number) {
  if      (number ==  1.) { return pattern_1(uv); }
  else if (number ==  2.) { return pattern_2(uv); }
  else if (number ==  3.) { return pattern_3(uv); }
  else if (number ==  4.) { return pattern_4(uv); }
  else if (number ==  5.) { return pattern_5(uv); }
  else if (number ==  6.) { return pattern_6(uv); }
  else if (number ==  7.) { return pattern_7(uv); }
  else if (number ==  8.) { return pattern_8(uv); }
  else if (number ==  9.) { return pattern_9(uv); }
  else if (number == 10.) { return pattern_10(uv); }
  else if (number == 11.) { return pattern_11(uv); }
  else if (number == 12.) { return pattern_12(uv); }
  else if (number == 13.) { return pattern_13(uv); }
  else if (number == 14.) { return pattern_14(uv); }
  else if (number == 15.) { return pattern_15(uv); }
}