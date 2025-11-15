using System.Collections.Generic;
using UnityEngine;

public class ColliderMap<T>
{
    private readonly Dictionary<Collider, T> ColliderDic;

    public ColliderMap()
    {
        ColliderDic = new();
    }

    public void Add(Collider collider, T entity)
    {
        ColliderDic[collider] = entity;
    }

    public void Remove(Collider collider)
    {
        ColliderDic.Remove(collider);
    }

    public bool TryGetEntity(Collider collider, out T entity)
    {
        return ColliderDic.TryGetValue(collider, out entity);
    }
}