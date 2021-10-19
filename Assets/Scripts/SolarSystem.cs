using System.Collections;
using System.Collections.Generic;

public class SolarSystem
{
    private List<Planet> planets = new List<Planet>();

    public void AddPlanet(Planet p)
    {
        planets.Add(p);
    }

    public void AddPlanet(List<Planet> listP)
    {
        planets.AddRange(listP);
    }

    public List<Planet> GetPlanets()
    {
        return planets;
    }
}
