using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit
{
    string name { get; }
    int hp { get; }
    int power { get; }
    int shield { get; }
    Sprite sprite { get; }
    ResourceCollection costToCreate { get; }
    string GetCost();
}