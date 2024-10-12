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
        Planet sun = PlanetFactory.CreatePlanet("Sun", 600, PlanetType.Sunlike, (int)SunType.Yellow);
        Planet p1 = PlanetFactory.CreatePlanet("First Planet", 300, PlanetType.Terrestrial, (int)TerrestrialType.Temperate, new Vector3(20, 0, 0));
        Planet p2 = PlanetFactory.CreatePlanet("Second Planet", 500, PlanetType.GasGiant, (int)GasType.Hot, new Vector3(30, 0, 0));
        Planet p3 = PlanetFactory.CreatePlanet("Planet Enemy 1", 50, PlanetType.Terrestrial, (int)TerrestrialType.Ocean, new Vector3(40, 0, 0));
        Planet p4 = PlanetFactory.CreatePlanet("Planet Enemy 2", 60, PlanetType.Sunlike, (int)TerrestrialType.Cold, new Vector3(50, 0, 0));

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
