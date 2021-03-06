using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public TypeResource type;
    public enum TypeResource
    {
        Metal,
        Gaz,
        Water
    }
    public long amount;

    public Resource(Resource resource)
    {
        this.type = resource.type;
        this.amount = resource.amount;
    }

    public Resource(TypeResource type, long amount)
    {
        this.type = type;
        this.amount = amount;
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
            Resource r = (Resource)obj;
            return (type == r.type);
        }
    }

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }

    public override string ToString()
    {
        return type + " : " + amount;
    }
}
