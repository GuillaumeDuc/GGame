using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateClouds
{
    public static void GenerateMesh(MeshFilter[] meshClouds, GameObject parent, int resolution, ColorSettings colorSettings, ShapeSettings shapeSettings, ShapeGenerator shapeGenerator)
    {
        if (meshClouds == null || meshClouds.Length == 0)
        {
            meshClouds = new MeshFilter[6];
        }
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };


        for (int i = 0; i < 6; i++)
        {
            if (meshClouds[i] == null)
            {
                GameObject meshObj = new GameObject("clouds");
                meshObj.transform.parent = parent.transform;
                meshObj.transform.position = parent.transform.position;

                meshObj.AddComponent<MeshRenderer>();
                meshClouds[i] = meshObj.AddComponent<MeshFilter>();
                meshClouds[i].sharedMesh = new Mesh();
            }

            meshClouds[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.cloudMaterial;

            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int triIndex = 0;
            Vector2[] uv = new Vector2[resolution * resolution];

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int index = x + y * resolution;
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 axisA = new Vector3(directions[i].y, directions[i].z, directions[i].x);
                    Vector3 axisB = Vector3.Cross(directions[i], axisA);
                    Vector3 pointOnUnitCube = directions[i] + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[index] = pointOnUnitSphere * shapeSettings.planetRadius * (1 + shapeGenerator.elevationMinMax.Max);
                    uv[index] = vertices[index];

                    if (x != resolution - 1 && y != resolution - 1)
                    {
                        triangles[triIndex] = index;
                        triangles[triIndex + 1] = index + resolution + 1;
                        triangles[triIndex + 2] = index + resolution;

                        triangles[triIndex + 3] = index;
                        triangles[triIndex + 4] = index + 1;
                        triangles[triIndex + 5] = index + resolution + 1;
                        triIndex += 6;
                    }
                }
            }
            meshClouds[i].sharedMesh.Clear();
            meshClouds[i].sharedMesh.vertices = vertices;
            meshClouds[i].sharedMesh.triangles = triangles;
            meshClouds[i].sharedMesh.RecalculateNormals();
            meshClouds[i].sharedMesh.uv = uv;
        }
    }
}

