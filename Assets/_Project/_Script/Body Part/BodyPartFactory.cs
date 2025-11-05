using UnityEngine;

public class BodyPartFactory : Singleton<BodyPartFactory>
{
    [SerializeField] private FakeBodyPart prefab;
    PoolFactory<FakeBodyPart> poolFactory = new();

    private void Start()
    {
        poolFactory.AddPool(prefab, true, 10, 100);
        poolFactory.Preload(prefab, 10);
    }

    public FakeBodyPart Spawn(SkinnedMeshRenderer skinnedMesh, Vector3 position, Quaternion rotation)
    {
        var part = poolFactory.Spawn(prefab, position, transform);
        part.transform.rotation = rotation;
        part.Init(skinnedMesh);
        return part;
    }

    public void Release(FakeBodyPart part)
    {
        poolFactory.Release(prefab, part);
    }
}