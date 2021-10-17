using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : Unit
{
    public string name { get; set; }
    public int hp { get; set; }
    public int currentHP { get; set; }
    public int power { get; set; }
    public int shield { get; set; }
    public Sprite sprite { get; set; }
    public ResourceCollection costToCreate { get; set; }
    public int overlandSpeed { get; set; }

    public Troop(string name, ResourceCollection costToCreate)
    {
        this.name = name;
        this.costToCreate = costToCreate;
    }

    public Troop(string name, Resource costToCreate)
    {
        this.name = name;
        this.costToCreate = new ResourceCollection() { costToCreate };
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

    public override string ToString()
    {
        return name;
    }
}
