using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Factory
{
    string name;
    public int currentLv;
    private Dictionary<int, ResourceCollection> resourcesPerLv = new Dictionary<int, ResourceCollection>();
    private Dictionary<int, ResourceCollection> nextLvCostPerLv = new Dictionary<int, ResourceCollection>();

    public Factory(string name, Resource resource, int max = 10, float incrPercent = .1f)
    {
        this.name = name;
        initFactory(new ResourceCollection() { resource }, new ResourceCollection() { resource }, max, incrPercent);
    }

    public Factory(string name, ResourceCollection resourcesProduction, ResourceCollection nextLvCost, int max = 10, float incrPercent = .1f)
    {
        this.name = name;
        initFactory(resourcesProduction, nextLvCost, max, incrPercent);
    }

    public Factory(Factory f)
    {
        this.name = f.name;
        this.currentLv = f.currentLv;
        this.resourcesPerLv = new Dictionary<int, ResourceCollection>(f.resourcesPerLv);
        this.nextLvCostPerLv = new Dictionary<int, ResourceCollection>(f.nextLvCostPerLv);
    }

    private void initFactory(ResourceCollection resourcesProduction, ResourceCollection nextLvCost, int maxLv, float incrPercent)
    {
        this.currentLv = 1;
        ResourceCollection currResources = new ResourceCollection(resourcesProduction);
        ResourceCollection currNextLvCost = new ResourceCollection(nextLvCost);
        // First lv
        resourcesPerLv.Add(currentLv, new ResourceCollection(currResources));
        nextLvCostPerLv.Add(currentLv, new ResourceCollection(currNextLvCost));
        for (int i = currentLv + 1; i <= maxLv; i++)
        {
            currResources.Multiply(1 + incrPercent);
            currNextLvCost.Multiply(1 + incrPercent);
            resourcesPerLv.Add(i, new ResourceCollection(currResources));
            nextLvCostPerLv.Add(i, new ResourceCollection(currNextLvCost));
        }
    }

    public ResourceCollection GetResources()
    {
        return resourcesPerLv[currentLv];
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
