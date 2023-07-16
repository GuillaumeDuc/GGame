using UnityEngine;
using UnityEngine.UI;

public class DisplayUnit : DisplayUI
{
    public override void Display(Planet p)
    {
        string displayS = "Unit\n";
        foreach (var unit in p.units)
        {
            displayS += unit.Key.ToString() + "\nNumber : " + unit.Value + "\n";
        }
        this.gameObject.GetComponent<Text>().text = displayS;
    }
}
