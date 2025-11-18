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

    public void Preload(T prefab, int count)
    {
        if(!pools.TryGetValue(prefab, out UnityPool<T> pool))
        {
            AddPool(prefab);
            pool = pools[prefab];
        }
        
        for (int i = 0; i < count; ++i)
        {
            pool.Get();
        }
        pool.ReleaseAll();     
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
}

public class UnityPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Stack<T> inactiveStack;
    private readonly int maxSize;

    public UnityPool(T prefab, int defaultCapacity, int maxSize)
    {
        this.prefab = prefab;
        inactiveStack = new(defaultCapacity);
        this.maxSize = maxSize;
    }

    private T Create()
    {
        return Object.Instantiate(prefab);
    }

    public T Get()
    {
        T instance;
        
        if (inactiveStack.Count == 0) instance = Create();
        else instance = inactiveStack.Pop();

        instance.gameObject.SetActive(true);
        return instance;
    }

    public void Release(T instance)
    {
        if(inactiveStack.Count > maxSize)
        {
            Object.Destroy(instance);
            return;
        }
        instance.gameObject.SetActive(false);
        inactiveStack.Push(instance);
    }

    public void ReleaseAll()
    {
        while(inactiveStack.Count > 0)
        {
            Object.Destroy(inactiveStack.Pop());
        }
        inactiveStack.Clear();
    }
}