// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// incorrect number of arguments to numeric-type constructor
//Compiling Vertex program
//Platform defines : UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR

Shader "Custom/Noise Normal"
{
	Properties
	{
		_Density("Density", Range(2,10000)) = 5000
	}
    CGINCLUDE

#include "UnityCG.cginc"
#include "ClassicNoise3D.cginc"

struct v2f
{
	half3 worldNormal : TEXCOORD1;
	// texture coordinate for the normal map
	float2 uv : TEXCOORD0;

    float4 pos : SV_POSITION;

};

	float _Density;

float random(float2 p)
{
	float2 K1 = float2(23.14069263277926, 2.665144142690225);
	return frac(cos(dot(p, K1)) * 12345.6789);
}


v2f vert(appdata_base v, float3 normal : NORMAL, float2 uv : TEXCOORD1)
{
    v2f o;
    float offs = 3.0 * _Time.y;
    float3 crd = v.vertex.xyz;
	float multiplier = 0.2;
	
	float noiseSeed = random(crd.xy);

	float3 normaloffset = float3(cnoise(noiseSeed + offs)*normal.x*multiplier, cnoise(noiseSeed + offs)*normal.y*multiplier, cnoise(noiseSeed + offs)*normal.z*multiplier);


    o.pos = UnityObjectToClipPos(v.vertex + float4(normaloffset, 0));

	o.uv = uv * _Density;

    return o;
}


//float4 frag(v2f i) : SV_Target 
//{
//    return float4(1, 1, 1, 1);
//}

// normal map texture from shader properties
sampler2D _BumpMap;

fixed4 frag(v2f i) : SV_Target
{
	float2 c = i.uv;
	c = floor(c) / 2;
	float checker = frac(c.x + c.y) * 2;
	return checker;
}

    ENDCG
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma glsl
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

}
