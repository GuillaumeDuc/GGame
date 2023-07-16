using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Planet
{
    public string name;
    public long size, occupiedSize;
    public Player owner;
    public List<Factory> factories = new List<Factory>();
    public ResourceCollection resources = new ResourceCollection();
    public Dictionary<Unit, int> units = new Dictionary<Unit, int>();
    public GameObject planetGO;

    public Planet(string name, long size = 1, GameObject planetGO = null)
    {
        this.name = name;
        this.size = size;
        this.planetGO = planetGO;
        occupiedSize = 0;
        // Default production
        factories.AddRange(FactoryList.GetDefaultFactories());
        int deepness = 1;
        factories.ForEach(factory =>
        {
            ResourceCollection resourcesNeeded = new ResourceCollection(factory.GetResources());
            resourcesNeeded.Multiply(size / deepness);
            factory.initFactory(resourcesNeeded, resourcesNeeded);
            deepness++;
        });
    }

    public void CreateUnit(Unit unit)
    {
        bool canConsume = ConsumeResource(unit.costToCreate);
        if (canConsume)
        {
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

    public bool ConsumeResource(ResourceCollection resourceNeeded)
    {
        if (resources.ContainsEnough(resourceNeeded))
        {
            resources.Substract(resourceNeeded);
            return true;
        }
        return false;
    }

    public void ProduceResources()
    {
        factories.ForEach(factory =>
        {
            resources.Add(factory.GetResources());
        });
    }

    public void LevelUpFactory(Factory factory)
    {
        Factory matchFactory = factories.First(f => f.Equals(factory));
        // Check for max level
        if (factory.currentLv < factory.GetMaxLevel())
        {
            bool canConsume = ConsumeResource(matchFactory.GetResourcesNeededLvUp());
            if (canConsume)
            {
                matchFactory.LevelUp();
            }
        }
    }

    public Dictionary<Ship, int> GetShips()
    {
        Dictionary<Ship, int> ships = new Dictionary<Ship, int>();
        foreach (var item in units)
        {
            if (item.Key is Ship)
            {
                ships.Add((Ship)item.Key, item.Value);
            }
        }
        return ships;
    }
}
