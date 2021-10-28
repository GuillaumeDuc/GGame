using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSettings
{
    public float planetRadius = 1;
    public NoiseLayer[] noiseLayers;
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}
