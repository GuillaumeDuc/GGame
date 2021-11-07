using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackView : MonoBehaviour
{
    public GameObject selectedPlanetGO, attackedPlanetGO;
    public GameObject itemSelectionGO;
    public GameObject galaxyView;
    public Button closeView;
    Planet attackedPlanet, selectedPlanet;

    void Start()
    {
        closeView.onClick.AddListener(CloseView);
    }

    public void setAttackView(Planet selectedPlanet, Planet attackedPlanet)
    {
        this.selectedPlanet = selectedPlanet;
        this.attackedPlanet = attackedPlanet;
        setPlanet(selectedPlanetGO, selectedPlanet);
        setPlanet(attackedPlanetGO, attackedPlanet);
        SetSelection(selectedPlanet);
    }

    void setPlanet(GameObject planetGO, Planet planet)
    {
        planetGO.GetComponentInChildren<Text>().text = planet.name;
        planetGO.GetComponentInChildren<Image>().sprite = null;
    }

    void SetSelection(Planet planet)
    {
        ItemSelection itemSelection = itemSelectionGO.GetComponent<ItemSelection>();
        foreach (var item in planet.units)
        {
            itemSelection.setItem(item.Key.name, item.Value);
        }
    }

    void CloseView()
    {
        itemSelectionGO.GetComponent<ItemSelection>().ResetContent();
        this.gameObject.SetActive(false);
        galaxyView.SetActive(true);
    }
}
