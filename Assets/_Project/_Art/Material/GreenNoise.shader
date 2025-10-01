Shader "Custom/GreenNoiseGridWorld"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.8, 0.0, 1.0)
        _NoiseScale ("Noise Scale", Float) = 5.0
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.2
        _StripeSize ("Stripe Spacing", Float) = 1.0
        _DarkFactor ("Stripe Dark Factor", Range(0,1)) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            fixed4 _BaseColor;
            float _NoiseScale;
            float _NoiseStrength;
            float _StripeSize;
            float _DarkFactor;

            // Simple hash noise
            float rand(float2 n)
            {
                return frac(sin(dot(n, float2(12.9898,78.233))) * 43758.5453);
            }

            float noise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // --- Base green with noise ---
                float n = noise(i.uv * _NoiseScale);
                float variation = (n - 0.5) * 2.0 * _NoiseStrength;
                fixed4 col = _BaseColor + fixed4(0, variation, 0, 0);

                // --- Grid stripes based on world position ---
                float verticalStripe   = fmod(floor(i.worldPos.x / _StripeSize), 2.0);
                float horizontalStripe = fmod(floor(i.worldPos.z / _StripeSize), 2.0);

                // Allow overlap to darken twice
                float combined = verticalStripe + horizontalStripe; // 0, 1, or 2
                float darkFactor = 1.0 - combined * _DarkFactor;
                darkFactor = max(darkFactor, 0.0);

                return col * darkFactor;
            }
            ENDCG
        }
    }
}
