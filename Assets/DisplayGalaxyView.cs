using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisplayGalaxyView : DisplayUI
{
    public GameObject content;
    public GameObject planetOverviewPrefab;
    private List<Planet> listPlanet;

    public override void Display(Planet selectedPlanet)
    {
        // If no list or new list create new content else update content
        if (listPlanet == null || listPlanet.Count != Store.solarSystem.GetPlanets().Count)
        {
            if (listPlanet != null)
            {
                ResetContent();
            }
            Store.solarSystem.GetPlanets().ForEach(planet =>
            {
                if (!planet.Equals(selectedPlanet))
                {
                    GameObject planetOverview = Instantiate(planetOverviewPrefab);
                    planetOverview.transform.SetParent(content.transform);
                    SetPlanetOverview(selectedPlanet, planet, planetOverview);
                }
            });
        }
        else
        {
            int planetNB = 0;
            Store.solarSystem.GetPlanets().ForEach(planet =>
            {
                if (!planet.Equals(selectedPlanet))
                {
                    PlanetOverview planetOverview = content.GetComponentsInChildren<PlanetOverview>()[planetNB];
                    SetPlanetOverview(selectedPlanet, planet, planetOverview.gameObject);
                    planetNB++;
                }
            });
        }
        listPlanet = Store.solarSystem.GetPlanets();
    }

    void SetPlanetOverview(Planet selectedPlanet, Planet planet, GameObject planetOverviewGO)
    {
        PlanetOverview planetOverview = planetOverviewGO.GetComponent<PlanetOverview>();
        planetOverview.SetName(planet.name);
        planetOverview.SetSprite(null);
        planetOverview.SetDistance(Vector3.Distance(selectedPlanet.planetGO.transform.position, planet.planetGO.transform.position));
        // Hide attack button if own planet
        if (Store.player.planets.Contains(planet))
        {
            planetOverview.attackButton.gameObject.SetActive(false);
        }
        else
        {
            planetOverview.SetAttackButton(AttackAction);
        }
        planetOverview.SetTransportButton(TransportAction);
        planetOverview.SetStayButton(StayAction);
    }

    void AttackAction()
    {

    }

    void TransportAction()
    {

    }

    void StayAction()
    {

    }

    private void ResetContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
