using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Grass : MonoBehaviour
{
    public Mesh grassMesh;
    public Material grassMaterial;

    public Transform[] blocks;
    private List<Matrix4x4> matrices = new();

    public Quaternion grassRot = Quaternion.Euler(0, 0, 0);
    public Vector3 grassScale = Vector3.one;

    private ComputeBuffer argsBuffer;
    private ComputeBuffer matrixBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    struct GrassData
    {
        public Matrix4x4 matrix;
    }

    void Start()
    {
        int instanceCount = blocks.Length;
        GrassData[] grassData = new GrassData[instanceCount];

        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 pos = blocks[i].position + Vector3.up * 1.2f;
            grassData[i].matrix = Matrix4x4.TRS(pos, grassRot, grassScale);
        }

        // 1️⃣ Create buffer for instance data
        matrixBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf(typeof(GrassData)));
        matrixBuffer.SetData(grassData);
        grassMaterial.SetBuffer("_GrassBuffer", matrixBuffer);

        // 2️⃣ Setup indirect draw args
        args[0] = grassMesh.GetIndexCount(0);
        args[1] = (uint)instanceCount;
        args[2] = grassMesh.GetIndexStart(0);
        args[3] = grassMesh.GetBaseVertex(0);
        args[4] = 0;

        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);
    }

    void Update()
    {
        Graphics.DrawMeshInstancedIndirect(
            grassMesh,
            0,
            grassMaterial,
            new Bounds(Vector3.zero, new Vector3(1000, 1000, 1000)),
            argsBuffer
        );
    }
}
