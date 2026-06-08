Shader "BoredomAndDungeons/TargetSilhouetteOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0.08, 0.055, 0.9)
        _OutlinePixels ("Outline Width Pixels", Float) = 2.4
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent+40"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "SilhouetteOutline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlinePixels;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                float4 positionCS = TransformObjectToHClip(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float3 normalVS = TransformWorldToViewDir(normalWS, true);
                float2 direction = normalVS.xy;
                float lengthValue = max(length(direction), 0.0001);
                direction /= lengthValue;
                float2 pixelOffset =
                    direction * _OutlinePixels * 2.0 / _ScreenParams.xy;
                positionCS.xy += pixelOffset * positionCS.w;
                output.positionCS = positionCS;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }

    SubShader
    {
        Tags { "Queue"="Transparent+40" "RenderType"="Transparent" }
        Pass
        {
            Cull Front
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlinePixels;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float4 pos = UnityObjectToClipPos(v.vertex);
                float3 normalWS = UnityObjectToWorldNormal(v.normal);
                float3 normalVS = mul((float3x3)UNITY_MATRIX_V, normalWS);
                float2 direction = normalVS.xy;
                direction /= max(length(direction), 0.0001);
                float2 pixelOffset = direction * _OutlinePixels * 2.0 / _ScreenParams.xy;
                pos.xy += pixelOffset * pos.w;
                o.pos = pos;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
