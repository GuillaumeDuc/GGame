using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class DisplayBuyUnit : DisplayUI
{
    public GameObject scrollViewContent;
    public GameObject unitContentPrefab;
    public GameObject unitField;

    private bool added = false;
    public override void Display(Planet p)
    {
        // Title
        this.gameObject.GetComponent<Text>().text = "Units";
        // Only add them one time in UI
        if (!added)
        {
            AddAllUnits();
            added = !added;
        }
    }

    void AddAllUnits()
    {
        UnitList.getShips().ForEach(ship =>
        {
            AddUnitInView(ship);
        });
        UnitList.getDefenses().ForEach(defense =>
        {
            AddUnitInView(defense);
        });
        UnitList.getTroops().ForEach(troop =>
        {
            AddUnitInView(troop);
        });
    }

    private void AddUnitInView(Unit unit)
    {
        if (unit != null)
        {
            GameObject go = Instantiate(unitContentPrefab);
            // Unit image
            Image img = go.GetComponentInChildren<Image>();
            img.sprite = img.sprite ?? unit.sprite;
            // Unit description
            DisplayUnitDescription(go.transform.GetChild(0).GetChild(0).GetChild(0).gameObject, unit);
            // Unit buy button
            go.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickBuy(unit));
            go.transform.SetParent(scrollViewContent.transform, false);
        }
    }

    private void OnClickBuy(Unit unit)
    {
        Store.selectedPlanet.CreateUnit(unit);
        Store.UpdateUI();
    }

    private void DisplayUnitDescription(GameObject content, Unit unit)
    {
        SetLabel(content, "Name", unit.name);
        SetLabel(content, "Health", "" + unit.hp);
        SetLabel(content, "Power", "" + unit.power);
        SetLabel(content, "Shield", "" + unit.shield);
        if (!(unit is Defense))
        {
            SetLabel(content, "Overland Speed Test test test teteeeest", unit is Ship ? ((Ship)unit).overlandSpeed + "" : ((Troop)unit).overlandSpeed + "");
        }
        SetLabel(content, "Cost", unit.GetCost());
    }

    private void SetLabel(GameObject content, string label, string text)
    {
        GameObject go = Instantiate(unitField);
        go.transform.SetParent(content.transform, false);
        // Label
        go.transform.GetChild(0).GetComponent<Text>().text = label;
        // Text
        go.transform.GetChild(1).GetComponent<Text>().text = text;
    }
}