using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Factory
{
    string name;
    public int currentLv;
    ResourceCollection resourcesProduced;
    private Dictionary<int, List<Resource>> resourcesPerLv = new Dictionary<int, List<Resource>>();
    private Dictionary<int, List<Resource>> nextLvCostPerLv = new Dictionary<int, List<Resource>>();

    public Factory(string name, Resource resource)
    {
        this.name = name;
        resourcesProduced = new ResourceCollection() { resource };
    }


    public Factory(string name, ResourceCollection resources)
    {
        this.name = name;
        resourcesProduced = new ResourceCollection(resources);
    }

    public Factory(string name, List<Resource> resourcesList, List<Resource> nextLvCost, int max = 10, int incrPercent = 10)
    {
        this.name = name;
        this.currentLv = 1;
        initFactory(resourcesList, nextLvCost, max, incrPercent);
    }
    private void initFactory(List<Resource> resourcesList, List<Resource> nextLvCost, int maxLv, float incrPercent)
    {
        List<Resource> currResources = new List<Resource>(resourcesList);
        for (int i = 1; i <= maxLv; i++)
        {
        }
    }

    public ResourceCollection GetResources()
    {
        return resourcesProduced;
    }

    public override string ToString()
    {
        string produced = " ";
        foreach (Resource resource in resourcesProduced)
        {
            produced += resource + " ";
        }

        return name + "\n Produce ";
    }
}
