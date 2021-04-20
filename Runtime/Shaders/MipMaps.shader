// Ref: https://developer.nvidia.com/gpugems/gpugems2/part-iii-high-quality-rendering/chapter-28-mipmap-level-measurement
// https://github.com/jintiao/MipmapLevel/blob/master/Assets/MipmapColor.shader

Shader "Hidden/RS/MipMaps"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
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
            float4 _MipMapColors[15]; // max mipmaps = 1 + floor(log2(maxTexSize))
            int _Max;

            v2f vert(appdata v)
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
                int mipLevels = 1 + floor(log2(max(_MainTex_TexelSize.z, _MainTex_TexelSize.w)));

                // int m = RS_MipMap(i.uvmip, _MainTex_TexelSize);
                int m = RS_M(i.uvmip, _MainTex_TexelSize);
                // int m = RS_Mip(i.uvmip, _MainTex_TexelSize, mipLevels);
                // m = clamp(m, 0, _Max);

                // m = RS_Remap(m, 0, 15, 0, _Max);

                fixed4 mip = _MipMapColors[m];

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 res;
                res.rgb = lerp(col.rgb, mip.rgb, mip.a);
                res.a = col.a;

                return res;
            }
            ENDCG
        }
    }
}