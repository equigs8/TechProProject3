Shader "Custom/SimpleWaterURP"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (0, 0.5, 0.8, 0.5)
        _ScrollSpeed ("Scroll Speed (X, Y)", Vector) = (0.1, 0.1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _ScrollSpeed;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                // Animate UVs based on time
                output.uv = input.uv + (_Time.y * _ScrollSpeed.xy);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * _BaseColor;
                return color;
            }
            ENDHLSL
        }
    }
}