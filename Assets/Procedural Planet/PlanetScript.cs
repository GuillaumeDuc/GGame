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
    [SerializeField]
    PlanetType material;
    [SerializeField]
    Material[] materials;
    [SerializeField]
    Light planetLight;
    Mesh mesh;

    public void SetPlanet(PlanetType planetType, IPlanetMatOptions planetOptions, float radius, int resolution = 10)
    {
        transform.localScale *= radius;
        material = planetType;
        this.resolution = resolution;
        Material planetMat = planetOptions.GetMat(materials[(int)material]);
        GetComponent<MeshRenderer>().material = planetMat;
    }

    public void SetLight(long intensity, long range, Color color)
    {
        planetLight.gameObject.SetActive(true);
        planetLight.color = color;
        planetLight.intensity = intensity;
        planetLight.range = range;
    }

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
        GenerateMesh();
    }

    void OnValidate()
    {
        GetComponent<MeshRenderer>().material = materials[(int)material];
    }

    void Update() { }

    void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        g = new GeoOctasphere()
        {
            Resolution = resolution,
            Radius = radius
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
}
