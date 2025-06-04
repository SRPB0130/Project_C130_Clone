using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanAttackArea : MonoBehaviour
{
    public float radius = 3f;
    public float angle = 90f;
    public int segmentCount = 30;

    private MeshRenderer meshRenderer;
    private Material mat;

    void Start()
    {
        GenerateMesh();
        ApplyMaterial(new Color(1f, 0.5f, 0f, 0.3f)); // 기본 주황색
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segmentCount + 2];
        int[] triangles = new int[segmentCount * 3];

        vertices[0] = Vector3.zero;

        float angleStep = angle / segmentCount;
        for (int i = 0; i <= segmentCount; i++)
        {
            float currentAngle = -angle / 2 + angleStep * i;
            float rad = Mathf.Deg2Rad * currentAngle;
            vertices[i + 1] = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * radius;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void ApplyMaterial(Color color)
    {
        mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = mat;
    }

    // 외부에서 색상 변경 가능하도록
    public void SetColor(Color color)
    {
        if (mat != null)
        {
            mat.color = color;
        }
    }
}
