using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SunType { Yellow, White }
public class SunMatOptions : IPlanetMatOptions
{
    public Color baseColor, rimColor;
    public SunType type;

    public SunMatOptions(SunType type)
    {
        this.type = type;
        switch (type)
        {
            case SunType.White:
                SetWhite();
                return;
            case SunType.Yellow:
            default:
                SetDefault((int)type);
                return;
        }
    }

    void SetWhite()
    {
        int p = 10;
        baseColor = new Color(.01f, .5f, .7f) * (p * p);
        rimColor = new Color(.1f, .65f, .85f) * (p * p * p);
    }

    public virtual void SetDefault(int type)
    {
        const int p = 10;
        baseColor = new Color(.44f, .01f, 0) * (p * (p / 2));
        rimColor = new Color(.70f, .12f, .05f) * (p * p);
    }

    public virtual Material GetMat(Material sunMat)
    {
        Material material = new(sunMat);

        material.SetColor("Base_Color", baseColor);
        material.SetColor("_Color", rimColor);

        return material;
    }
}
