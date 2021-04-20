// Ref: https://aras-p.info/blog/2011/05/03/a-way-to-visualize-mip-levels/

Shader "Hidden/RS/TexelDensityChecker"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "RSPropertiesCG.cginc"
			#include "RSUtilsCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uvmip : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				int mips = RS_MipCount(_MainTex_TexelSize);
				float2 mipuv = v.uv * (_MainTex_TexelSize.zw / mips);
				o.uvmip = TRANSFORM_TEX(mipuv, _MainTex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mip = tex2D(_RS_Texture, i.uvmip);

				fixed4 res;
				res.rgb = lerp(col.rgb, mip.rgb, mip.a);
				res.a = col.a;

				return res;
			}
			ENDCG
		}
	}
}