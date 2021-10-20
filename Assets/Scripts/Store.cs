using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Store
{
    public static Player player;
    public static List<Player> enemies = new List<Player>();
    public static SolarSystem solarSystem;
    public static System.Action UpdateUI;

    public static void SetStore(Player player, List<Player> enemies, SolarSystem solarSystem)
    {
        Store.player = player;
        Store.enemies = enemies;
        Store.solarSystem = solarSystem;
    }
}
