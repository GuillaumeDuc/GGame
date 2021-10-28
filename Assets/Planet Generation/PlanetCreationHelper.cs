using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetCreationHelper
{
    public static NoiseSettings GetSimpleNoiseSettings(int numLayers, float strength, float baseRoughness, float roughness, float persistence, float minValue)
    {
        NoiseSettings noiseSettings = new NoiseSettings();
        noiseSettings.filterType = NoiseSettings.FilterType.Simple;
        noiseSettings.simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings();
        noiseSettings.simpleNoiseSettings.numLayers = numLayers;
        noiseSettings.simpleNoiseSettings.strength = strength;
        noiseSettings.simpleNoiseSettings.baseRoughness = baseRoughness;
        noiseSettings.simpleNoiseSettings.roughness = roughness;
        noiseSettings.simpleNoiseSettings.persistence = persistence;
        noiseSettings.simpleNoiseSettings.minValue = minValue;
        return noiseSettings;
    }

    public static Gradient GetGradient(Color[] colors)
    {
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        Gradient gradient = new Gradient();

        colorKey = new GradientColorKey[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colorKey[i].color = colors[i];
            colorKey[i].time = (float)i / colors.Length;
        }

        gradient.SetKeys(colorKey, alphaKey);

        return gradient;
    }

    public static ColorSettings.BiomeColorSettings.Biome GetEarthBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[4];
        // Green
        colors[0] = new Color(0, 0.5f, 0);
        // Yellow
        colors[1] = new Color(0.6f, 0.6f, 0);
        // Brown
        colors[2] = new Color(0.5f, 0.2f, 0);
        colors[3] = Color.white;

        biome.gradient = PlanetCreationHelper.GetGradient(colors);
        biome.startHeight = startHeight;
        return biome;
    }
    public static ColorSettings.BiomeColorSettings.Biome GetTropicalBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[2];
        // Green
        colors[0] = new Color(0, 0.3f, 0);
        colors[1] = Color.white;

        biome.gradient = PlanetCreationHelper.GetGradient(colors);
        biome.startHeight = startHeight;
        return biome;
    }

    public static ColorSettings.BiomeColorSettings.Biome GetSnowBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[2];
        colors[0] = Color.gray;
        colors[1] = Color.white;

        biome.gradient = PlanetCreationHelper.GetGradient(colors);
        biome.startHeight = startHeight;
        return biome;
    }
}
