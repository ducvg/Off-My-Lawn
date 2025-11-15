using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolFactory<T> where T : Component
{
    private Dictionary<T, ObjectPool<T>> pools = new();

    public void AddPool(T prefab, bool collectionCheck = false, int defaultCapacity = 10, int maxSize = 50)
    {
        if (pools.ContainsKey(prefab)) return;
        var pool = new ObjectPool<T>(
            () => OnCreate(prefab),
            OnGet,
            OnRelease,
            OnDestroy,
            collectionCheck,
            defaultCapacity,
            maxSize
        );
        pools[prefab] = pool;
    }

    public T Spawn(T prefab, Vector3 position, Transform parent = null)
    {
        if(!pools.TryGetValue(prefab, out var pool))
        {
            AddPool(prefab);
            pool = pools[prefab];
        }
        var instance = pool.Get();
        instance.transform.SetPositionAndRotation(position, Quaternion.identity);
        instance.transform.SetParent(parent);

        return instance;
    }

    public void Release(T prefab, T instance)
    {
        if (pools.TryGetValue(prefab, out var pool))
        {
            pool.Release(instance);
        }
        else
        {
            Object.Destroy(instance.gameObject);
        }
    }

    public void Preload(T prefab, int count)
    {
        if(!pools.TryGetValue(prefab, out ObjectPool<T> pool))
        {
            AddPool(prefab);
            pool = pools[prefab];
        }
        for (int i = 0; i < count; ++i)
        {
            var instance = pool.Get();
            pool.Release(instance);
        }     
    }

    #region Pool logic
    private T OnCreate(T prefab)
    {
        return Object.Instantiate(prefab);
    }

    private void OnGet(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    private void OnRelease(T instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnDestroy(T instance)
    {
        if (!instance) return;
        Object.Destroy(instance.gameObject);
    }

    #endregion
}