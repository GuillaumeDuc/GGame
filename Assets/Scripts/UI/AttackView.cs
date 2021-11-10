using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackView : MonoBehaviour
{
    public GameObject selectedPlanetGO, attackedPlanetGO;
    public GameObject galaxyView;
    public GameObject movementCostView;
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
        SetSelection(selectedPlanet, attackedPlanet);
    }

    void setPlanet(GameObject planetGO, Planet planet)
    {
        planetGO.GetComponentInChildren<Text>().text = planet.name;
        planetGO.GetComponentInChildren<Image>().sprite = null;
    }

    void SetSelection(Planet planet, Planet attacked)
    {
        MovementCost movementCost = movementCostView.GetComponent<MovementCost>();
        movementCost.SetSelection(planet, attacked);
    }

    void CloseView()
    {
        movementCostView.GetComponent<MovementCost>().CloseView();
        this.gameObject.SetActive(false);
        galaxyView.SetActive(true);
    }
}
