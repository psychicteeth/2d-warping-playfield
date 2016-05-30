Shader "Unlit/FlatLit"
{
	Properties
	{
	}
	SubShader
	{
		Tags { 
		"RenderType"="Opaque" 
		"LightMode"="Vertex"
		}
            
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 uv : TEXCOORD0;
				float4 col: COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 col : COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 refPos = float4(v.uv,0);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex + refPos);
				o.col = v.col;
				o.col.a = 1;
				float3 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
				// get vertex normal in world space
                //half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                //half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                //o.col = nl * _LightColor0;
				float shiny = v.col.a;
                for (int i = 0; i < 8; i++)
                {                	
               		half3 toLight = unity_LightPosition[i].xyz - viewPos;
                	half lightRange = sqrt(unity_LightAtten[i].w);
                	half atten = smoothstep(lightRange,0,length(toLight));

					fixed3 lightDirObj = mul( (float3x3)UNITY_MATRIX_T_MV, toLight);	//View => model
 
					lightDirObj = normalize(lightDirObj);
					fixed d = (dot(v.normal, lightDirObj) + 1.0f) / 2.0f;
					fixed diff = d / (d + (1.f + v.col.a * 10.0f));//pow(( d , (v.col.a + 0.2f) * 10.0f);

                	o.col.rgb += unity_LightColor[i].rgb * atten * diff;

                }
                o.col = saturate(o.col);
                return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.col;
				return col;
			}
			ENDCG
		}
	}
}
