Shader "BoredomAndDungeons/ModernHandheldDecal"
{
    Properties
    {
        _MainTex ("Decal", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _GlossLift ("Gloss Lift", Range(0,0.4)) = 0.16
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent+10"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        LOD 100
        Cull Off
        ZWrite Off
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
            float _GlossLift;

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
                clip(tex.a - 0.005);

                float3 n = normalize(i.worldNormal);
                float3 v = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float3 keyLight = normalize(float3(0.48, 0.72, -0.52));
                float diffuse = 0.78 + 0.22 * saturate(dot(n, keyLight));
                float facing = pow(saturate(abs(dot(n, v))), 10.0);
                tex.rgb = tex.rgb * diffuse + facing * _GlossLift;
                return tex;
            }
            ENDCG
        }
    }
}
