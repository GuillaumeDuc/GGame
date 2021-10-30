using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetFactory
{
    public enum PlanetType
    {
        Earth,
        Cold,
        Desert,
        Alien
    }
    static CreatePlanetAlien createPlanetAlien = new CreatePlanetAlien();
    static CreatePlanetCold createPlanetCold = new CreatePlanetCold();
    static CreatePlanetDesert createPlanetDesert = new CreatePlanetDesert();
    static CreatePlanetTemperate createPlanetTemperate = new CreatePlanetTemperate();

    public static Planet CreatePlanet(string name, Material planetMaterial, Material cloudMaterial, long size = 100, PlanetType planetType = PlanetType.Earth, Vector3 position = new Vector3())
    {
        switch (planetType)
        {
            case PlanetType.Earth:
                GameObject planetGOEarth = CreatePlanetGO(createPlanetTemperate, name, position, planetMaterial, cloudMaterial, (float)size / 100);
                return new Planet(name, size, planetGOEarth);
            case PlanetType.Cold:
                GameObject planetGOCold = CreatePlanetGO(createPlanetCold, name, position, planetMaterial, cloudMaterial, (float)size / 100);
                return new Planet(name, size, planetGOCold);
            case PlanetType.Desert:
                GameObject planetGODesert = CreatePlanetGO(createPlanetDesert, name, position, planetMaterial, cloudMaterial, (float)size / 100);
                return new Planet(name, size, planetGODesert);
            case PlanetType.Alien:
                GameObject planetGOAlien = CreatePlanetGO(createPlanetAlien, name, position, planetMaterial, cloudMaterial, (float)size / 100);
                return new Planet(name, size, planetGOAlien);
            default:
                GameObject planetGO = CreatePlanetGO(createPlanetTemperate, name, position, planetMaterial, cloudMaterial, (float)size / 100);
                return new Planet(name, size, planetGO);
        }
    }

    static GameObject CreatePlanetGO(CreatePlanet createPlanet, string name, Vector3 position, Material planetMaterial, Material cloudMaterial, float radius)
    {
        GameObject planetGO = new GameObject(name);
        planetGO.transform.position = position;
        createPlanet.SetPlanet(planetGO, planetMaterial, cloudMaterial, radius, 100);
        return planetGO;
    }
}
