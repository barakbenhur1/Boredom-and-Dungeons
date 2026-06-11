Shader "Hidden/BoredomAndDungeons/CinematicDepthOfField"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _BlurTex ("Blurred", 2D) = "black" {}
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragBlur
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _BlurDirection;

            fixed4 fragBlur(v2f_img input) : SV_Target
            {
                float2 uv = input.uv;
                fixed4 color = tex2D(_MainTex, uv) * 0.227027;
                color += tex2D(
                    _MainTex,
                    uv + _BlurDirection.xy * 1.384615
                ) * 0.316216;
                color += tex2D(
                    _MainTex,
                    uv - _BlurDirection.xy * 1.384615
                ) * 0.316216;
                color += tex2D(
                    _MainTex,
                    uv + _BlurDirection.xy * 3.230769
                ) * 0.070270;
                color += tex2D(
                    _MainTex,
                    uv - _BlurDirection.xy * 3.230769
                ) * 0.070270;
                return color;
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragComposite
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BlurTex;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            float _FocusDistance;
            float _NearFocusRange;
            float _FarFocusRange;
            float _BlurStrength;

            fixed4 fragComposite(v2f_img input) : SV_Target
            {
                fixed4 sharp = tex2D(_MainTex, input.uv);
                fixed4 blurred = tex2D(_BlurTex, input.uv);

                float rawDepth = SAMPLE_DEPTH_TEXTURE(
                    _CameraDepthTexture,
                    input.uv
                );
                float eyeDepth = LinearEyeDepth(rawDepth);

                float nearCoc = saturate(
                    (_FocusDistance - eyeDepth) /
                    max(_NearFocusRange, 0.0001)
                );
                float farCoc = saturate(
                    (eyeDepth - _FocusDistance) /
                    max(_FarFocusRange, 0.0001)
                );
                float coc = smoothstep(
                    0.08,
                    1.0,
                    max(nearCoc, farCoc)
                ) * _BlurStrength;

                return lerp(sharp, blurred, coc);
            }
            ENDCG
        }
    }

    Fallback Off
}
