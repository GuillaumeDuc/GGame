using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanetCold : CreatePlanet
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
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0.05f, 1, 5, 0.15f, 0.5f);
            }
            else
            {
                noiseLayer.useFirstLayerAsMask = true;
                noiseSettings = PlanetCreationHelper.GetSimpleNoiseSettings(5, 0.1f, 5, 5, 0.15f, 0.5f);
            }
            noiseLayer.noiseSettings = noiseSettings;
        }
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
        biomes[0] = PlanetCreationHelper.GetSnowBiome(0);

        colorSettings.biomeColorSettings = biomeColorSettings;
        colorSettings.biomeColorSettings.biomes = biomes;
    }
}
