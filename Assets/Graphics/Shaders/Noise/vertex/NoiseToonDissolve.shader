Shader "Custom/NoiseToonDissolve"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "Transparent" {}
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		_SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
		_SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0

		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
		_BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}
		_BurnColor("Burn Color", Color) = (1,1,1,1)

		_EmissionAmount("Emission amount", float) = 2.0
	}
	
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull front
			LOD 100
			Pass
			{
				Tags
				{
					"LightMode" = "ForwardBase"
					"PassFlags" = "OnlyDirectional"
					"RenderType" = "Opaque"
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase

				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				#include "ClassicNoise3D.cginc"


				struct appdata
				{
					float4 vertex : POSITION;
					float4 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 worldNormal : NORMAL;
					float3 viewDir : TEXCOORD1;
					SHADOW_COORDS(2)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				float random(float2 p)
				{
					float2 K1 = float2(23.14069263277926, 2.665144142690225);
					return frac(cos(dot(p, K1)) * 12345.6789);
				}

				v2f vert(appdata v, float3 normal : NORMAL)
				{
					v2f o;

					float offs = 3.0 * _Time.y;
					float3 crd = v.vertex.xyz;
					float multiplier = 0.2;
					float noiseSeed = random(crd.xy);

					float3 normaloffset = float3(cnoise(noiseSeed + offs)*normal.x*multiplier, cnoise(noiseSeed + offs)*normal.y*multiplier, cnoise(noiseSeed + offs)*normal.z*multiplier);

					o.pos = UnityObjectToClipPos(v.vertex + float4(normaloffset, 0));
					// o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.viewDir = WorldSpaceViewDir(v.vertex);

					TRANSFER_SHADOW(o)
					return o;
				}

				float4 _Color;
				float4 _AmbientColor;
				float _Glossiness;
				float4 _SpecularColor;
				float4 _RimColor;
				float _RimAmount;
				float _RimThreshold;

				float4 frag(v2f i) : SV_Target
				{
		/*			float3 normal = normalize(i.worldNormal);
					float NdotL = dot(_WorldSpaceLightPos0, normal);

					float shadow = SHADOW_ATTENUATION(i);
					float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
					float4 light = lightIntensity * _LightColor0;
					float3 viewDir = normalize(i.viewDir);

					float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
					float NdotH = dot(normal, halfVector);

					float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

					float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
					float4 specular = specularIntensitySmooth * _SpecularColor;

					float4 rimDot = 1 - dot(viewDir, normal);

					float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
					rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
					float4 rim = rimIntensity * _RimColor;

					float4 sample = tex2D(_MainTex, i.uv);

					return _Color * sample * (_AmbientColor + light + specular + rim);*/

					fixed4 col = tex2D(_MainTex, i.uv) * _Color; // multiply by _Color
					return col;
					

				}
				ENDCG
			}
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

			CGPROGRAM


				#pragma surface surf Lambert addshadow

				fixed4 _Color;
				sampler2D _MainTex;
				sampler2D _SliceGuide;
				sampler2D _BumpMap;
				sampler2D _BurnRamp;
				fixed4 _BurnColor;
				float _BurnSize;
				float _SliceAmount;
				float _EmissionAmount;

				struct Input {
					float2 uv_MainTex;
				};

				void surf(Input IN, inout SurfaceOutput o) {
				_SliceAmount += _Time.y * 0.1f;
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - _SliceAmount;
				clip(test);

				if (test < _BurnSize && _SliceAmount > 0) {
					o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0)) * _BurnColor * _EmissionAmount;
				}

				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
		}
}