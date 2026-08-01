using UnityEngine;

[System.Serializable]
public class BoidSystems
{
    public BoidSystem boidSystem;
    public BoidsRenderer boidsRenderer;
}

public class BoidManager : MonoBehaviour
{
    [SerializeField] private BoidSystems[] boidSystems;

    private void Start()
    {
        // Initialize all boid systems
        if (boidSystems == null || boidSystems.Length == 0)
            return;

        for (int i = 0; i < boidSystems.Length; i++)
        {
            if (boidSystems[i].boidSystem != null)
            {
                boidSystems[i].boidSystem.Initialize(transform.position);
            }

            if (boidSystems[i].boidsRenderer != null && boidSystems[i].boidSystem != null)
            {
                boidSystems[i].boidsRenderer.Initialize(boidSystems[i].boidSystem.GetBoidsCount());
            }
        }
    }

    private void Update()
    {
        if (boidSystems == null || boidSystems.Length == 0)
            return;

        // Simulate all boid systems, keeping simulation bounds centered on this transform
        for (int i = 0; i < boidSystems.Length; i++)
        {
            if (boidSystems[i].boidSystem != null)
            {
                boidSystems[i].boidSystem.Simulate(Time.deltaTime, transform.position);
            }
        }
    }

    private void LateUpdate()
    {
        if (boidSystems == null || boidSystems.Length == 0)
            return;

        // Render all boid systems
        for (int i = 0; i < boidSystems.Length; i++)
        {
            if (boidSystems[i].boidsRenderer != null && boidSystems[i].boidSystem != null)
            {
                boidSystems[i].boidsRenderer.Render(boidSystems[i].boidSystem);
            }
        }
    }

    private void OnDestroy()
    {
        // Cleanup all boid systems
        if (boidSystems == null)
            return;

        for (int i = 0; i < boidSystems.Length; i++)
        {
            if (boidSystems[i].boidSystem != null)
            {
                boidSystems[i].boidSystem.Cleanup();
            }
        }
    }

    public int GetTotalBoidsCount()
    {
        int total = 0;
        if (boidSystems != null)
        {
            for (int i = 0; i < boidSystems.Length; i++)
            {
                if (boidSystems[i].boidSystem != null)
                    total += boidSystems[i].boidSystem.GetBoidsCount();
            }
        }
        return total;
    }

    public BoidSystem GetBoidSystem(int index)
    {
        if (boidSystems != null && index >= 0 && index < boidSystems.Length)
            return boidSystems[index].boidSystem;
        return null;
    }

    public BoidsRenderer GetBoidsRenderer(int index)
    {
        if (boidSystems != null && index >= 0 && index < boidSystems.Length)
            return boidSystems[index].boidsRenderer;
        return null;
    }

    public int GetBoidSystemCount()
    {
        return boidSystems != null ? boidSystems.Length : 0;
    }

    public BoidSystems[] GetAllBoidSystems()
    {
        return boidSystems;
    }

    private void OnDrawGizmosSelected()
    {
        if (boidSystems == null || boidSystems.Length == 0)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < boidSystems.Length; i++)
        {
            BoidSystem boidSystem = boidSystems[i].boidSystem;
            if (boidSystem == null)
                continue;

            Gizmos.DrawWireSphere(transform.position + boidSystem.simulationCenter, boidSystem.simulationRadius);
        }
    }
}

