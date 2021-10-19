using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    private SolarSystem solarSystem;
    void Start()
    {

        // Create Planets
        Planet p1 = new Planet("Player's first Planet", 100);
        Planet p2 = new Planet("Planet 2", 20);
        Planet p3 = new Planet("Planet Enemy 1", 50);
        Planet p4 = new Planet("Planet Enemy 2", 500);

        // Create Players
        Player player = new Player("Player 1", new List<Planet>() { p1, p2 });
        Player enemy1 = new Player("Enemy 1", p3);
        Player enemy2 = new Player("Enemy 2", p4);

        // Store players info
        Store.player = player;

        // Create solar system
        solarSystem = new SolarSystem();
        solarSystem.AddPlanet(new List<Planet>() { p1, p2, p3, p4 });

        // UI
        Store.UpdateUI();
        InvokeRepeating("ProduceResources", 1, 1);
    }

    void ProduceResources()
    {
        Store.player.selectedPlanet.ProduceResources();
        Store.UpdateUI();
    }
}
