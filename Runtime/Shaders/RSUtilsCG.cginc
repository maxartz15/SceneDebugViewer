#ifndef RS_UTILS_CG_INCLUDED
#define RS_UTILS_CG_INCLUDED

// https://www.ronja-tutorials.com/post/010-triplanar-mapping/
fixed4 SampleTriPlanar(sampler2D tex, float4 tex_ST, float3 position, float3 normal, float sharpness)
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

#endif