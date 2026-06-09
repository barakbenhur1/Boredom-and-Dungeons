Shader "BoredomAndDungeons/ModernHandheldSurface"
{
    Properties
    {
        _MainTex ("Micro Surface Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _EmissionColor ("Emission", Color) = (0,0,0,1)
        _RimColor ("Rim", Color) = (0.1,0.3,0.8,1)
        _RimPower ("Rim Power", Range(1,8)) = 4
        _Roughness ("Roughness", Range(0,1)) = 0.5
        _UseObjectGradient ("Use Object Gradient", Range(0,1)) = 0
        _GradientLeft ("Gradient Left", Color) = (0.02,0.12,0.95,1)
        _GradientMid ("Gradient Mid", Color) = (0.34,0.07,0.55,1)
        _GradientRight ("Gradient Right", Color) = (1,0.18,0.03,1)
        _MicroContrast ("Micro Contrast", Range(0,0.3)) = 0.08
        _SpecularStrength ("Specular Strength", Range(0,1)) = 0.22
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 150
        Cull Off
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _EmissionColor;
            fixed4 _RimColor;
            float _RimPower;
            float _Roughness;
            float _UseObjectGradient;
            fixed4 _GradientLeft;
            fixed4 _GradientMid;
            fixed4 _GradientRight;
            float _MicroContrast;
            float _SpecularStrength;

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
                float3 objectPos : TEXCOORD3;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.objectPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 sampled = tex2D(_MainTex, i.uv);
                float luminance = dot(sampled.rgb, float3(0.299, 0.587, 0.114));
                float micro = (luminance - 0.5) * _MicroContrast;

                float gradientT = saturate(i.objectPos.x / 10.0 + 0.5);
                float3 gradient = gradientT < 0.5
                    ? lerp(_GradientLeft.rgb, _GradientMid.rgb, gradientT * 2.0)
                    : lerp(_GradientMid.rgb, _GradientRight.rgb, (gradientT - 0.5) * 2.0);

                float3 baseColor = lerp(sampled.rgb * _Color.rgb, gradient * (1.0 + micro), _UseObjectGradient);

                float3 n = normalize(i.worldNormal);
                float3 v = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                // The approved key light is above and to the device's right.
                float3 keyLight = normalize(float3(0.56, 0.72, -0.40));
                float3 h = normalize(keyLight + v);
                float ndotl = saturate(dot(n, keyLight));
                float diffuse = 0.34 + 0.66 * ndotl;
                float rim = pow(1.0 - saturate(abs(dot(n, v))), max(1.0, _RimPower));
                float specPower = lerp(52.0, 10.0, saturate(_Roughness));
                float specular = pow(saturate(dot(n, h)), specPower) * _SpecularStrength;

                // Side planes darken slightly, making the molded depth readable.
                float frontFacing = saturate(abs(n.z));
                float sideDepth = lerp(0.76, 1.0, frontFacing);
                float3 rgb = baseColor * diffuse * sideDepth;
                rgb += _EmissionColor.rgb;
                rgb += _RimColor.rgb * rim * 0.58;
                rgb += specular;
                return fixed4(rgb, _Color.a * sampled.a);
            }
            ENDCG
        }
    }
}
