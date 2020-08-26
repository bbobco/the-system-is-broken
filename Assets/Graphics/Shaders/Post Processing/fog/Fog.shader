Shader "Custom/Fog"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
        _FogColor ("-", Color) = (0, 0, 0, 0)
        _SkyTint ("-", Color) = (.5, .5, .5, .5)
        [Gamma] _SkyExposure ("-", Range(0, 8)) = 1.0
        [NoScaleOffset] _SkyCubemap ("-", CUBE) = "" {}
    }
    CGINCLUDE

    #include "UnityCG.cginc"
    #pragma multi_compile _ RADIAL_DIST

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    sampler2D_float _CameraDepthTexture;

    float _DistanceOffset;
    float _LinearGrad;
    float _LinearOffs;

    // Fog/skybox information
    half4 _FogColor;
    samplerCUBE _SkyCubemap;
    half4 _SkyCubemap_HDR;
    half4 _SkyTint;
    half _SkyExposure;
    float _SkyRotation;

    struct v2f
    {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
        float2 uv_depth : TEXCOORD1;
        float3 ray : TEXCOORD2;
    };

    float3 RotateAroundYAxis(float3 v, float deg)
    {
        float alpha = deg * UNITY_PI / 180.0;
        float sina, cosa;
        sincos(alpha, sina, cosa);
        float2x2 m = float2x2(cosa, -sina, sina, cosa);
        return float3(mul(m, v.xz), v.y).xzy;
    }

    v2f vert(appdata_full v)
    {
        v2f o;

        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        o.uv_depth = v.texcoord.xy;
        o.ray = RotateAroundYAxis(v.texcoord1.xyz, -_SkyRotation);

    #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0.0) o.uv.y = 1.0 - o.uv.y;
    #endif

        return o;
    }

    half ComputeFogFactor(float coord)
    {
        float fog = 0.0;
        // factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
        fog = coord * _LinearGrad + _LinearOffs;
        return saturate(fog);
    }

    // Distance-based fog
    float ComputeDistance(float3 ray, float depth)
    {
        float dist;
    #if RADIAL_DIST
        dist = length(ray * depth);
    #else
        dist = depth * _ProjectionParams.z;
    #endif
        // Built-in fog starts at near plane, so match that by
        // subtracting the near value. 
        dist -= _ProjectionParams.y;
        return dist;
    }

    half4 frag(v2f i) : SV_Target
    {
        half4 sceneColor = tex2D(_MainTex, i.uv);

        // Reconstruct world space position & direction towards this screen pixel.
        float zsample = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
        float depth = Linear01Depth(zsample * (zsample < 1.0));

        // Compute fog amount.
        float g = ComputeDistance(i.ray, depth) - _DistanceOffset;
        half fog = ComputeFogFactor(max(0.0, g));

        // Look up the skybox color.
        // half3 skyColor = DecodeHDR(texCUBE(_SkyCubemap, i.ray), _SkyCubemap_HDR);
        half3 skyColor = texCUBE(_SkyCubemap, i.ray);
        skyColor *= _SkyTint.rgb * _SkyExposure * unity_ColorSpaceDouble;
        // Lerp between source color to skybox color with fog amount.
        return lerp(half4(skyColor, 1), sceneColor, fog/2);
        return half4(skyColor,0.5);
    }

    ENDCG
    SubShader
    {
        ZTest Always Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback off
}
