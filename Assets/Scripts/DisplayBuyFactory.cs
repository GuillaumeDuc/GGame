using UnityEngine;
using UnityEngine.UI;

public class DisplayBuyFactory : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "Factory\n";
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
