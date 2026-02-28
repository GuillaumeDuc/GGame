using UnityEngine;

public struct BoidData
{
    public Vector3 velocity;
    public Vector3 position;
}

public class BoidSystem : MonoBehaviour
{
    public ComputeShader boidsComputeShader;

    [Range(128, 60000)]
    [SerializeField] private int boidsCount = 5000;

    [Header("CAS Radius")]
    [SerializeField] private float cohesionRadius = 1.0f; // Radius for applying cohesion to other individuals
    [SerializeField] private float alignmentRadius = 1.0f; // Radius for applying alignment to other individuals
    [SerializeField] private float separationRadius = 0.5f; // Radius for applying separation to other individuals

    [Header("CAS Forces")]
    [SerializeField] private float cohesionWeight = 0.5f; // Cohesion force appliance weight
    [SerializeField] private float alignmentWeight = 0.5f; // Alignment force appliance weight
    [SerializeField] private float separationWeight = 2.0f; // Separation force appliance weight

    [Header("Boid")]
    [SerializeField] private float boidMaximumSpeed = 10.0f; // Boid maximum speed
    [SerializeField] private float boidMaxSteeringForce = 1.0f; // Boid maximum steering force

    [Header("Simulation")]
    [SerializeField] private Vector3 simulationCenter = Vector3.zero; // Simulation center
    [SerializeField] private Vector3 simulationDimensions = new Vector3(32.0f, 32.0f, 32.0f); // Simulation dimensions
    [SerializeField] private float simulationBoundsAvoidWeight = 10.0f; // Bounding avoidance weight

    private ComputeBuffer _boidsSteeringForcesBuffer; // Buffer for Boids steering forces values storage
    private ComputeBuffer _boidsDataBuffer; // Buffer storing basic data of Boids (velocity, position, Transform, etc.)


    private uint _storedThreadGroupSize; // Thread group size received from Compute Shader
    private int _dispatchedThreadGroupSize; // Thread group size calculated

    private int _steeringForcesKernelId; // Kernel for processing boids steering forces calculation
    private int _boidsDataKernelId; // Kernel for processing boids steering forces calculation

    private void Start()
    {
        InitBuffers();
        InitKernels();
    }

    private void InitBuffers()
    {
        _boidsDataBuffer = new ComputeBuffer(boidsCount, sizeof(float) * 6); // 6 for two Vector3
        _boidsSteeringForcesBuffer = new ComputeBuffer(boidsCount, sizeof(float) * 3); // 3 for one Vector3

        // Prepare data arrays
        Vector3[] forceArr = new Vector3[boidsCount];
        BoidData[] boidDataArr = new BoidData[boidsCount];

        Vector3 centerPos = GetSimulationCenter();
        for (var i = 0; i < boidsCount; i++)
        {
            forceArr[i] = Vector3.zero;
            boidDataArr[i].position = centerPos + Random.insideUnitSphere * 1.0f;
            boidDataArr[i].velocity = Random.insideUnitSphere * 0.1f;
        }

        // Set data to buffers
        _boidsSteeringForcesBuffer.SetData(forceArr);
        _boidsDataBuffer.SetData(boidDataArr);
    }

    private void InitKernels()
    {
        _steeringForcesKernelId = boidsComputeShader.FindKernel("SteeringForcesCS");
        _boidsDataKernelId = boidsComputeShader.FindKernel("BoidsDataCS");

        boidsComputeShader.GetKernelThreadGroupSizes(_steeringForcesKernelId, out _storedThreadGroupSize, out _, out _);

        // Calculate thread groups needed using ceiling division
        _dispatchedThreadGroupSize = (boidsCount + (int)_storedThreadGroupSize - 1) / (int)_storedThreadGroupSize;

        Debug.LogFormat("Thread group size: {0}", _storedThreadGroupSize);
        Debug.LogFormat("Total thread groups to dispatch: {0}", _dispatchedThreadGroupSize);
        Debug.LogFormat("Total threads: {0}", _dispatchedThreadGroupSize * _storedThreadGroupSize);
    }

