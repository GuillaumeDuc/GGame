using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Rendering;

public class PlanetScript : MonoBehaviour
{
    [System.Flags]
    public enum MeshOptimizationMode
    {
        Nothing = 0, ReorderIndices = 1, ReorderVertices = 0b10
    }
    [SerializeField]
    MeshOptimizationMode meshOptimization;
    [SerializeField, Range(1, 50)]
    int resolution = 1;

    [SerializeField, Range(1, 50)]
    int radius = 1;
    public enum MaterialMode { DefaultPlanetMat, GasPlanetMat, SunPlanetMat }
    [SerializeField]
    MaterialMode material;
    [SerializeField]
    Material[] materials;
    Mesh mesh;
    [System.Flags]
    public enum GizmoMode
    {
        Nothing = 0, Vertices = 1, Normals = 0b10, Tangents = 0b100, Triangles = 0b1000
    }
    [SerializeField]
    GizmoMode gizmos;
    [SerializeField]
    Vector3 noiseOffset;
    [System.NonSerialized]
    Vector3[] vertices, normals;
    [System.NonSerialized]
    Vector4[] tangents;
    [System.NonSerialized]
    int[] triangles;

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob : IJobFor
    {
        public GeoOctasphere g;
        public void Execute(int i)
        {
            g.Execute(i);
        }
    }

    GeoOctasphere g;

    void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void OnValidate() => enabled = true;

    void Update()
    {
        GenerateMesh();
        enabled = false;

        GetComponent<MeshRenderer>().material = materials[(int)material];
    }

    void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        g = new GeoOctasphere()
        {
            Resolution = resolution,
            Radius = radius,
            NoiseOffset = noiseOffset
        };
        g.Setup(meshData);
        new MeshJob
        {
            g = g,
        }.ScheduleParallel(g.JobLength, resolution, default).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        if (meshOptimization == MeshOptimizationMode.ReorderIndices)
        {
            mesh.OptimizeIndexBuffers();
        }
        else if (meshOptimization == MeshOptimizationMode.ReorderVertices)
        {
            mesh.OptimizeReorderVertexBuffer();
        }
        else if (meshOptimization != MeshOptimizationMode.Nothing)
        {
            mesh.Optimize();
        }
    }

    void OnDrawGizmos()
    {
        if (gizmos == GizmoMode.Nothing || mesh == null)
        {
            return;
        }

        bool drawVertices = (gizmos & GizmoMode.Vertices) != 0;
        bool drawNormals = (gizmos & GizmoMode.Normals) != 0;
        bool drawTangents = (gizmos & GizmoMode.Tangents) != 0;
        bool drawTriangles = (gizmos & GizmoMode.Triangles) != 0;

        if (vertices == null)
        {
            vertices = mesh.vertices;
        }
        if (drawNormals && normals == null)
        {
            drawNormals = mesh.HasVertexAttribute(VertexAttribute.Normal);
            if (drawNormals)
            {
                normals = mesh.normals;
            }
        }
        if (drawTangents && tangents == null)
        {
            drawTangents = mesh.HasVertexAttribute(VertexAttribute.Tangent);
            if (drawTangents)
            {
                tangents = mesh.tangents;
            }
        }
        if (drawTriangles && triangles == null)
        {
            triangles = mesh.triangles;
        }

        Transform t = transform;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = t.TransformPoint(vertices[i]);
            if (drawVertices)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(position, 0.02f);
            }
            if (drawNormals)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(position, t.TransformDirection(normals[i]) * 0.2f);
            }
            if (drawTangents)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(position, t.TransformDirection(tangents[i]) * 0.2f);
            }
        }

        if (drawTriangles)
        {
            float colorStep = 1f / (triangles.Length - 3);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                float c = i * colorStep;
                Gizmos.color = new Color(c, 0f, c);
                Gizmos.DrawSphere(
                    t.TransformPoint((
                        vertices[triangles[i]] +
                        vertices[triangles[i + 1]] +
                        vertices[triangles[i + 2]]
                    ) * (1f / 3f)),
                    0.02f
                );
            }
        }
    }
}
