using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanetAlien : CreatePlanet
{
    public CreatePlanetAlien(int resolution, float radius, Material planetMaterial, Material cloudMaterial)
    {
        this.resolution = resolution;
        this.radius = radius;
        this.planetMaterial = planetMaterial;
        this.cloudMaterial = cloudMaterial;
    }

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
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0.08f, 2, 5, 0.15f, 0.5f);
            }
            else
            {
                noiseLayer.useFirstLayerAsMask = true;
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 1, 5, 5, 0.15f, 0.5f);
            }
            noiseLayer.noiseSettings = noiseSettings;
        }
    }

    protected override void SetColorSettings(ProceduralPlanet planet, Material planetMaterial, Material cloudMaterial)
    {
        // Atmosphere
        planet.colorSettings.planetMaterial = new Material(planetMaterial);
        planet.colorSettings.planetMaterial.SetColor("_atmosphereColor", new Color(.15f, .6f, 0));
        planet.colorSettings.planetMaterial.SetFloat("_rimIntensity", 1f);
        // Clouds
        planet.colorSettings.cloudMaterial = new Material(cloudMaterial);
        planet.colorSettings.cloudMaterial.SetColor("_color", Color.blue);
        planet.colorSettings.cloudMaterial.SetFloat("_distortionScale", (float)(8 / planet.shapeSettings.planetRadius));
        planet.colorSettings.cloudMaterial.SetFloat("_scale", (float)(8 / planet.shapeSettings.planetRadius));
        // Ocean
        Color[] oceanColors = new Color[2];
        oceanColors[0] = Color.black;
        oceanColors[1] = new Color(0.6f, 0, 0);
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
        biomes[0] = PlanetCreationHelper.GetAlienBiome(0);

        colorSettings.biomeColorSettings = biomeColorSettings;
        colorSettings.biomeColorSettings.biomes = biomes;
    }
}
