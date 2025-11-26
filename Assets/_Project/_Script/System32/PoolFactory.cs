using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnityPoolFactory<T> where T : Component
{
    private Dictionary<T, UnityPool<T>> pools;

    public UnityPoolFactory()
    {
        pools = new();
    }

    private void AddPool(T prefab, int defaultCapacity = 100, int maxSize = 1000)
    {
        if (pools.ContainsKey(prefab)) return;
        var pool = new UnityPool<T>(prefab, defaultCapacity, maxSize);
        pools[prefab] = pool;
    }

    public void Preload(T prefab, int preloadAmount, int defaultCapacity = 100, int maxSize = 1000)
    {
        if(!pools.TryGetValue(prefab, out UnityPool<T> pool))
        {
            AddPool(prefab, defaultCapacity, maxSize);
            pool = pools[prefab];
        }

        pool.Preload(preloadAmount);
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