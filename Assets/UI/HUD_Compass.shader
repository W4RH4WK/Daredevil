Shader "Daredevil/UI/HUD/Compass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaMap ("AlphaMap", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Opaque"
            "PreviewType" = "Plane"
        }
        LOD 100

        Pass
        {
			Blend SrcAlpha One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 alpha_uv : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 alpha_uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _AlphaMap;
            float4 _AlphaMap_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.alpha_uv = TRANSFORM_TEX(v.uv, _AlphaMap);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 alpha = tex2D(_AlphaMap, i.alpha_uv);
                col.a = alpha.a;
                return col;
            }
            ENDCG
        }
    }
}
