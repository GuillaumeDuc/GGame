using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Planet
{
    public string name;
    public long size, occupiedSize;
    public List<Factory> factories = new List<Factory>();
    public ResourceCollection resources = new ResourceCollection();
    public Dictionary<Unit, int> units = new Dictionary<Unit, int>();

    public Planet(string name, long size = 1)
    {
        this.name = name;
        this.size = size;
        occupiedSize = 0;
    }

    public void CreateUnit(Unit unit)
    {
        if (resources.ContainsEnough(unit.costToCreate))
        {
            resources.Substract(unit.costToCreate);
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
            resources.Add(factory.GetResources());
        });
    }
}
