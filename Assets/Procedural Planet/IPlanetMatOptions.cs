using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlanetMatOptions
{
    void SetDefault(int planetType);
    Material GetMat(Material planetMat);
}
