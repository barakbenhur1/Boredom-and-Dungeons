Shader "Hidden/BoredomAndDungeons/HandheldScreenPowerReveal"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Progress ("Reveal Progress", Range(0, 1)) = 0
        _EdgeWidth ("Reveal Edge Width", Range(0.001, 0.08)) = 0.018
        _GlowStrength ("Reveal Glow", Range(0, 2)) = 1
        _GlowColor ("Reveal Glow Color", Color) = (0.40, 0.88, 1.00, 1.00)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent+500"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float4 _ClipRect;
            float _Progress;
            float _EdgeWidth;
            float _GlowStrength;
            float4 _GlowColor;

            v2f vert(appdata input)
            {
                v2f output;
                output.worldPosition = input.vertex;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 sprite = tex2D(_MainTex, input.uv);
                float progress = saturate(_Progress);
                float frontY = lerp(1.035, -0.035, progress);
                float feather = max(_EdgeWidth, fwidth(input.uv.y) * 1.75);
                float blackAlpha = 1.0 - smoothstep(
                    frontY - feather,
                    frontY + feather,
                    input.uv.y
                );
                float edgeDistance = abs(input.uv.y - frontY);
                float glow = 1.0 - smoothstep(
                    feather * 0.18,
                    feather * 3.4,
                    edgeDistance
                );
                glow *= sin(progress * UNITY_PI) * _GlowStrength;
                fixed4 result = fixed4(
                    _GlowColor.rgb * glow * input.color.rgb,
                    max(blackAlpha, glow * _GlowColor.a * 0.88) *
                        sprite.a * input.color.a
                );
#ifdef UNITY_UI_CLIP_RECT
                result.a *= UnityGet2DClipping(
                    input.worldPosition.xy,
                    _ClipRect
                );
#endif
#ifdef UNITY_UI_ALPHACLIP
                clip(result.a - 0.001);
#endif
                return result;
            }
            ENDCG
        }
    }
    Fallback Off
}
