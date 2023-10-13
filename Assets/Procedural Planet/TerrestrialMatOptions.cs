using Unity.Mathematics;
using UnityEngine;

public enum TerrestrialType { Temperate, Cold, Desert, Forest, Ocean }
public class TerrestrialMatOptions : IPlanetMatOptions
{
    public TerrestrialType terrestrialType;
    public Vector3 rgbOffset;
    public float strength, baseRoughness, rougness, persistence, minValue, regionScale;
    public Vector3 offsetNoise;
    public float cloudStrength, cloudScale, exclusionCloudScale,
                exclusionCloudStrength, extraCloudFade, extraCloudScale, cloudSlowness;
    public float atmosphereStrength;
    public Color atmosphereColor;

    public TerrestrialMatOptions(TerrestrialType type)
    {
        switch (type)
        {
            case TerrestrialType.Desert:
                SetDefault((int)type);
                atmosphereColor = new Color(1, 0.533f, 0);
                return;
            case TerrestrialType.Temperate:
            case TerrestrialType.Forest:
            case TerrestrialType.Cold:
            case TerrestrialType.Ocean:
            default:
                SetDefault((int)type);
                return;
        }
    }

    public virtual void SetDefault(int planetType)
    {
        terrestrialType = (TerrestrialType)TerrestrialType.GetValues(terrestrialType.GetType()).GetValue(planetType);
        offsetNoise = new Vector3();
        rgbOffset = new Vector3();

        // Terrain
        strength = 1.5f;
        baseRoughness = .92f;
        rougness = 5;
        persistence = .15f;
        minValue = .61f;
        regionScale = .6f;

        // Cloud
        cloudStrength = .65f;
        cloudScale = 5.75f;
        exclusionCloudScale = 2.5f;
        exclusionCloudStrength = .94f;
        extraCloudFade = 15f;
        extraCloudScale = 30f;
        cloudSlowness = 150;

        // Atmosphere
        atmosphereStrength = 2.5f;
        atmosphereColor = new Color(0, 0.7436f, 1, 0); // blue
    }

    public virtual Material GetMat(Material terrestrialMat)
    {
        Material material = new(terrestrialMat);

        material.SetFloat("_Planet_Type", (int)terrestrialType);
        material.SetVector("_RGBOffset", rgbOffset);
        material.SetVector("_Offset_Noise", offsetNoise);

        material.SetFloat("_Strength", strength);
        material.SetFloat("_BaseRoughness", baseRoughness);
        material.SetFloat("_Roughness", rougness);
        material.SetFloat("_Persistence", persistence);
        material.SetFloat("_MinValue", minValue);
        material.SetFloat("_RegionScale", regionScale);

        material.SetFloat("_CloudStrength", cloudStrength);
        material.SetFloat("_CloudScale", cloudScale);
        material.SetFloat("_ExclusionCloudScale", exclusionCloudScale);
        material.SetFloat("_ExclusionCloudStrength", exclusionCloudStrength);
        material.SetFloat("_ExtraCloudScale", extraCloudScale);
        material.SetFloat("_ExtraCloudFade", extraCloudFade);
        material.SetFloat("_CloudSlowness", cloudSlowness);

        material.SetFloat("_AtmosphereStrength", atmosphereStrength);
        material.SetColor("_AtmosphereColor", atmosphereColor);

        return material;
    }
}