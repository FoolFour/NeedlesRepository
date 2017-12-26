Shader "Unlit/BlockTilingShader"
{
	Properties
	{
		_MainTex ("Texture",     2D) = "white" {}
		_RuleTex ("RuleTexture", 2D) = "white" {}
		_Amount  ("Amount",      Range(0, 1)) = 0
		_Range   ("Range",       Range(0, 1)) = 0
		_Hardness("Hardness",    Float) = 1
	}

	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;\
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _RuleTex;
			float4 _MainTex_ST;

			float _Amount;
			float _Range;
			float _Hardness;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed4 rule_col = tex2D(_RuleTex, i.uv);
				float len = rule_col.r;
				if(len > _Amount)
				{
					float res = max(_Amount + _Range - len, 0) * _Hardness;
					return fixed4(col.rgb, res);
				}

				// sample the texture
				
				return col;
			}
			ENDCG
		}
	}
}
