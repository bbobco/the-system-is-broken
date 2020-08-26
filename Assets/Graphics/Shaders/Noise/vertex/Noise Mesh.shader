// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// incorrect number of arguments to numeric-type constructor
//Compiling Vertex program
//Platform defines : UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR

Shader "Custom/Noise Mesh"
{
    Properties
    {
    }
    CGINCLUDE

#include "UnityCG.cginc"
#include "ClassicNoise3D.cginc"

struct v2f
{
	half3 worldNormal : TEXCOORD0;
    float4 pos : SV_POSITION;
};

v2f vert(appdata_base v, float3 normal : NORMAL)
{
    v2f o;
    float3 offs = float3(0, 0, 0.75) * _Time.y;
    float3 crd = v.vertex.xyz;

    float n1 =
        cnoise(crd * 1 + offs) +
        cnoise(crd * 2 + offs) * 0.25 +
        cnoise(crd * 4 + offs) * 0.125;

    offs += float3(0, 0, 5.3);

    float n2 =
        cnoise(crd * 1 + offs) +
        cnoise(crd * 2 + offs) * 0.25 +
        cnoise(crd * 4 + offs) * 0.125;

    offs += float3(0, 0, 5.3);

    float n3 =
        cnoise(crd * 1 + offs) +
        cnoise(crd * 2 + offs) * 0.25 +
        cnoise(crd * 4 + offs) * 0.125;

    o.pos = UnityObjectToClipPos(v.vertex + float4(n1 * normal.x, n2 * normal.y, n3 * normal.z, 0));
	o.worldNormal = UnityObjectToWorldNormal(normal);
    return o;
}

//float4 frag(v2f i) : SV_Target 
//{
//    return float4(1, 1, 1, 1);
//}

fixed4 frag(v2f i) : SV_Target
{
	fixed4 c = 0;
	// normal is a 3D vector with xyz components; in -1..1
	// range. To display it as color, bring the range into 0..1
	// and put into red, green, blue components
	c.rgb = i.worldNormal*0.5 + 0.5;
	return c;
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
