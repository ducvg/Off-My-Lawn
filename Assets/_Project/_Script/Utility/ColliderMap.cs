using System.Collections.Generic;
using UnityEngine;

public class ColliderMap<T> where T : MonoBehaviour
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
        if (!ColliderDic.ContainsKey(collider))
        {
            entity = null;
            return false;
        }
        return ColliderDic.TryGetValue(collider, out entity);
    }
}