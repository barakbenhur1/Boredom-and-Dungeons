Shader "Hidden/BoredomAndDungeons/BBHSmoothDrip"
{
    Properties
    {
        _MainTex ("Captured BBH Frame", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _EdgeSoftness ("Edge Softness", Range(0.0005, 0.05)) = 0.008
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay+100"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;
            float _EdgeSoftness;

            float Hash11(float value)
            {
                return frac(sin(value * 127.1) * 43758.5453123);
            }

            float SmoothBell(float x, float center, float halfWidth)
            {
                float distance01 = abs(x - center) / max(halfWidth, 0.0001);
                float inside = saturate(1.0 - distance01);
                return inside * inside * (3.0 - 2.0 * inside);
            }

            fixed4 frag(v2f_img input) : SV_Target
            {
                float2 uv = input.uv;
                float progress = saturate(_Progress);
                float eased = progress * progress * (3.0 - 2.0 * progress);

                float broadWave =
                    sin(uv.x * 6.2831853 * 1.35 + progress * 1.8) * 0.020 +
                    sin(uv.x * 6.2831853 * 2.70 - progress * 1.1) * 0.010;

                float lobeField = 0.0;
                lobeField += SmoothBell(uv.x, 0.09, 0.13) * 0.075;
                lobeField += SmoothBell(uv.x, 0.25, 0.17) * 0.145;
                lobeField += SmoothBell(uv.x, 0.43, 0.15) * 0.095;
                lobeField += SmoothBell(uv.x, 0.58, 0.19) * 0.175;
                lobeField += SmoothBell(uv.x, 0.76, 0.15) * 0.110;
                lobeField += SmoothBell(uv.x, 0.92, 0.12) * 0.155;

                float cells = uv.x * 8.0;
                float cellIndex = floor(cells);
                float cellLocal = frac(cells);
                float randomCenter = lerp(0.34, 0.66, Hash11(cellIndex + 19.0));
                float randomWidth = lerp(0.22, 0.38, Hash11(cellIndex + 41.0));
                float randomDepth = lerp(0.025, 0.080, Hash11(cellIndex + 73.0));
                float smallLobe = SmoothBell(cellLocal, randomCenter, randomWidth) * randomDepth;

                float viscosity = sin(progress * 3.14159265);
                float movingFront = lerp(1.16, -0.20, eased);
                float edge = movingFront -
                    (lobeField * lerp(0.35, 1.0, viscosity)) -
                    (smallLobe * viscosity) +
                    broadWave * viscosity;

                float softness = max(_EdgeSoftness, fwidth(uv.y) * 1.45);
                float mask = 1.0 - smoothstep(edge - softness, edge + softness, uv.y);

                float edgeDistance = abs(uv.y - edge);
                float edgeBand = 1.0 - smoothstep(softness, softness * 4.0, edgeDistance);
                float2 sampleUv = uv;
                sampleUv.x += sin(uv.y * 18.0 + uv.x * 9.0 + progress * 3.0) *
                    edgeBand * 0.0028;
                sampleUv.y += sin(uv.x * 24.0 - progress * 4.0) *
                    edgeBand * 0.0018;

                fixed4 color = tex2D(_MainTex, sampleUv);
                color.rgb += edgeBand * float3(0.030, 0.045, 0.070);
                color.a *= mask;
                return color;
            }
            ENDCG
        }
    }
    Fallback Off
}
