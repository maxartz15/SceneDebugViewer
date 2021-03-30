Shader "Hidden/Surface"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _RS_Texture;
        fixed4 _RS_Color;
        half _RS_Metallic;
        half _RS_Glossiness;

        struct Input
        {
            float2 uv_RS_Texture;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = tex2D(_RS_Texture, IN.uv_RS_Texture) * _RS_Color;
            o.Metallic = _RS_Metallic;
            o.Smoothness = _RS_Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
