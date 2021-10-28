using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreation : MonoBehaviour
{
    public int resolution = 200;
    public Material planetMaterial;
    public Material cloudMaterial;

    void Start()
    {
        ProceduralPlanet planet = gameObject.AddComponent<ProceduralPlanet>();
        planet.resolution = resolution;
        SetColorSettings(planet);
        SetShapeSettings(planet);

        planet.GeneratePlanet();
    }

    void SetShapeSettings(ProceduralPlanet planet)
    {
        planet.shapeSettings.noiseLayers = new ShapeSettings.NoiseLayer[2];
        for (int i = 0; i < planet.shapeSettings.noiseLayers.Length; i++)
        {
            ShapeSettings.NoiseLayer noiseLayer = new ShapeSettings.NoiseLayer();
            planet.shapeSettings.noiseLayers[i] = noiseLayer;
            NoiseSettings noiseSettings = new NoiseSettings();
            if (i == 0)
            {
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0.05f, 1, 5, 0.15f, 0.5f);
            }
            else
            {
                noiseLayer.useFirstLayerAsMask = true;
                // Random base roughness & roughness
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0.1f, Random.Range(5f, 10.0f), Random.Range(5f, 10.0f), 0.15f, 0.5f);
            }
            noiseLayer.noiseSettings = noiseSettings;
        }
    }

    void SetColorSettings(ProceduralPlanet planet)
    {
        planet.colorSettings.planetMaterial = new Material(planetMaterial);
        planet.colorSettings.cloudMaterial = new Material(cloudMaterial);

        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        Gradient gradient = new Gradient();

        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.black;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
        planet.colorSettings.oceanColor = gradient;

        SetBiomes(planet.colorSettings);
    }

    void SetBiomes(ColorSettings colorSettings)
    {
        ColorSettings.BiomeColorSettings biomeColorSettings = new ColorSettings.BiomeColorSettings();

        NoiseSettings noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0, 1, 1, 0.2f, 0.01f);
        biomeColorSettings.noise = noiseSettings;
        biomeColorSettings.noiseOffset = 0.2f;
        biomeColorSettings.noiseStrength = 0.2f;
        biomeColorSettings.blendAmount = 0f;

        ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[5];
        biomes[0] = PlanetCreationHelper.GetSnowBiome(0);
        biomes[1] = PlanetCreationHelper.GetTropicalBiome(0.1f);
        biomes[2] = PlanetCreationHelper.GetEarthBiome(0.3f);
        biomes[3] = PlanetCreationHelper.GetTropicalBiome(0.8f);
        biomes[4] = PlanetCreationHelper.GetSnowBiome(1);


        colorSettings.biomeColorSettings = biomeColorSettings;
        colorSettings.biomeColorSettings.biomes = biomes;
    }
}
