using UnityEngine;
using UnityEngine.UI;

public class DisplayPlanet : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "Planet\n";
        displayS += p.name + "\nSpace : " + p.size + "\nOccupied Space : " + p.occupiedSize;
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
