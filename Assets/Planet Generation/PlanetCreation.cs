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
            if (i > 0)
            {
                noiseLayer.useFirstLayerAsMask = true;
            }
            NoiseSettings noiseSettings = new NoiseSettings();
            noiseSettings.filterType = NoiseSettings.FilterType.Simple;
            noiseSettings.simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings();
            noiseSettings.simpleNoiseSettings.numLayers = 5;
            noiseSettings.simpleNoiseSettings.strength = 1f;
            noiseSettings.simpleNoiseSettings.baseRoughness = 1;
            noiseSettings.simpleNoiseSettings.roughness = 5;
            noiseSettings.simpleNoiseSettings.persistence = .2f;
            noiseSettings.simpleNoiseSettings.minValue = 1f;

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
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
        planet.colorSettings.oceanColor = gradient;

        SetBiome(planet.colorSettings, 3);
    }

    void SetBiome(ColorSettings colorSettings, int biomeNb)
    {
        ColorSettings.BiomeColorSettings biomeColorSettings = new ColorSettings.BiomeColorSettings();

        NoiseSettings noiseSettings = new NoiseSettings();
        noiseSettings.filterType = NoiseSettings.FilterType.Simple;
        noiseSettings.simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings();
        biomeColorSettings.noise = noiseSettings;
        biomeColorSettings.noiseOffset = 0.3f;
        biomeColorSettings.noiseStrength = 2;
        biomeColorSettings.blendAmount = 0.3f;

        ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[biomeNb];
        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i] = new ColorSettings.BiomeColorSettings.Biome();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;
            Gradient gradient = new Gradient();

            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.green;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 1.0f;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            biomes[i].gradient = gradient;
            biomes[i].startHeight = i / (biomes.Length + 0.01f);
        }

        colorSettings.biomeColorSettings = biomeColorSettings;
        colorSettings.biomeColorSettings.biomes = biomes;
    }
}
