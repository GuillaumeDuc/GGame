using UnityEngine;

public struct BoidData
{
    public Vector3 velocity;
    public Vector3 position;
}

[System.Serializable]
public class BoidSystem
{
    public ComputeShader boidsComputeShader;

    public int boidsCount = 5000;

    // CAS Radius
    public float cohesionRadius = 1.0f;
    public float alignmentRadius = 1.0f;
    public float separationRadius = 0.5f;

    // CAS Forces
    public float cohesionWeight = 0.5f;
    public float alignmentWeight = 0.5f;
    public float separationWeight = 2.0f;

    // Boid
    public float boidMaximumSpeed = 10.0f;
    public float boidMaxSteeringForce = 1.0f;

    // Simulation
    public Vector3 simulationCenter = Vector3.zero;
    public float simulationRadius = 32.0f;
    public float simulationBoundsAvoidWeight = 10.0f;

    private ComputeBuffer _boidsSteeringForcesBuffer;
    private ComputeBuffer _boidsDataBuffer;

    private uint _storedThreadGroupSize;
    private int _dispatchedThreadGroupSize;

    private int _steeringForcesKernelId;
    private int _boidsDataKernelId;
    private bool _isInitialized = false;

    public void Initialize()
    {
        if (_isInitialized) return;

        InitBuffers();
        InitKernels();
        _isInitialized = true;
    }

    private void InitBuffers()
    {
        _boidsDataBuffer = new ComputeBuffer(boidsCount, sizeof(float) * 6);
        _boidsSteeringForcesBuffer = new ComputeBuffer(boidsCount, sizeof(float) * 3);

        Vector3[] forceArr = new Vector3[boidsCount];
        BoidData[] boidDataArr = new BoidData[boidsCount];

        for (var i = 0; i < boidsCount; i++)
        {
            forceArr[i] = Vector3.zero;
            boidDataArr[i].position = simulationCenter + Random.insideUnitSphere * 1.0f;
            boidDataArr[i].velocity = Random.insideUnitSphere * 0.1f;
        }

        _boidsSteeringForcesBuffer.SetData(forceArr);
        _boidsDataBuffer.SetData(boidDataArr);
    }

    private void InitKernels()
    {
        _steeringForcesKernelId = boidsComputeShader.FindKernel("SteeringForcesCS");
        _boidsDataKernelId = boidsComputeShader.FindKernel("BoidsDataCS");

        boidsComputeShader.GetKernelThreadGroupSizes(_steeringForcesKernelId, out _storedThreadGroupSize, out _, out _);

        _dispatchedThreadGroupSize = (boidsCount + (int)_storedThreadGroupSize - 1) / (int)_storedThreadGroupSize;
    }

    public void UpdateShaderParameters()
    {
        if (boidsComputeShader == null) return;

        boidsComputeShader.SetFloat("_CohesionRadius", cohesionRadius);
        boidsComputeShader.SetFloat("_AlignmentRadius", alignmentRadius);
        boidsComputeShader.SetFloat("_SeparationRadius", separationRadius);
        boidsComputeShader.SetFloat("_BoidMaximumSpeed", boidMaximumSpeed);
        boidsComputeShader.SetFloat("_BoidMaximumSteeringForce", boidMaxSteeringForce);
        boidsComputeShader.SetFloat("_SeparationWeight", separationWeight);
        boidsComputeShader.SetFloat("_CohesionWeight", cohesionWeight);
        boidsComputeShader.SetFloat("_AlignmentWeight", alignmentWeight);
        boidsComputeShader.SetFloat("_SimulationBoundsAvoidWeight", simulationBoundsAvoidWeight);
        boidsComputeShader.SetVector("_SimulationCenter", simulationCenter);
        boidsComputeShader.SetFloat("_SimulationRadius", simulationRadius);
    }

    public void Simulate(float deltaTime)
    {
        if (boidsComputeShader == null || !_isInitialized) return;

        boidsComputeShader.SetInt("_BoidsCount", boidsCount);

        boidsComputeShader.SetBuffer(_steeringForcesKernelId, "_BoidsDataBuffer", _boidsDataBuffer);
        boidsComputeShader.SetBuffer(_steeringForcesKernelId, "_BoidsSteeringForcesBufferRw", _boidsSteeringForcesBuffer);
        boidsComputeShader.SetBuffer(_boidsDataKernelId, "_BoidsSteeringForcesBuffer", _boidsSteeringForcesBuffer);
        boidsComputeShader.SetBuffer(_boidsDataKernelId, "_BoidsDataBufferRw", _boidsDataBuffer);

        UpdateShaderParameters();

        boidsComputeShader.SetFloat("_DeltaTime", deltaTime);

        boidsComputeShader.Dispatch(_steeringForcesKernelId, _dispatchedThreadGroupSize, 1, 1);
        boidsComputeShader.Dispatch(_boidsDataKernelId, _dispatchedThreadGroupSize, 1, 1);
    }

    public void Cleanup()
    {
        SafeReleaseBuffer(ref _boidsDataBuffer);
        SafeReleaseBuffer(ref _boidsSteeringForcesBuffer);
        _isInitialized = false;
    }

    private void SafeReleaseBuffer(ref ComputeBuffer buffer)
    {
        if (buffer == null) return;
        buffer.Release();
        buffer = null;
    }

    public void UpdateBoidsCenter(Vector3 newCenter)
    {
        if (!_isInitialized || _boidsDataBuffer == null)
            return;

        simulationCenter = newCenter;
    }

    public ComputeBuffer GetBoidsData()
    {
        return _boidsDataBuffer;
    }

    public int GetBoidsCount()
    {
        return boidsCount;
    }
}

