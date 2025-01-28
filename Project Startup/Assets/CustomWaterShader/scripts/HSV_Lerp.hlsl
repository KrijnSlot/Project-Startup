half3 RGBToHSV(half3 In)
{
	half4 K = half4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	half4 P = lerp(half4(In.bg, K.wz), half4(In.gb, K.xy), step(In.b, In.g));
	half4 Q = lerp(half4(P.xyw, In.r), half4(In.r, P.yzx), step(P.x, In.r));
	half D = Q.x - min(Q.w, Q.y);
	half E = 1e-10;
	return half3(abs(Q.z + (Q.w - Q.y) / (6.0 * D + E)), D / (Q.x + E), Q.x);
}