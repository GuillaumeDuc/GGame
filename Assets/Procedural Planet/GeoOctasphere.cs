using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Rendering;

public struct GeoOctasphere
{
    public struct Vertex
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }

    struct Rhombus
    {
        public int id;
        public float3 leftCorner, rightCorner;
    }

    public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));
    public int VertexCount => 4 * Resolution * Resolution + 2 * Resolution + 7;
    public int IndexCount => 6 * 4 * Resolution * Resolution;
    public int JobLength => 4 * Resolution + 1;
    public int Resolution { get; set; }
    public int Radius;
    public float3 NoiseOffset;

    [NativeDisableContainerSafetyRestriction]
    NativeArray<Stream0> stream0;

    [NativeDisableContainerSafetyRestriction]
    NativeArray<TriangleUInt16> triangles;

    public void Execute(int i)
    {
        if (i == 0)
        {
            ExecutePolesAndSeam();
        }
        else
        {
            ExecuteRegular(i - 1);
        }
    }

    public void ExecuteRegular(int i)
    {
        int u = i / 4;
        Rhombus rhombus = GetRhombus(i - 4 * u);
        int vi = Resolution * (Resolution * rhombus.id + u + 2) + 7;
        int ti = 2 * Resolution * (Resolution * rhombus.id + u);
        bool firstColumn = u == 0;

        int4 quad = int4(
            vi,
            firstColumn ? rhombus.id : vi - Resolution,
            firstColumn ?
                rhombus.id == 0 ? 8 : vi - Resolution * (Resolution + u) :
                vi - Resolution + 1,
            vi + 1
        );

        u += 1;

        var vertex = new Vertex();
        sincos(PI + PI * u / (2 * Resolution), out float sine, out vertex.position.y);
        vertex.position -= sine * rhombus.rightCorner;
        vertex.normal = vertex.position;

        vertex.tangent.xz = GetTangentXZ(vertex.position);
        vertex.tangent.w = -1f;
        vertex.texCoord0.x = rhombus.id * 0.25f + 0.25f;
        vertex.texCoord0.y = (float)u / (2 * Resolution);
        SetVertex(vi, vertex);
        vi += 1;

        for (int v = 1; v < Resolution; v++, vi++, ti += 2)
        {
            float h = u + v;
            float3 pRight = 0f;
            sincos(PI + PI * h / (2 * Resolution), out sine, out pRight.y);
            float3 pLeft = pRight - sine * rhombus.leftCorner;
            pRight -= sine * rhombus.rightCorner;

            float3 axis = normalize(cross(pRight, pLeft));
            float angle = acos(dot(pRight, pLeft)) * (
                v <= Resolution - u ? v / h : (Resolution - u) / (2f * Resolution - h)
            );
            vertex.normal = vertex.position = mul(
                quaternion.AxisAngle(axis, angle), pRight
            );
            vertex.tangent.xz = GetTangentXZ(vertex.position);
            vertex.texCoord0 = GetTexCoord(vertex.position);
            SetVertex(vi, vertex);
            SetTriangle(ti + 0, quad.xyz);
            SetTriangle(ti + 1, quad.xzw);

            quad.y = quad.z;
            quad += int4(1, 0, firstColumn && rhombus.id != 0 ? Resolution : 1, 1);
        }

        quad.z = Resolution * Resolution * rhombus.id + Resolution + u + 6;
        quad.w = u < Resolution ? quad.z + 1 : rhombus.id + 4;

        SetTriangle(ti + 0, quad.xyz);
        SetTriangle(ti + 1, quad.xzw);
    }

    public void ExecutePolesAndSeam()
    {
        var vertex = new Vertex();
        vertex.tangent = float4(sqrt(0.5f), 0f, sqrt(0.5f), -1f);
        vertex.texCoord0.x = 0.125f;

        for (int i = 0; i < 4; i++)
        {
            vertex.position = vertex.normal = down();
            vertex.texCoord0.y = 0f;
            SetVertex(i, vertex);
            vertex.position = vertex.normal = up();
            vertex.texCoord0.y = 1f;
            SetVertex(i + 4, vertex);
            vertex.tangent.xz = float2(-vertex.tangent.z, vertex.tangent.x);
            vertex.texCoord0.x += 0.25f;
        }

        vertex.tangent.xz = float2(1f, 0f);
        vertex.texCoord0.x = 0f;

        for (int v = 1; v < 2 * Resolution; v++)
        {
            sincos(
                PI + PI * v / (2 * Resolution),
                out vertex.position.z, out vertex.position.y
            );
            vertex.normal = vertex.position;
            vertex.texCoord0.y = (float)v / (2 * Resolution);
            SetVertex(v + 7, vertex);
        }
    }

    static Rhombus GetRhombus(int id) => id switch
    {
        0 => new Rhombus
        {
            id = id,
            leftCorner = back(),
            rightCorner = right()
        },
        1 => new Rhombus
        {
            id = id,
            leftCorner = right(),
            rightCorner = forward()
        },
        2 => new Rhombus
        {
            id = id,
            leftCorner = forward(),
            rightCorner = left()
        },
        _ => new Rhombus
        {
            id = id,
            leftCorner = left(),
            rightCorner = back()
        }
    };

    static float2 GetTangentXZ(float3 p) => normalize(float2(-p.z, p.x));

    static float2 GetTexCoord(float3 p)
    {
        var texCoord = float2(
            atan2(p.x, p.z) / (-2f * PI) + 0.5f,
            asin(p.y) / PI + 0.5f
        );
        if (texCoord.x < 1e-6f)
        {
            texCoord.x = 1f;
        }
        return texCoord;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct Stream0
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUInt16
    {

        public ushort a, b, c;

        public static implicit operator TriangleUInt16(int3 t) => new TriangleUInt16
        {
            a = (ushort)t.x,
            b = (ushort)t.y,
            c = (ushort)t.z
        };
    }

    public void Setup(Mesh.MeshData meshData)
    {
        var descriptor = new NativeArray<VertexAttributeDescriptor>(
            4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
        );
        descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
        descriptor[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension: 3
        );
        descriptor[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4
        );
        descriptor[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension: 2
        );
        meshData.SetVertexBufferParams(VertexCount, descriptor);
        descriptor.Dispose();

        meshData.SetIndexBufferParams(IndexCount, IndexFormat.UInt16);

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(
            0, new SubMeshDescriptor(0, IndexCount)
            {
                bounds = Bounds,
                vertexCount = VertexCount
            },
            MeshUpdateFlags.DontRecalculateBounds |
            MeshUpdateFlags.DontValidateIndices
        );

        stream0 = meshData.GetVertexData<Stream0>();
        triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);
    }

    public void SetVertex(int index, Vertex v)
    {
        // float3 n = noise.cnoise(v.position + NoiseOffset);
        // v.position += n * v.normal;

        stream0[index] = new Stream0
        {
            position = v.position * Radius,
            normal = v.normal,
            tangent = v.tangent,
            texCoord0 = v.texCoord0
        };
    }

    public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;
}