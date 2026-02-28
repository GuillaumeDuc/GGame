using UnityEngine;

public class BoidsRenderer : MonoBehaviour
{
    public BoidSystem csBoids;
    [SerializeField] private Mesh instanceMesh;
    [SerializeField] private Material instanceRenderMaterial;
    [SerializeField] private Vector3 boidScale = new Vector3(0.2f, 0.3f, 0.6f);

    private int _boidsCount;
    private BoidData[] _boidData;
    private Matrix4x4[] _matrices;

    private void Start()
    {
        _boidsCount = csBoids.GetBoidsCount();
        _boidData = new BoidData[_boidsCount];
        _matrices = new Matrix4x4[_boidsCount];
    }

    private void Update()
    {
        if (instanceMesh == null || instanceRenderMaterial == null || csBoids == null)
            return;

        RenderBoids();
    }

    private void RenderBoids()
    {
        // Read boid data from GPU buffer
        csBoids.GetBoidsData().GetData(_boidData);

        // Build transformation matrices for all boids
        for (int i = 0; i < _boidsCount; i++)
        {
            Vector3 position = _boidData[i].position;
            Vector3 velocity = _boidData[i].velocity;

            // Create rotation from velocity
            Vector3 forward;
            if (velocity.magnitude < 0.001f)
                forward = Vector3.forward;
            else
                forward = velocity.normalized;

            Vector3 up = Mathf.Abs(forward.y) < 0.99f ? Vector3.up : Vector3.right;
            Vector3 right = Vector3.Cross(up, forward).normalized;
            up = Vector3.Cross(forward, right).normalized;

            // Build matrix
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetColumn(0, new Vector4(right.x * boidScale.x, right.y * boidScale.x, right.z * boidScale.x, 0));
            matrix.SetColumn(1, new Vector4(up.x * boidScale.y, up.y * boidScale.y, up.z * boidScale.y, 0));
            matrix.SetColumn(2, new Vector4(forward.x * boidScale.z, forward.y * boidScale.z, forward.z * boidScale.z, 0));
            matrix.SetColumn(3, new Vector4(position.x, position.y, position.z, 1));

            _matrices[i] = matrix;
        }

        // Draw all boids with computed matrices
        Graphics.DrawMeshInstanced(instanceMesh, 0, instanceRenderMaterial, _matrices, _boidsCount);
    }
}
