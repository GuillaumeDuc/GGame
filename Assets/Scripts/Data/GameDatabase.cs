using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "GGame/Game Database")]
public class GameDatabase : ScriptableObject
{
    private static GameDatabase instance;

    public static GameDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameDatabase has not been provided. Assign the GameDatabase asset to SystemManager's 'Game Database' field in the Inspector.");
            }
            return instance;
        }
    }

    // Called once at startup (SystemManager) with the Inspector-assigned asset reference,
    // instead of loading it implicitly from a Resources folder by magic string.
    public static void Provide(GameDatabase database)
    {
        instance = database;
    }

    [Header("Units")]
    public List<Ship> ships = new List<Ship>();
    public List<Troop> troops = new List<Troop>();
    public List<Defense> defenses = new List<Defense>();

    [Header("Fleet Visualization")]
    [SerializeField] private ComputeShader boidsComputeShader;

    [Header("Factories")]
    public List<FactoryDefinition> factoryDefinitions = new List<FactoryDefinition>();

    public ComputeShader BoidsComputeShader => boidsComputeShader;
    public List<Ship> getShips() => new List<Ship>(ships);
    public List<Troop> getTroops() => new List<Troop>(troops);
    public List<Defense> getDefenses() => new List<Defense>(defenses);

    public List<Factory> GetFactories() => factoryDefinitions.Select(def => new Factory(def)).ToList();
    public List<Factory> GetDefaultFactories() => GetFactories();
}
