using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatePlanet
{
    public virtual void SetPlanet(GameObject planetGO, Material planetMaterial, Material cloudMaterial, float radius = 1, int resolution = 100)
    {
        ProceduralPlanet planetScript = planetGO.AddComponent<ProceduralPlanet>();
        planetScript.resolution = resolution;
        SetShapeSettings(planetScript, radius);
        SetColorSettings(planetScript, planetMaterial, cloudMaterial);

        planetScript.GeneratePlanet();
    }

    protected abstract void SetShapeSettings(ProceduralPlanet proceduralPlanet, float radius);

    /// <summary>method <c>SetColorSettings</c> set default ocean (blue) and call SetBiomes.</summary>
    protected virtual void SetColorSettings(ProceduralPlanet planet, Material planetMaterial, Material cloudMaterial)
    {
        planet.colorSettings.planetMaterial = new Material(planetMaterial);
        planet.colorSettings.cloudMaterial = new Material(cloudMaterial);
        planet.colorSettings.cloudMaterial.SetFloat("_distortionScale", (float)(8 / planet.shapeSettings.planetRadius));
        planet.colorSettings.cloudMaterial.SetFloat("_scale", (float)(8 / planet.shapeSettings.planetRadius));
        planet.colorSettings.planetMaterial.SetFloat("_rimIntensity", 1.8f);
        planet.colorSettings.planetMaterial.SetFloat("_atmosphereIntensity", .3f);

        Color[] oceanColors = new Color[2];
        oceanColors[0] = Color.black;
        oceanColors[1] = new Color(0, 0, 0.3f);
        planet.colorSettings.oceanColor = PlanetCreationHelper.GetGradient(oceanColors);

        SetBiomes(planet.colorSettings);
    }

    protected abstract void SetBiomes(ColorSettings colorSettings);
}
