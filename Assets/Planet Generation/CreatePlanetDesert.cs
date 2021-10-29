using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanetDesert : CreatePlanet
{
    protected override void SetShapeSettings(ProceduralPlanet planet, float radius)
    {
        planet.shapeSettings.noiseLayers = new ShapeSettings.NoiseLayer[2];
        planet.shapeSettings.planetRadius = radius;
        for (int i = 0; i < planet.shapeSettings.noiseLayers.Length; i++)
        {
            ShapeSettings.NoiseLayer noiseLayer = new ShapeSettings.NoiseLayer();
            planet.shapeSettings.noiseLayers[i] = noiseLayer;
            NoiseSettings noiseSettings = new NoiseSettings();
            if (i == 0)
            {
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, .05f, .55f, 4, .15f, .5f);
            }
            else
            {
                noiseLayer.useFirstLayerAsMask = true;
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, .1f, 5, 5, .15f, .5f);
            }
            noiseLayer.noiseSettings = noiseSettings;
        }
    }

    protected override void SetColorSettings(ProceduralPlanet planet, Material planetMaterial, Material cloudMaterial)
    {
        // Atmosphere
        planet.colorSettings.planetMaterial = new Material(planetMaterial);
        planet.colorSettings.planetMaterial.SetColor("_atmosphereColor", new Color(.8f, .4f, 0));
        planet.colorSettings.planetMaterial.SetFloat("_smoothness", 0f);
        planet.colorSettings.planetMaterial.SetFloat("_rimIntensity", 1.5f);
        planet.colorSettings.planetMaterial.SetFloat("_atmosphereIntensity", .3f);

        // Clouds
        planet.colorSettings.cloudMaterial = new Material(cloudMaterial);
        planet.colorSettings.cloudMaterial.SetFloat("_distortionScale", (float)(8 / planet.shapeSettings.planetRadius));
        planet.colorSettings.cloudMaterial.SetFloat("_scale", (float)(8 / planet.shapeSettings.planetRadius));
        // Ocean
        Color[] oceanColors = new Color[2];
        oceanColors[0] = new Color(0.18f, 0.18f, 0);
        oceanColors[1] = new Color(0.38f, 0.31f, 0.08f);
        planet.colorSettings.oceanColor = PlanetCreationHelper.GetGradient(oceanColors);

        SetBiomes(planet.colorSettings);
    }

    protected override void SetBiomes(ColorSettings colorSettings)
    {
        ColorSettings.BiomeColorSettings biomeColorSettings = new ColorSettings.BiomeColorSettings();

        NoiseSettings noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0, 1, 1, 0.2f, 0.01f);
        biomeColorSettings.noise = noiseSettings;
        biomeColorSettings.noiseOffset = 0.2f;
        biomeColorSettings.noiseStrength = 0.2f;
        biomeColorSettings.blendAmount = 0.4f;

        ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[1];
        biomes[0] = PlanetCreationHelper.GetDesertBiome(0);

        colorSettings.biomeColorSettings = biomeColorSettings;
        colorSettings.biomeColorSettings.biomes = biomes;
    }
}
