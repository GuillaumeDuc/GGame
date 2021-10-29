using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemGeneration : MonoBehaviour
{
    public Material planetMaterial;
    public Material cloudMaterial;
    public int planetNumber;
    void Start()
    {
        List<GameObject> planets = new List<GameObject>();
        Vector3 pos = new Vector3();
        for (int i = 0; i < planetNumber; i++)
        {
            GameObject planetGO = new GameObject();
            planetGO.transform.position = pos;
            float radius = Random.Range(1f, 5f);
            if (i % 4 == 0)
            {
                CreatePlanetAlien alien = new CreatePlanetAlien(100, radius, planetMaterial, cloudMaterial);
                alien.SetPlanet(planetGO);
            }
            else if (i % 4 == 1)
            {
                CreatePlanetTemperate cp = new CreatePlanetTemperate(100, radius, planetMaterial, cloudMaterial);
                cp.SetPlanet(planetGO);
            }
            else if (i % 4 == 2)
            {
                CreatePlanetCold cpc = new CreatePlanetCold(100, radius, planetMaterial, cloudMaterial);
                cpc.SetPlanet(planetGO);
            }
            else
            {
                CreatePlanetDesert cpd = new CreatePlanetDesert(100, radius, planetMaterial, cloudMaterial);
                cpd.SetPlanet(planetGO);
            }
            pos.x += 2 + (radius * 2);
        }
    }
}
