Shader "BoredomAndDungeons/ModernHandheldGlass"
{
    Properties
    {
        _MainTex ("Reflection Texture", 2D) = "white" {}
        _Color ("Glass Tint", Color) = (0.2,0.5,0.8,0.08)
        _EdgeColor ("Edge Color", Color) = (0.3,0.7,1,0.25)
        _FresnelPower ("Fresnel Power", Range(1,8)) = 4
        _GlintColor ("Upper Right Glint", Color) = (0.75,0.92,1,1)
        _GlintStrength ("Glint Strength", Range(0,1)) = 0.3
        _GlintSpeed ("Glint Speed", Range(0,1)) = 0.2
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent+10"
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
            fixed4 _EdgeColor;
            fixed4 _GlintColor;
            float _FresnelPower;
            float _GlintStrength;
            float _GlintSpeed;

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
                float3 n = normalize(i.worldNormal);
                float3 v = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float fresnel = pow(
                    1.0 - saturate(abs(dot(n, v))),
                    max(1.0, _FresnelPower)
                );
                fixed4 reflection = tex2D(_MainTex, i.uv);

                // A narrow, low-opacity glint stays on the upper-right side,
                // matching the key light without washing over central UI.
                float upperRight = smoothstep(0.66, 0.96, i.uv.x) *
                                   smoothstep(0.54, 0.94, i.uv.y);
                float travel = 0.012 * sin(_Time.y * max(0.01, _GlintSpeed));
                float stripeDistance = abs((i.uv.x + i.uv.y) - (1.63 + travel));
                float stripe = 1.0 - smoothstep(0.018, 0.085, stripeDistance);
                float softPulse = 0.78 + 0.22 * sin(_Time.y * 0.42);
                float glint = upperRight * stripe * softPulse;

                fixed3 rgb = _Color.rgb;
                rgb += reflection.rgb * reflection.a * 0.12;
                rgb += _EdgeColor.rgb * fresnel;
                rgb += _GlintColor.rgb * glint * _GlintStrength;

                float alpha = _Color.a;
                alpha += reflection.a * 0.065;
                alpha += fresnel * _EdgeColor.a;
                alpha += glint * 0.055 * _GlintStrength;
                return fixed4(rgb, saturate(alpha));
            }
            ENDCG
        }
    }
}
