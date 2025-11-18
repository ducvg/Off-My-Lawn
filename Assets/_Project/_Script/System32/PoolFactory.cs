using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolFactory<T> where T : Component
{
    private Dictionary<T, UnityPool<T>> pools;

    public PoolFactory()
    {
        pools = new();
    }

    private void AddPool(T prefab, int defaultCapacity = 10, int maxSize = 1000)
    {
        if (pools.ContainsKey(prefab)) return;
        var pool = new UnityPool<T>(prefab, defaultCapacity, maxSize);
        pools[prefab] = pool;
    }

    public void Preload(T prefab, int defaultCapacity = 10, int maxSize = 1000)
    {
        if(!pools.TryGetValue(prefab, out UnityPool<T> pool))
        {
            AddPool(prefab, defaultCapacity, maxSize);
            pool = pools[prefab];
        }

        Span<T> buffer = new T[defaultCapacity];
        for (int i = 0; i < defaultCapacity; ++i)
           buffer[i] = pool.Get();
        for (int i = 0; i < defaultCapacity; ++i)
           pool.Release(buffer[i]);
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
            UnityEngine.Object.Destroy(instance.gameObject);
        }
    }
}