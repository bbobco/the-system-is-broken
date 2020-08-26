using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartPiece : MonoBehaviour
{

    public float percentage;
    public float angleIncrement;
    public float depth;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;       
        meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        meshRenderer.material = Resources.Load("Materials/PieChartRed") as Material;

        Mesh pieMesh = CreateMesh(makeFrontFace());
        meshFilter.mesh = pieMesh;

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.convex = true;
    }

    private Mesh CreateMesh(Vector2[] poly)
    {
        // convert polygon to triangles
        Triangulator triangulator = new Triangulator(poly);
        int[] tris = triangulator.Triangulate();
        Mesh m = new Mesh();
        Vector3[] vertices = new Vector3[poly.Length * 2];

        for (int i = 0; i < poly.Length; i++)
        {
            vertices[i].x = poly[i].x;
            vertices[i].y = poly[i].y;
            vertices[i].z = -depth; // front vertex
            vertices[i + poly.Length].x = poly[i].x;
            vertices[i + poly.Length].y = poly[i].y;
            vertices[i + poly.Length].z = depth;  // back vertex    
        }
        int[] triangles = new int[tris.Length * 2 + poly.Length * 6];
        int count_tris = 0;
        for (int i = 0; i < tris.Length; i += 3)
        {
            triangles[i] = tris[i];
            triangles[i + 1] = tris[i + 1];
            triangles[i + 2] = tris[i + 2];
        } // front vertices
        count_tris += tris.Length;
        for (int i = 0; i < tris.Length; i += 3)
        {
            triangles[count_tris + i] = tris[i + 2] + poly.Length;
            triangles[count_tris + i + 1] = tris[i + 1] + poly.Length;
            triangles[count_tris + i + 2] = tris[i] + poly.Length;
        } // back vertices
        count_tris += tris.Length;
        // Triangles around the perimeter of the object
        for (int i = 0; i < poly.Length; i++)
        {
            int n = (i + 1) % poly.Length;
            triangles[count_tris] = n;
            triangles[count_tris + 1] = i + poly.Length;
            triangles[count_tris + 2] = i;
            triangles[count_tris + 3] = n;
            triangles[count_tris + 4] = n + poly.Length;
            triangles[count_tris + 5] = i + poly.Length;
            count_tris += 6;
        }
        m.vertices = vertices;
        m.triangles = triangles;
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.Optimize();
        return m;
    }

    private Vector2[] makeFrontFace()
    {
        Vector2[] vertices;

        float angle = percentage * 3.6f;
        int vertLength = (int)Mathf.Ceil(angle / angleIncrement) + 1;
     //   Debug.Log(vertLength);
        vertices = new Vector2[vertLength];

        // make verts
        float currentAngle = 0;
        vertices[0] = new Vector2(0f, 0f);
        for (int i = 1; i < vertLength; i++)
        {
            vertices[i] = new Vector2(
                Mathf.Cos(currentAngle * Mathf.PI / 180f),
                Mathf.Sin(currentAngle * Mathf.PI / 180f)
                );
            currentAngle += angleIncrement;
        }

        return vertices;
    }
}
