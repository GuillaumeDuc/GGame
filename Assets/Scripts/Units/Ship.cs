using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "GGame/Units/Ship")]
public class Ship : ScriptableObject, Unit
{
    [SerializeField] private int _hp;
    [SerializeField] private int _power;
    [SerializeField] private int _shield;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<Resource> _costToCreate = new List<Resource>();
    [SerializeField] private int _overlandSpeed;
    [SerializeField] private int _speed;
    [SerializeField] private Resource _travelCost;
    [Tooltip("Fleet prefab for this ship type. Its MeshFilter and Renderer provide the boid mesh and material.")]
    [SerializeField] private GameObject _fleetPrefab;
    [Tooltip("Mesh used to represent this ship type when instanced (e.g. fleet boid swarms).")]
    [SerializeField] private Mesh _instanceMesh;
    [Tooltip("Material used to render this ship type when instanced (e.g. fleet boid swarms). Needs GPU Instancing enabled.")]
    [SerializeField] private Material _instanceMaterial;
    [Tooltip("Scale applied to each instanced boid representing this ship type.")]
    [SerializeField] private Vector3 _boidScale = new(0.2f, 0.2f, 0.2f);

    public int hp => _hp;
    public int power => _power;
    public int shield => _shield;
    public Sprite sprite => _sprite;
    public ResourceCollection costToCreate => new ResourceCollection(_costToCreate);
    public int overlandSpeed => _overlandSpeed;
    public int speed => _speed;
    public Resource travelCost => _travelCost;
    public GameObject fleetPrefab => _fleetPrefab;
    public Mesh instanceMesh => _instanceMesh;
    public Material instanceMaterial => _instanceMaterial;
    public Vector3 boidScale => _boidScale;

    public string GetCost()
    {
        string s = "";
        foreach (var resource in costToCreate)
        {
            s += resource + "\n";
        }
        return s;
    }

    public Resource getTravelCost(float distance)
    {
        long costDistance = (long)distance / speed;
        Resource costResource = new Resource(travelCost);
        costResource.amount *= costDistance;
        return costResource;
    }

    public override string ToString()
    {
        return name;
    }
}