    private void OnValidate()
    {
        // Update compute shader parameters in real-time when edited in Inspector
        if (!Application.isPlaying || boidsComputeShader == null) return;

        boidsComputeShader.SetFloat("_CohesionRadius", cohesionRadius);
        boidsComputeShader.SetFloat("_AlignmentRadius", alignmentRadius);
        boidsComputeShader.SetFloat("_SeparationRadius", separationRadius);
        boidsComputeShader.SetFloat("_BoidMaximumSpeed", boidMaximumSpeed);
        boidsComputeShader.SetFloat("_BoidMaximumSteeringForce", boidMaxSteeringForce);
        boidsComputeShader.SetFloat("_SeparationWeight", separationWeight);
        boidsComputeShader.SetFloat("_CohesionWeight", cohesionWeight);
        boidsComputeShader.SetFloat("_AlignmentWeight", alignmentWeight);
        boidsComputeShader.SetFloat("_SimulationBoundsAvoidWeight", simulationBoundsAvoidWeight);
        boidsComputeShader.SetVector("_SimulationCenter", GetSimulationCenter());
        boidsComputeShader.SetVector("_SimulationDimensions", simulationDimensions);
    }

    private void Update()
    {
        Simulation(_steeringForcesKernelId, _boidsDataKernelId);
    }
    private void Simulation(int steeringForcesKernelId, int boidsDataKernelId)
    {
        if (boidsComputeShader == null) return;

        boidsComputeShader.SetInt("_BoidsCount", boidsCount);

        boidsComputeShader.SetBuffer(steeringForcesKernelId, "_BoidsDataBuffer", _boidsDataBuffer);
        boidsComputeShader.SetBuffer(steeringForcesKernelId, "_BoidsSteeringForcesBufferRw", _boidsSteeringForcesBuffer);
        boidsComputeShader.SetBuffer(boidsDataKernelId, "_BoidsSteeringForcesBuffer", _boidsSteeringForcesBuffer);
        boidsComputeShader.SetBuffer(boidsDataKernelId, "_BoidsDataBufferRw", _boidsDataBuffer);

        boidsComputeShader.SetFloat("_CohesionRadius", cohesionRadius);
        boidsComputeShader.SetFloat("_AlignmentRadius", alignmentRadius);
        boidsComputeShader.SetFloat("_SeparationRadius", separationRadius);
        boidsComputeShader.SetFloat("_BoidMaximumSpeed", boidMaximumSpeed);
        boidsComputeShader.SetFloat("_BoidMaximumSteeringForce", boidMaxSteeringForce);
        boidsComputeShader.SetFloat("_SeparationWeight", separationWeight);
        boidsComputeShader.SetFloat("_CohesionWeight", cohesionWeight);
        boidsComputeShader.SetFloat("_AlignmentWeight", alignmentWeight);
        boidsComputeShader.SetFloat("_SimulationBoundsAvoidWeight", simulationBoundsAvoidWeight);

        boidsComputeShader.SetVector("_SimulationCenter", GetSimulationCenter());
        boidsComputeShader.SetVector("_SimulationDimensions", simulationDimensions);

        boidsComputeShader.SetFloat("_DeltaTime", Time.deltaTime);

        boidsComputeShader.Dispatch(steeringForcesKernelId, _dispatchedThreadGroupSize, 1, 1);
        boidsComputeShader.Dispatch(boidsDataKernelId, _dispatchedThreadGroupSize, 1, 1);
    }

    private void OnDestroy()
    {
        ReleaseBuffer();
    }
    private void ReleaseBuffer()
    {
        SafeReleaseBuffer(ref _boidsDataBuffer);
        SafeReleaseBuffer(ref _boidsSteeringForcesBuffer);
    }

    private void SafeReleaseBuffer(ref ComputeBuffer buffer)
    {
        if (buffer == null) return;
        buffer.Release();
        buffer = null;
    }

    public ComputeBuffer GetBoidsData()
    {
        return _boidsDataBuffer;
    }

    public int GetBoidsCount()
    {
        return boidsCount;
    }

    public Vector3 GetSimulationCenter()
    {
        return transform.position + simulationCenter;
    }

    public Vector3 GetSimulationDimensions()
    {
        return simulationDimensions;
    }
}
