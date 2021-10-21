using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 1;
        public float persistence = .5f;
        public Vector3 centre;
        public float minValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultipler = .8f;
    }
}
