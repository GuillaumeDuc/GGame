using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Factory
{
    public FactoryDefinition definition;
    public int currentLv;
    private Dictionary<int, ResourceCollection> resourcesPerLv;
    private Dictionary<int, ResourceCollection> nextLvCostPerLv;

    public string name => definition.name;
    public Sprite sprite => definition.sprite;

    public Factory(FactoryDefinition definition)
    {
        this.definition = definition;
        initFactory(definition.GetBaseProduction(), definition.GetBaseNextLevelCost(), definition.maxLevel, definition.levelIncreasePercent);
    }

    public Factory(Factory f)
    {
        this.definition = f.definition;
        this.currentLv = f.currentLv;
        this.resourcesPerLv = new Dictionary<int, ResourceCollection>(f.resourcesPerLv);
        this.nextLvCostPerLv = new Dictionary<int, ResourceCollection>(f.nextLvCostPerLv);
    }

    public void initFactory(ResourceCollection resourcesProduction, ResourceCollection nextLvCost, int maxLv = 10, float incrPercent = .5f)
    {
        this.currentLv = 1;
        resourcesPerLv = new Dictionary<int, ResourceCollection>();
        nextLvCostPerLv = new Dictionary<int, ResourceCollection>();
        ResourceCollection currResources = new ResourceCollection(resourcesProduction);
        ResourceCollection currNextLvCost = new ResourceCollection(nextLvCost);
        // First lv
        resourcesPerLv.Add(currentLv, new ResourceCollection(currResources));
        nextLvCostPerLv.Add(currentLv, new ResourceCollection(currNextLvCost));
        for (int i = currentLv + 1; i <= maxLv; i++)
        {
            currResources.Multiply(1 + incrPercent);
            currNextLvCost.Multiply(10 * incrPercent);
            resourcesPerLv.Add(i, new ResourceCollection(currResources));
            nextLvCostPerLv.Add(i, new ResourceCollection(currNextLvCost));
        }
    }

    public void LevelUp()
    {
        if (this.currentLv < GetMaxLevel())
        {
            this.currentLv += 1;
        }
    }

    public int GetMaxLevel()
    {
        return resourcesPerLv.Count;
    }

    public ResourceCollection GetResourcesNeededLvUp()
    {
        return nextLvCostPerLv[currentLv];
    }

    public ResourceCollection GetResources()
    {
        return resourcesPerLv[currentLv];
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Factory f = (Factory)obj;
            return (definition == f.definition);
        }
    }

    public override int GetHashCode()
    {
        return definition.GetHashCode();
    }

    public override string ToString()
    {
        string produced = " ";
        foreach (Resource resource in GetResources())
        {
            produced += resource + " ";
        }

        return name + "\n Produce ";
    }
}
