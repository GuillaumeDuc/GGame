using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSettings
{
    public Material planetMaterial;
    public Material cloudMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset, noiseStrength;
        public float blendAmount;

        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            public float startHeight;
            public float tintPercent;
        }
    }
}
