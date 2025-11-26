using System.Collections.Generic;
using UnityEngine;

public sealed class UnityPool<T> where T : Component
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

    public void Preload(int amount)
    {
        for(int i=0; i<amount; i++)
        {
            T instance = Create();
            instance.gameObject.SetActive(false);
            inactiveStack.Push(instance);
        }
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

    public void DestroyAll()
    {
        while(inactiveStack.Count > 0)
        {
            Object.Destroy(inactiveStack.Pop());
        }
        inactiveStack.Clear();
    }
}