#ifndef RS_UTILS_CG_INCLUDED
#define RS_UTILS_CG_INCLUDED

// https://www.ronja-tutorials.com/post/010-triplanar-mapping/
fixed4 RS_TriPlanar(sampler2D tex, float4 tex_ST, float3 position, float3 normal, float sharpness)
{
	//calculate UV coordinates for three projections
	float2 uv_front = TRANSFORM_TEX(position.xy, tex);
	float2 uv_side = TRANSFORM_TEX(position.zy, tex);
	float2 uv_top = TRANSFORM_TEX(position.xz, tex);

	//read texture at uv position of the three projections
	fixed4 col_front = tex2D(tex, uv_front);
	fixed4 col_side = tex2D(tex, uv_side);
	fixed4 col_top = tex2D(tex, uv_top);

	//generate weights from world normals
	float3 weights = normal;
	//show texture on both sides of the object (positive and negative)
	weights = abs(weights);
	//make the transition sharper
	weights = pow(weights, sharpness);
	//make it so the sum of all components is 1
	weights = weights / (weights.x + weights.y + weights.z);

	//combine weights with projected colors
	col_front *= weights.z;
	col_side *= weights.x;
	col_top *= weights.y;

	//combine the projected colors
	fixed4 col = col_front + col_side + col_top;

	return col;
}

float RS_Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
{
	return (newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
}

int RS_MipCount(float4 texelSize)
{
	int m = max(texelSize.z, texelSize.w);
	int mip = 1 + floor(log2(m));

	return mip;
}

// https://github.com/jintiao/MipmapLevel/blob/master/Assets/MipmapColor.shader
int RS_MipMap(float2 uv, float4 texelSize)
{
	// Mipmap calculation.
	float2 muv = uv * texelSize.zw;
	float2 dx = ddx(muv);
	float2 dy = ddy(muv);

//#if 0
//	float rho = max(sqrt(dot(dx, dx)), sqrt(dot(dy, dy)));
//	float lambda = log2(rho);
//#else
	float rho = max(dot(dx, dx), dot(dy, dy));
	float lambda = 0.5 * log2(rho);
//#endif

	return max(int(lambda + 0.5), 0);
}

float RS_Mip(float2 uv, float4 texelSize, int mipLevels)
{
	float2 muv = uv * texelSize.zw;

	float2 derivX = ddx(muv);
	float2 derivY = ddy(muv);

	float delta_max_sqr = max(dot(derivX, derivX), dot(derivY, derivY));
	float mip = 0.5 * log2(delta_max_sqr) * mipLevels;
	
	return mip;
}

// https://gamedev.stackexchange.com/questions/28401/detect-mip-mapping-level-in-the-shader
float RS_M(float2 uv, float4 texelSize)
{
	float2 dx = ddx(uv * texelSize.zw);
	float2 dy = ddy(uv * texelSize.zw);
	float d = max(dot(dx, dx), dot(dy, dy));

	// Clamp the value to the max mip level counts
	const float rangeClamp = pow(2.0, (RS_MipCount(texelSize) - 1) * 2.0);
	d = clamp(d, 1.0, rangeClamp);

	float mipLevel = 0.5 * log2(d);
	mipLevel = floor(mipLevel);

	return mipLevel;
}
#endif