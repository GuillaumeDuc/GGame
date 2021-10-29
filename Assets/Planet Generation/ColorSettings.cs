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
        [Range(0, 1)]
        public float blendAmount;

        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0, 1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
