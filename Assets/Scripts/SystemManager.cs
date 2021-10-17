using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    private SolarSystem solarSystem;
    void Start()
    {
        solarSystem = new SolarSystem();
        // Store selectedPlanet
        Store.selectedPlanet = new Planet("Your Planet", 100);
        solarSystem.AddPlanet(Store.selectedPlanet);
        ProduceResources();
        InvokeRepeating("ProduceResources", 2.0f, .5f);
    }

    void ProduceResources()
    {
        Store.selectedPlanet.ProduceResources();
        Store.UpdateUI();
    }
}
