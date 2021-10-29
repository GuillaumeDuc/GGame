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
        // Put some random
        noiseSettings.simpleNoiseSettings.centre = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
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

    public static ColorSettings.BiomeColorSettings.Biome GetDesertBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[4];
        // Green
        colors[0] = new Color(0.38f, 0.31f, 0.08f);
        // Yellow
        colors[1] = new Color(0.78f, 0.53f, 0.13f);
        // Brown
        colors[2] = new Color(0.95f, 0.88f, 0.43f);
        colors[3] = Color.white;

        biome.gradient = PlanetCreationHelper.GetGradient(colors);
        biome.startHeight = startHeight;
        return biome;
    }
    public static ColorSettings.BiomeColorSettings.Biome GetTropicalBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[5];
        // Green
        colors[0] = new Color(0.01f, 0.35f, 0.05f);
        colors[1] = new Color(0.01f, 0.35f, 0.05f);
        colors[2] = new Color(0.01f, 0.35f, 0.05f);
        colors[3] = new Color(0.07f, 0.47f, 0.16f);
        colors[4] = Color.white;

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

    public static ColorSettings.BiomeColorSettings.Biome GetAlienBiome(float startHeight)
    {
        ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
        Color[] colors = new Color[3];
        colors[0] = Color.cyan;
        colors[1] = Color.cyan;
        colors[2] = Color.magenta;

        biome.gradient = PlanetCreationHelper.GetGradient(colors);
        biome.startHeight = startHeight;
        return biome;
    }
}
