using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlayerAttackRange : MonoBehaviour
{
    [Header("���� ����")]
    public float radius = 1f;
    [Range(0f, 360f)] public float angle = 180f;
    public int segments = 30;

    [Header("��Ƽ���� ����")]
    public Color rangeColor = new Color(0f, 1f, 0f, 0.3f); // ���λ� ������

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // ��Ƽ���� ����
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = rangeColor;
        meshRenderer.material = mat;

        GenerateFanMesh();
    }

    void OnValidate()
    {
        // �����Ϳ��� �� �ٲ� �� �ٷ� �ݿ�
        if (Application.isPlaying && meshFilter != null)
        {
            GenerateFanMesh();
            meshRenderer.material.color = rangeColor;
        }
    }

    public void GenerateFanMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float angleStep = angle / segments;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle / 2f + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;
            vertices[i + 1] = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * radius;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
