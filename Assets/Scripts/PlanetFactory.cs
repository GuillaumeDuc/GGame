using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetType
{
    Terrestrial,
    GasGiant,
    Sunlike
}

public static class PlanetFactory
{
    const string planetPathFile = "Planet/Planet";
    static CreatePlanetAlien createPlanetAlien = new CreatePlanetAlien();
    static CreatePlanetCold createPlanetCold = new CreatePlanetCold();
    static CreatePlanetDesert createPlanetDesert = new CreatePlanetDesert();
    static CreatePlanetTemperate createPlanetTemperate = new CreatePlanetTemperate();

    public static Planet CreatePlanet(string name, long size = 100, PlanetType planetType = PlanetType.Terrestrial, Vector3 position = new Vector3())
    {
        switch (planetType)
        {
            case PlanetType.Terrestrial:
                GameObject planetGOEarth = CreatePlanetGO(planetType, name, position, (float)size / 100);
                return new Planet(name, size, planetGOEarth);
            case PlanetType.Sunlike:
                GameObject planetGOCold = CreatePlanetGO(planetType, name, position, (float)size / 100);
                return new Planet(name, size, planetGOCold);
            case PlanetType.GasGiant:
                GameObject planetGOAlien = CreatePlanetGO(planetType, name, position, (float)size / 100);
                return new Planet(name, size, planetGOAlien);
            default:
                GameObject planetGO = CreatePlanetGO(planetType, name, position, (float)size / 100);
                return new Planet(name, size, planetGO);
        }
    }

    static GameObject CreatePlanetGO(PlanetType planetType, string name, Vector3 position, float radius)
    {
        GameObject planetGO = Object.Instantiate(Resources.Load<GameObject>(planetPathFile));
        planetGO.name = name;
        planetGO.transform.position = position;

        PlanetScript ps = planetGO.GetComponent<PlanetScript>();
        ps.SetPlanet(planetType, radius);

        // createPlanet.SetPlanet(planetGO, planetMaterial, radius, 100);
        return planetGO;
    }
}
