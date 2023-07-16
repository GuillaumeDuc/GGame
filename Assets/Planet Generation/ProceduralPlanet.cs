using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPlanet : MonoBehaviour
{
    public int resolution = 10;
    public ShapeSettings shapeSettings = new ShapeSettings();
    public ColorSettings colorSettings = new ColorSettings();
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    [SerializeField, HideInInspector]
    MeshFilter[] meshClouds;

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.transform.position = gameObject.transform.position;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
        GenerateCloudsMesh();
    }

    public void GenerateCloudsMesh()
    {
        if (this != null)
        {
            GenerateClouds.GenerateMesh(meshClouds, gameObject, resolution, colorSettings, shapeSettings, shapeGenerator);
        }

    }

    public void OnShapeSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        Initialize();
        GenerateColors();
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        foreach (TerrainFace face in terrainFaces)
        {
            face.UpdateUV(colorGenerator);
        }
    }

    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            gameObject.transform.Rotate(0, 0.5f * Time.deltaTime, 0);
        }
    }
}
