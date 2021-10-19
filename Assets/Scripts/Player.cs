using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<Planet> planets = new List<Planet>();
    public Planet selectedPlanet;
    public string name;
    public Player(string name, Planet planet = null)
    {
        this.name = name;
        selectedPlanet = planet;
        SetPlanet(planet);
    }

    public Player(string name, List<Planet> planets)
    {
        this.name = name;
        selectedPlanet = planets.Count > 0 ? planets[0] : null;
        SetPlanet(planets);
    }

    public void SetPlanet(Planet planet)
    {
        planet.owner = this;
        planets.Add(planet);
    }

    public void SetPlanet(List<Planet> planets)
    {
        planets.ForEach(planet =>
        {
            SetPlanet(planet);
        });
    }
}
