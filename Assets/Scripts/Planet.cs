using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Planet
{
    public string name;
    public long size, occupiedSize;
    public List<Factory> factories = new List<Factory>();
    public List<Resource> resources = new List<Resource>();
    public Dictionary<Unit, int> units = new Dictionary<Unit, int>();

    public Planet(string name, long size = 1)
    {
        this.name = name;
        this.size = size;
        occupiedSize = 0;
    }

    public void CreateUnit(Unit unit)
    {
        if (ContainsEnoughResources(unit.costToCreate))
        {
            ConsumeResources(unit.costToCreate);
            try
            {
                units[unit] += 1;
            }
            catch
            {
                units.Add(unit, 1);
            }
        }
    }

    public void ProduceResources()
    {
        factories.ForEach(factory =>
        {
            foreach (Resource resource in factory.GetResources())
            {
                AddResource(resource);
            }
        });
    }

    public void ConsumeResources(List<Resource> list)
    {
        foreach (Resource resource in list)
        {
            SubstractResource(resource);
        }
    }

    private void AddResource(Resource add)
    {
        Resource resource = resources.Find(resource =>
        {
            return resource.type == add.type;
        });

        if (resource == null)
        {
            resources.Add(new Resource(add));
        }
        else
        {
            resource.amount += add.amount;
        }
    }

    private void SubstractResource(Resource substract)
    {
        Resource resource = resources.Find(resource =>
        {
            return resource.type == substract.type;
        });

        if (resource != null)
        {
            resource.amount -= substract.amount;
        }
    }

    private bool ContainsEnoughResources(List<Resource> resourcesNeeded)
    {
        bool enough = resourcesNeeded.Any(resourceNeeded =>
        {
            Resource match = resources.Find(resource =>
            {
                return resource.type == resourceNeeded.type;
            });
            return match == null || match.amount < resourceNeeded.amount;
        });
        return !enough;
    }
}
