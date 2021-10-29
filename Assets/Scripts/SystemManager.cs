using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public Material planetMaterial;
    public Material cloudMaterial;
    public Camera worldCamera;

    void Start()
    {
        // Create Planets
        Planet p1 = PlanetFactory.CreatePlanet("First Planet", planetMaterial, cloudMaterial, 300, PlanetFactory.PlanetType.Earth, new Vector3(20, 0, 0));
        Planet p2 = PlanetFactory.CreatePlanet("Second Planet", planetMaterial, cloudMaterial, 100, PlanetFactory.PlanetType.Cold, new Vector3(30, 0, 0));
        Planet p3 = PlanetFactory.CreatePlanet("Planet Enemy 1", planetMaterial, cloudMaterial, 50, PlanetFactory.PlanetType.Alien, new Vector3(40, 0, 0));
        Planet p4 = PlanetFactory.CreatePlanet("Planet Enemy 2", planetMaterial, cloudMaterial, 500, PlanetFactory.PlanetType.Desert, new Vector3(50, 0, 0)); ;

        // Create Players
        Player player = new Player("Player 1", new List<Planet>() { p1, p2 });
        Player enemy1 = new Player("Enemy 1", p3);
        Player enemy2 = new Player("Enemy 2", p4);

        // Create solar system
        SolarSystem solarSystem = new SolarSystem();
        solarSystem.AddPlanet(new List<Planet>() { p1, p2, p3, p4 });

        // Store info
        Store.SetStore(player, new List<Player>() { enemy1, enemy2 }, solarSystem, worldCamera);

        // UI
        Store.UpdateUI();
        InvokeRepeating("ProduceResources", 1, 1);
    }

    void ProduceResources()
    {
        Store.player.ProduceResourcesFromPlanets();
        Store.UpdateUI();
    }
}
