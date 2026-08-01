using System.Collections.Generic;
using UnityEngine;

// Attach this component directly to the planet prefab/GameObject (Resources/Planet/Planet).
// Each planet instance manages only its own fleet - no cross-planet manager/dictionary needed.
public class PlanetFleetBoids : MonoBehaviour
{
    [Tooltip("Compute shader used to simulate this planet's fleet swarms (assign Boids.compute).")]
    [SerializeField] private ComputeShader boidsComputeShader;
    [Tooltip("Fallback mesh used for ship types that don't define their own instanceMesh.")]
    [SerializeField] private Mesh fallbackInstanceMesh;
    [Tooltip("Fallback material used for ship types that don't define their own instanceMaterial. Needs GPU Instancing enabled.")]
    [SerializeField] private Material fallbackInstanceMaterial;

    [Header("Boid Behavior")]
    [SerializeField] private float cohesionRadius = 1.0f;
    [SerializeField] private float alignmentRadius = 0.6f;
    [SerializeField] private float separationRadius = 0.5f;
    [SerializeField] private float cohesionWeight = 0.5f;
    [SerializeField] private float alignmentWeight = 0.5f;
    [SerializeField] private float separationWeight = 2.0f;
    [SerializeField] private float boidMaximumSpeed = 1.0f;
    [SerializeField] private float boidMaxSteeringForce = 0.01f;
    [SerializeField] private float simulationBoundsAvoidWeight = 1.0f;

    private class ShipSwarm
    {
        public BoidSystem boidSystem;
        public BoidsRenderer boidsRenderer;
        public int lastCount;
    }

    private Planet _planet;
    private readonly Dictionary<Ship, ShipSwarm> _swarms = new();
    private readonly List<Ship> _staleShips = new();

    // Called once by Planet's constructor after this planet GameObject is created.
    public void Initialize(Planet planet, ComputeShader computeShader)
    {
        _planet = planet;
        boidsComputeShader = computeShader;
        if (boidsComputeShader == null)
        {
            Debug.LogError($"Planet '{name}' cannot render fleet boids: assign Boids.compute to GameDatabase.", this);
        }
    }

    private void Update()
    {
        if (_planet == null || boidsComputeShader == null)
            return;

        RefreshFleet();

        foreach (ShipSwarm swarm in _swarms.Values)
        {
            swarm.boidSystem.Simulate(Time.deltaTime, transform.position);
        }
    }

    private void LateUpdate()
    {
        foreach (ShipSwarm swarm in _swarms.Values)
        {
            swarm.boidsRenderer.Render(swarm.boidSystem);
        }
    }

    // (Re)builds swarms for this planet's current fleet, one per ship type stationed here.
    // Only tears down/rebuilds a ship type's GPU buffers when its count actually changed.
    // Safe to call every frame (already done in Update()) or on-demand right after a
    // purchase for immediate feedback.
    public void RefreshFleet()
    {
        if (_planet == null || boidsComputeShader == null)
            return;

        Dictionary<Ship, int> ships = _planet.GetShips();

        _staleShips.Clear();
        foreach (Ship ship in _swarms.Keys)
        {
            if (!ships.ContainsKey(ship))
                _staleShips.Add(ship);
        }
        for (int i = 0; i < _staleShips.Count; i++)
        {
            _swarms[_staleShips[i]].boidSystem.Cleanup();
            _swarms.Remove(_staleShips[i]);
        }

        foreach (KeyValuePair<Ship, int> shipEntry in ships)
        {
            Ship ship = shipEntry.Key;
            int count = shipEntry.Value;
            if (ship == null || count <= 0)
                continue;

            _swarms.TryGetValue(ship, out ShipSwarm existing);
            if (existing != null && existing.lastCount == count)
                continue; // Unchanged, keep simulating the existing swarm

            Mesh mesh = ship.instanceMesh;
            Material material = ship.instanceMaterial;
            if ((mesh == null || material == null) && ship.fleetPrefab != null)
            {
                MeshFilter meshFilter = ship.fleetPrefab.GetComponentInChildren<MeshFilter>();
                Renderer meshRenderer = ship.fleetPrefab.GetComponentInChildren<Renderer>();
                mesh ??= meshFilter != null ? meshFilter.sharedMesh : null;
                material ??= meshRenderer != null ? meshRenderer.sharedMaterial : null;
            }
            mesh ??= fallbackInstanceMesh;
            material ??= fallbackInstanceMaterial;
            if (mesh == null || material == null)
            {
                Debug.LogError($"Ship '{ship.name}' cannot render fleet boids: assign a Fleet Prefab or both Instance Mesh and Instance Material on the Ship asset.", this);
                continue; // Nothing to render this ship type with yet
            }

            existing?.boidSystem.Cleanup();

            BoidSystem boidSystem = new()
            {
                boidsComputeShader = boidsComputeShader,
                boidsCount = count,
                cohesionRadius = cohesionRadius,
                alignmentRadius = alignmentRadius,
                separationRadius = separationRadius,
                cohesionWeight = cohesionWeight,
                alignmentWeight = alignmentWeight,
                separationWeight = separationWeight,
                boidMaximumSpeed = boidMaximumSpeed,
                boidMaxSteeringForce = boidMaxSteeringForce,
                simulationCenter = Vector3.zero,
                simulationRadius = 2f * _planet.size / 100f,
                simulationBoundsAvoidWeight = simulationBoundsAvoidWeight
            };
            BoidsRenderer boidsRenderer = new()
            {
                instanceMesh = mesh,
                instanceRenderMaterial = material,
                boidScale = ship.boidScale
            };

            boidSystem.Initialize(transform.position);
            boidsRenderer.Initialize(boidSystem.GetBoidsCount());

            _swarms[ship] = new ShipSwarm
            {
                boidSystem = boidSystem,
                boidsRenderer = boidsRenderer,
                lastCount = count
            };
        }
    }

    private void OnDestroy()
    {
        foreach (ShipSwarm swarm in _swarms.Values)
        {
            swarm.boidSystem.Cleanup();
        }
        _swarms.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (_planet == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 2f * _planet.size / 100f);
    }
}
