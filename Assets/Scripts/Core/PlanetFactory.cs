using System;
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

    public static Planet CreatePlanet
    (
        string name,
        long size = 100,
        PlanetType planetType = PlanetType.Terrestrial,
        int options = 0,
        Vector3 position = new Vector3()
    )
    {
        switch (planetType)
        {
            case PlanetType.Terrestrial:
            default:
                TerrestrialMatOptions tmo = new((TerrestrialType)options);
                GameObject terrestrialGO = CreatePlanetGO(planetType, tmo, name, position, (float)size / 100);

                return new Planet(name, size, terrestrialGO);
            case PlanetType.GasGiant:
                GasMatOptions gmo = new((GasType)options);
                GameObject gasGO = CreatePlanetGO(planetType, gmo, name, position, (float)size / 100);

                return new Planet(name, size, gasGO);
            case PlanetType.Sunlike:
                SunMatOptions smo = new((SunType)options);
                GameObject sunlikeGO = CreatePlanetGO(planetType, smo, name, position, (float)size / 100);
                // Light
                Color light = smo.lightColor;
                sunlikeGO.GetComponent<PlanetScript>().SetLight(size * 10, size, light);

                return new Planet(name, size, sunlikeGO);
        }
    }

    static GameObject CreatePlanetGO(PlanetType planetType, IPlanetMatOptions options, string name, Vector3 position, float radius)
    {
        GameObject planetGO = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(planetPathFile));
        planetGO.name = name;
        planetGO.transform.position = position;

        PlanetScript ps = planetGO.GetComponent<PlanetScript>();
        ps.SetPlanet(planetType, options, radius);

        return planetGO;
    }
}
