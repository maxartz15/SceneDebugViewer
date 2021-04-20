Shader "Hidden/RS/TriPlanarLit"
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

        #include "RSPropertiesCG.cginc"
        #include "RSUtilsCG.cginc"

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = RS_TriPlanar(_RS_Texture, _RS_Texture_ST, IN.worldPos, IN.worldNormal, _RS_Sharpness);

            o.Albedo = c * _RS_Color;
            o.Metallic = _RS_Metallic;
            o.Smoothness = _RS_Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}