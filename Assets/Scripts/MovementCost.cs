using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementCost : MonoBehaviour
{
    public GameObject itemSelectionGO;
    public GameObject resourceDislayGO;
    public GameObject buttonGO;

    private Planet planet, attacked;
    public void SetSelection(Planet planet, Planet attacked)
    {
        this.planet = planet;
        this.attacked = attacked;
        // Set item selection
        ItemSelection itemSelection = itemSelectionGO.GetComponent<ItemSelection>();
        foreach (var item in planet.GetShips())
        {
            itemSelection.setItem(item.Key.name, item.Value, OnChangeSelection);
        }
        // Initialize button
        buttonGO.GetComponent<Button>().onClick.AddListener(() => AttackPlanet(planet, attacked));
    }

    void OnChangeSelection(string shipName, string quantity)
    {
        // Calculate resource needed for ships
        Ship ship = UnitList.getShips().Find(ship => ship.name == shipName);
        float distance = Vector3.Distance(planet.planetGO.gameObject.transform.position, attacked.planetGO.transform.position);
        Resource costResource = new Resource(ship.getTravelCost(distance));
        costResource.amount *= System.Int32.Parse(quantity);
        // Set resources in view
        resourceDislayGO.GetComponent<DisplayResourceNavigation>().setItem(shipName, costResource);
    }

    void AttackPlanet(Planet attacking, Planet attacked)
    {
        // attacking.Attack(attacked, selectedShips);
    }

    public void CloseView()
    {
        itemSelectionGO.GetComponent<ItemSelection>().ResetContent();
        resourceDislayGO.GetComponent<DisplayResourceNavigation>().ResetContent();
    }
}
