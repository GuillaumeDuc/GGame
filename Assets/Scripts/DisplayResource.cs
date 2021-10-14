using UnityEngine;
using UnityEngine.UI;

public class DisplayResource : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "";
        p.resources.ForEach(resource =>
        {
            displayS += resource + " ";
        });
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
