Shader "BoredomAndDungeons/ModernHandheldShadow"
{
    Properties
    {
        _MainTex ("Shadow Mask", 2D) = "white" {}
        _Color ("Shadow Color", Color) = (0,0,0,0.7)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent-40"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        Cull Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MainTex, i.uv);
                return fixed4(_Color.rgb, mask.a * _Color.a);
            }
            ENDCG
        }
    }
}
