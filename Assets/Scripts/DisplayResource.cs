using UnityEngine;
using UnityEngine.UI;

public class DisplayResource : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "";
        foreach (Resource resource in p.resources)
        {
            displayS += resource + " ";
        }
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
