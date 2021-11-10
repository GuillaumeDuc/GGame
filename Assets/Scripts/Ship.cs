using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Unit
{
    public string name { get; set; }
    public int hp { get; set; }
    public int currentHP { get; set; }
    public int power { get; set; }
    public int shield { get; set; }
    public Sprite sprite { get; set; }
    public ResourceCollection costToCreate { get; set; }
    public int overlandSpeed { get; set; }
    public int speed { get; set; }
    public Resource travelCost { get; set; }

    public Ship(string name, int speed, Resource travelCost, ResourceCollection costToCreate)
    {
        this.name = name;
        this.costToCreate = costToCreate;
        this.speed = speed;
        this.travelCost = travelCost;
    }

    public Ship(string name, int speed, Resource travelCost, Resource costToCreate)
    {
        this.name = name;
        this.costToCreate = new ResourceCollection() { costToCreate };
        this.speed = speed;
        this.travelCost = travelCost;
    }
    public string GetCost()
    {
        string s = "";
        foreach (var resource in costToCreate)
        {
            s += resource + "\n";
        }
        return s;
    }

    public Resource getTravelCost(float distance)
    {
        long costDistance = (long)distance / speed;
        Resource costResource = new Resource(travelCost);
        costResource.amount *= costDistance;
        return costResource;
    }

    public override string ToString()
    {
        return name;
    }
}
