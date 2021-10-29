using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit
{
    string name { get; set; }
    int hp { get; set; }
    int currentHP { get; set; }
    int power { get; set; }
    int shield { get; set; }
    Sprite sprite { get; set; }
    ResourceCollection costToCreate { get; set; }
    string GetCost();
}