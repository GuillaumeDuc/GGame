using System;
using Unity.Mathematics;
using UnityEngine;

public enum GasType { Hot, Cold }
public class GasMatOptions : IPlanetMatOptions
{
    public GasType gasType;
    public Vector3 distortion, positionOffset;
    public Vector3 rgbOffset;
    public float circleStrength, absStrength, absScale, snoiseStrength, roughness;

    public GasMatOptions(GasType type)
    {
        switch (type)
        {
            case GasType.Hot:
            case GasType.Cold:
            default:
                SetDefault((int)type);
                return;
        }
    }
 
    public virtual void SetDefault(int planetType)
    {
        gasType = (GasType)GasType.GetValues(gasType.GetType()).GetValue(planetType);
        distortion = new Vector3(0.05f, 7, 0.05f);
        positionOffset = new Vector3();
        rgbOffset = new Vector3();

        circleStrength = 1.52f;
        absStrength = 3.69f;
        absScale = 9.53f;
        snoiseStrength = 2.18f;
        roughness = 3;
    }

    public virtual Material GetMat(Material gasMat)
    {
        Material material = new(gasMat);

        material.SetFloat("_Type", (int)gasType);
        material.SetVector("_Distortion", distortion);
        material.SetVector("_PostionOffset", positionOffset);

        material.SetFloat("_CircleStrength", circleStrength);
        material.SetFloat("_AbsStrength", absStrength);
        material.SetFloat("_AbsScale", absScale);
        material.SetFloat("_SnoiseStrength", snoiseStrength);
        material.SetFloat("_Roughness", roughness);

        material.SetVector("_RGBOffset", rgbOffset);

        return material;
    }
}