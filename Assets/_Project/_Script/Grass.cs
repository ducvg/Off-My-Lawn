using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;

[ExecuteAlways]
public class Grass : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;

    public Vector3 grassScale = Vector3.one;
    public Vector3 offsetPos = Vector3.zero;

    public ShadowCastingMode castShadows = ShadowCastingMode.Off;
    public bool receiveShadows = true;
    public Transform[] blocks;
    public int instanceCount = 100;

    private RenderParams rParams;
    private MaterialPropertyBlock MPB => rParams.matProps;
    private Bounds bounds => rParams.worldBounds;
    private List<Matrix4x4> matrices = new();

    void OnEnable()
    {
        Vector3 boundsSize = new Vector3(20, 1, 20);
        rParams = new RenderParams(Material)
        {
            worldBounds = new Bounds(transform.position + boundsSize * 0.5f, boundsSize),
            shadowCastingMode = castShadows,
            receiveShadows = receiveShadows,
            matProps = new MaterialPropertyBlock()
        };
        instanceCount = blocks.Length;

        SetupInstances();

    }

    void Update()
    {
        Graphics.RenderMeshPrimitives(rParams, Mesh, 0, instanceCount);

    }

    void OnDisable() {
        if (instancesBuffer != null) {
            instancesBuffer.Release();
            instancesBuffer = null;
        }
    }

    #region Instances
    [StructLayout(LayoutKind.Sequential)]
	private struct InstanceData {
		public Matrix4x4 matrix;
		//public Color color;

		public static int Size() {
			return
				sizeof(float) * 4 * 4 	// matrix
			//+ sizeof(float) * 4 		// color
			;
			// Alternatively one of these might work to calculate the size automatically?
            // return System.Runtime.InteropServices.Marshal.SizeOf(typeof(InstanceData));
            // return Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<InstanceData>();
		}
		/*
			Must match the layout/size of the struct in shader
			See https://docs.unity3d.com/ScriptReference/ComputeBufferType.Structured.html
			To avoid issues with how different graphics APIs structure data :
			- Order by largest to smallest 
			- Use Vector4/Color/float4 & Matrix4x4/float4x4 instead of float3 & float3x3
		*/
	}
	
	private ComputeBuffer instancesBuffer;

	private void SetupInstances(){
		if (instanceCount <= 0) {
			// Avoid negative or 0 instances, as that will crash Unity
            instanceCount = 1;
        }
		InstanceData[] instances = new InstanceData[instanceCount];
		Vector3 boundsSize = bounds.size;
		for (int i = 0; i < instanceCount; i++) {

            Vector3 pos = blocks[i].position + Vector3.up * 1.2f + offsetPos;

            instances[i] = new()
            {
                matrix = Matrix4x4.TRS(pos, Quaternion.identity, grassScale)
            };
		}
		instancesBuffer = new ComputeBuffer(instanceCount, InstanceData.Size());
		instancesBuffer.SetData(instances);
		MPB.SetBuffer("_PerInstanceData", instancesBuffer);
	}
#endregion 
}