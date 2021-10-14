using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    string name;
    List<Resource> resourcesProduced;

    public Factory(string name, Resource resource)
    {
        this.name = name;
        resourcesProduced = new List<Resource>() { resource };
    }

    public Factory(string name, List<Resource> resources)
    {
        this.name = name;
        resourcesProduced = new List<Resource>(resources);
    }

    public List<Resource> GetResources()
    {
        return resourcesProduced;
    }

    public override string ToString()
    {
        string produced = " ";
        resourcesProduced.ForEach(resource =>
        {
            produced += resource + " ";
        });
        return name + "\n Produce " + produced;
    }
}
