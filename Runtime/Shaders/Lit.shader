Shader "Hidden/RS/Lit"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        #include "RSPropertiesCG.cginc"

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_RS_Texture, IN.uv_MainTex) * _RS_Color;
            o.Albedo = c.rgb;
            o.Metallic = _RS_Metallic;
            o.Smoothness = _RS_Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
