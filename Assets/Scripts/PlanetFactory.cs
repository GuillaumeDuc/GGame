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

    public static Planet CreatePlanet(string name, long size = 100, PlanetType planetType = PlanetType.Terrestrial, Vector3 position = new Vector3())
    {
        switch (planetType)
        {
            case PlanetType.Terrestrial:
            default:
                GameObject terrestrialGO = CreatePlanetGO(planetType, new TerrestrialMatOptions(TerrestrialType.Ocean), name, position, (float)size / 100);
                return new Planet(name, size, terrestrialGO);
            case PlanetType.GasGiant:
                GameObject gasGO = CreatePlanetGO(planetType, new GasMatOptions(GasType.Hot), name, position, (float)size / 100);
                return new Planet(name, size, gasGO);
            case PlanetType.Sunlike:
                GameObject sunlikeGO = CreatePlanetGO(planetType, new TerrestrialMatOptions(TerrestrialType.Temperate), name, position, (float)size / 100);
                return new Planet(name, size, sunlikeGO);
        }
    }

    static GameObject CreatePlanetGO(PlanetType planetType, IPlanetMatOptions options, string name, Vector3 position, float radius)
    {
        GameObject planetGO = Object.Instantiate(Resources.Load<GameObject>(planetPathFile));
        planetGO.name = name;
        planetGO.transform.position = position;

        PlanetScript ps = planetGO.GetComponent<PlanetScript>();
        ps.SetPlanet(planetType, options, radius);

        return planetGO;
    }
}
