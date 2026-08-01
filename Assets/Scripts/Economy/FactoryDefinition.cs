using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactory", menuName = "GGame/Factories/Factory Definition")]
public class FactoryDefinition : ScriptableObject
{
    public Sprite sprite;
    public List<Resource> baseProduction = new List<Resource>();
    public List<Resource> baseNextLevelCost = new List<Resource>();
    public int maxLevel = 10;
    public float levelIncreasePercent = .5f;

    public ResourceCollection GetBaseProduction()
    {
        return new ResourceCollection(baseProduction);
    }

    public ResourceCollection GetBaseNextLevelCost()
    {
        return new ResourceCollection(baseNextLevelCost);
    }
}
