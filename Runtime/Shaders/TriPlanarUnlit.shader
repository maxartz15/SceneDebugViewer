Shader "Hidden/RS/TriPlanarUnlit"
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
			#include "RSPropertiesCG.cginc"
			#include "RSUtilsCG.cginc"

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = RS_TriPlanar(_RS_Texture, _RS_Texture_ST, i.worldPos, i.normal, _RS_Sharpness);
				c *= _RS_Color;
	            return c;
			}
			ENDCG
		}
	}
}
