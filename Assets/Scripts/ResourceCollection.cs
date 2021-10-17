using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class ResourceCollection : Collection<Resource>, IEnumerator, IEnumerable
{
    int position = -1;
    public ResourceCollection() : base() { }
    public ResourceCollection(ResourceCollection resources)
    {
        for (int i = 0; i < resources.Items.Count; i++)
        {
            this.InsertItem(i, resources.Items[i]);
        }
    }

    protected override void InsertItem(int index, Resource newItem)
    {
        bool found = false;
        int i = 0;
        // Search for matching resource
        while (!found && i < Items.Count)
        {
            if (Items[i].Equals(newItem))
            {
                Items[i].amount += newItem.amount;
                found = !found;
            }
            i++;
        }
        // No match, add item to Items
        if (!found)
        {
            base.InsertItem(index, new Resource(newItem));
        }
    }

    public void Add(ResourceCollection newCollection)
    {
        for (int i = 0; i < newCollection.Items.Count; i++)
        {
            this.InsertItem(Items.Count, newCollection.Items[i]);
        }
    }

    public void Substract(Resource resource)
    {
        Resource match = this.Get(resource);
        if (match != null)
        {
            match.amount -= resource.amount;
        }
    }

    public void Substract(ResourceCollection resources)
    {
        for (int i = 0; i < resources.Items.Count; i++)
        {
            Substract(resources.Items[i]);
        }
    }

    public void Multiply(float multiplier)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].amount = (int)(Items[i].amount * multiplier);
        }
    }

    public Resource Get(Resource resource)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Equals(resource))
            {
                return Items[i];
            }
        }
        return null;
    }

    public bool ContainsEnough(ResourceCollection resourcesNeeded)
    {
        bool enough = true;
        int i = 0;
        // Search for missing resource
        while (enough && i < resourcesNeeded.Items.Count)
        {
            Resource resource = this.Get(resourcesNeeded.Items[i]);
            // Missing resource or not enough amount
            if (resource == null || resource.amount < resourcesNeeded.Items[i].amount)
            {
                enough = false;
            }
            i++;
        }
        return enough;
    }

    //IEnumerator
    public bool MoveNext()
    {
        position++;
        return (position < Items.Count);
    }
    //IEnumerable
    public void Reset()
    {
        position = -1;
    }
    //IEnumerable
    public object Current
    {
        get { return Items[position]; }
    }
}
