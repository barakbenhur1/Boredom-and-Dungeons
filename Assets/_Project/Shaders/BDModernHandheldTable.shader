Shader "BoredomAndDungeons/ModernHandheldTable"
{
    Properties
    {
        _MainTex ("Sharp Wood", 2D) = "white" {}
        _BlurTex ("Defocused Wood", 2D) = "gray" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FocusCenter ("Focus Center", Range(0,1)) = 0.5
        _FocusHalfWidth ("Focus Half Width", Range(0.02,0.5)) = 0.17
        _FocusFalloff ("Focus Falloff", Range(0.02,0.5)) = 0.34
        _Vignette ("Vignette", Range(0,0.8)) = 0.26
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100
        Cull Off
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BlurTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _FocusCenter;
            float _FocusHalfWidth;
            float _FocusFalloff;
            float _Vignette;

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
                fixed4 sharp = tex2D(_MainTex, i.uv);
                fixed4 blurred = tex2D(_BlurTex, i.uv);

                // The handheld is the focal subject. The table stays relatively
                // crisp around its contact band and becomes progressively
                // defocused toward the near/far edges of the frame.
                float depthDistance = abs(i.uv.y - _FocusCenter);
                float blurAmount = smoothstep(
                    _FocusHalfWidth,
                    _FocusHalfWidth + _FocusFalloff,
                    depthDistance
                );

                fixed3 rgb = lerp(sharp.rgb, blurred.rgb, blurAmount);
                float2 centered = i.uv - 0.5;
                float vignette = saturate(dot(centered, centered) * 2.2);
                rgb *= 1.0 - vignette * _Vignette;
                return fixed4(rgb * _Color.rgb, 1.0);
            }
            ENDCG
        }
    }
}
