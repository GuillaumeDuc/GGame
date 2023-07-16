using UnityEngine;
using UnityEngine.UI;

public class DisplayFactory : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "Factory\n";
        p.factories.ForEach(factory =>
        {
            displayS += factory + "\n";
        });
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
