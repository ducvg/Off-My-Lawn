using UnityEngine;

public class DetachObjectFactory : Singleton<DetachObjectFactory>
{
    [SerializeField] private DetachObject prefab;
    PoolFactory<DetachObject> poolFactory = new();

    private void Start()
    {
        poolFactory.AddPool(prefab, true, 10, 100);
        poolFactory.Preload(prefab, 10);
    }

    public DetachObject Spawn(SkinnedMeshRenderer skinnedMesh, Vector3 position, Quaternion rotation)
    {
        var part = poolFactory.Spawn(prefab, position, transform);
        part.transform.rotation = rotation;
        part.Init(skinnedMesh.sharedMesh, skinnedMesh.sharedMaterial);
        return part;
    }

    public DetachObject Spawn(Mesh mesh, Material material, Vector3 position, Quaternion rotation)
    {
        var part = poolFactory.Spawn(prefab, position, transform);
        part.transform.rotation = rotation;
        part.Init(mesh, material);
        return part;
    }

    public void Release(DetachObject part)
    {
        poolFactory.Release(prefab, part);
    }
}