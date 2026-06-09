Shader "BoredomAndDungeons/ModernHandheldButtonCap"
{
    Properties
    {
        _MainTex ("Button Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Highlight ("Highlight", Range(0,1)) = 0.22
        _EdgeGlow ("Edge Glow", Color) = (0.2,0.45,1,0.25)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent+5"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Highlight;
            fixed4 _EdgeGlow;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                clip(tex.a - 0.025);

                float3 n = normalize(i.worldNormal);
                float3 v = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float3 key = normalize(float3(0.56, 0.72, -0.40));
                float diffuse = 0.76 + 0.24 * saturate(dot(n, key));
                float fresnel = pow(1.0 - saturate(abs(dot(n, v))), 3.0);
                float diagonal = smoothstep(0.48, 0.92, i.uv.x + i.uv.y);
                float sheen = diagonal * _Highlight * 0.12;

                float3 rgb = tex.rgb * diffuse;
                rgb += _EdgeGlow.rgb * fresnel * _EdgeGlow.a;
                rgb += sheen;
                return fixed4(rgb, tex.a);
            }
            ENDCG
        }
    }
}
