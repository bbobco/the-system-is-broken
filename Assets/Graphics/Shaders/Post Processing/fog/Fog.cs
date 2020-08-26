using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Fog : MonoBehaviour
{
    public Color fogColor;
    public float startDistance;
    public float endDistance;
    public bool useRadialDistance;
    [SerializeField] Shader _shader;

    Material _material;

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        startDistance = Mathf.Max(startDistance, 0.0f);
        _material.SetFloat("_DistanceOffset", startDistance);


        endDistance = Mathf.Max(endDistance, 0.0f);
        float end = endDistance;
        float invDiff = 1.0f / Mathf.Max(end - startDistance, 1E-6f);
        _material.SetFloat("_LinearGrad", -invDiff);
        _material.SetFloat("_LinearOffs", end * invDiff);
        

        if (useRadialDistance)
            _material.EnableKeyword("RADIAL_DIST");
        else
            _material.DisableKeyword("RADIAL_DIST");

        // i dont think this is rite
        Material skybox = GetComponent<Skybox>().material;
        _material.SetTexture("_SkyCubemap", skybox.GetTexture("_MainTex"));
        _material.SetColor("_SkyTint", fogColor);
        _material.SetFloat("_SkyExposure", skybox.GetFloat("_Exposure"));
        _material.SetFloat("_SkyRotation", skybox.GetFloat("_Rotation"));

        // get vectors towards frustum corners.
        Camera cam = GetComponent<Camera>();
        Transform camtr = cam.transform;
        float camNear = cam.nearClipPlane;
        float camFar = cam.farClipPlane;

        float tanHalfFov = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2);
        Vector3 toRight = camtr.right * camNear * tanHalfFov * cam.aspect;
        Vector3 toTop = camtr.up * camNear * tanHalfFov;

        Vector3 v_tl = camtr.forward * camNear - toRight + toTop;
        Vector3 v_tr = camtr.forward * camNear + toRight + toTop;
        Vector3 v_br = camtr.forward * camNear + toRight - toTop;
        Vector3 v_bl = camtr.forward * camNear - toRight - toTop;

        float v_s = v_tl.magnitude * camFar / camNear;

        // Draw screen quad.
        RenderTexture.active = destination;

        _material.SetTexture("_MainTex", source);
        _material.SetPass(0);

        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0, 0);
        GL.MultiTexCoord(1, v_bl.normalized * v_s);
        GL.Vertex3(0, 0, 0.1f);

        GL.MultiTexCoord2(0, 1, 0);
        GL.MultiTexCoord(1, v_br.normalized * v_s);
        GL.Vertex3(1, 0, 0.1f);

        GL.MultiTexCoord2(0, 1, 1);
        GL.MultiTexCoord(1, v_tr.normalized * v_s);
        GL.Vertex3(1, 1, 0.1f);

        GL.MultiTexCoord2(0, 0, 1);
        GL.MultiTexCoord(1, v_tl.normalized * v_s);
        GL.Vertex3(0, 1, 0.1f);

        GL.End();
        GL.PopMatrix();
    }

}
