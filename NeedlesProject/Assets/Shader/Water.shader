Shader "Nature/Water"
{
    Properties
    {
        _BaseColor     ("Base Color",      Color)             = (1.0, 1.0, 1.0, 1.0)
        _HighLightColor("HighLight Color", Color)             = (1.0, 1.0, 1.0, 1.0)
        _NWidth        ("Noise Width",     Float)             = 1.0
        _NHeight       ("Noise Height",    Float)             = 1.0
        _NOffset       ("Noise Offset",    Float)             = 0
        _CCenter       ("Clip Center",     Range(0.0, 1.0))   = 0.5
        _CRangeMin     ("Clip Range Min",  Range(0.0, 0.5))   = 0.02
        _CRangeMax     ("Clip Range Max",  Range(0.0, 0.5))   = 0.02
        _BlendRate     ("Blend Rate",      Range(0.0, 1.0))   = 1.0
        _WRoop         ("Wave Roop",       Float)             = 1.0
        _WSpeed        ("Wave Speed",      Float)             = 1.0
        _WaveOffset    ("Wave Offset",     Range(0.0, 360.0)) = 0
        _WWidth        ("Wave Width",      Float)             = 1.0
        _WHeight       ("Wave Height",     Float)             = 1.0
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent+100" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float3 co)
            {
                return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 56.787))) * 43758.5453);
            }

            float noise(float3 pos)
            {
                float3 ip = floor(pos);
                float3 fp = smoothstep(0, 1, frac(pos));
                float4 a = float4(
                    rand(ip + float3(0, 0, 0)),
                    rand(ip + float3(1, 0, 0)),
                    rand(ip + float3(0, 1, 0)),
                    rand(ip + float3(1, 1, 0)));
                float4 b = float4(
                    rand(ip + float3(0, 0, 1)),
                    rand(ip + float3(1, 0, 1)),
                    rand(ip + float3(0, 1, 1)),
                    rand(ip + float3(1, 1, 1)));

                a = lerp(a, b, fp.z);
                a.xy = lerp(a.xy, a.zw, fp.y);
                return lerp(a.x, a.y, fp.x);
            }

            float perlin(float3 pos)
            {
                return (noise(pos  ) * 2 +
                        noise(pos*2))
                        / 3;
            }

            float  _NWidth;
            float  _NHeight;
            float  _NOffset;
            float  _CCenter;
            float  _CRangeMin;
            float  _CRangeMax;
            float4 _BaseColor;
            float4 _HighLightColor;
            float  _BlendRate;
            float  _WRoop;
            float  _WSpeed;
            float  _WaveOffset;
            float  _WWidth;
            float  _WHeight;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.uv;


                float time = _Time.x * _WSpeed;

                pos.x = pos.x * _NWidth  + _NOffset + sin(radians(pos.y * 360 * _WRoop               + time)) * _WWidth;
                pos.y = pos.y * _NHeight + _NOffset + sin(radians(pos.x * 360 * _WRoop + _WaveOffset + time)) * _WHeight;

                float noise = perlin(float3(pos, 0.0));

                if (_CCenter - _CRangeMin < noise && noise < _CCenter + _CRangeMax)
                {
                    return lerp(_BaseColor, _HighLightColor, _BlendRate);
                }

                return _BaseColor;
            }
            ENDCG
        }
    }
}
