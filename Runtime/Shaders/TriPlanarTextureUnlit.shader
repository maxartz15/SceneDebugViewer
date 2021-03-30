// https://www.ronja-tutorials.com/post/010-triplanar-mapping/

Shader "Hidden/TriPlanarTexture"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            sampler2D _RS_Texture;
            float4 _RS_Texture_ST;
            float _RS_Sharpness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //calculate UV coordinates for three projections
				float2 uv_front = TRANSFORM_TEX(i.worldPos.xy, _RS_Texture);
				float2 uv_side = TRANSFORM_TEX(i.worldPos.zy, _RS_Texture);
				float2 uv_top = TRANSFORM_TEX(i.worldPos.xz, _RS_Texture);

                //read texture at uv position of the three projections
				fixed4 col_front = tex2D(_RS_Texture, uv_front);
				fixed4 col_side = tex2D(_RS_Texture, uv_side);
				fixed4 col_top = tex2D(_RS_Texture, uv_top);

                //generate weights from world normals
				float3 weights = i.normal;
				//show texture on both sides of the object (positive and negative)
				weights = abs(weights);
				//make the transition sharper
				weights = pow(weights, _RS_Sharpness);
				//make it so the sum of all components is 1
				weights = weights / (weights.x + weights.y + weights.z);

				//combine weights with projected colors
				col_front *= weights.z;
				col_side *= weights.x;
				col_top *= weights.y;

				//combine the projected colors
				fixed4 col = col_front + col_side + col_top;

                return col;

                // return  fixed4(abs(i.normal.xyz), 1);
            }
            ENDCG
        }
    }
}
