using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityFactory : Singleton<EntityFactory>
{
    [SerializeField] private SerializedDictionary<EntityID, EntityConfigSO> entityDatabase;
    private PoolFactory<Entity> entityPool = new();

    public Entity Spawn(EntityID Id, Vector3 position)
    {
        var config = GetEntityConfig(Id);
        if (config == null) return null;

        var entity = entityPool.Spawn(config.Prefab, position, transform);
        entity.Init(config);
        return entity;
    }

    public void Release(Entity entity)
    {
        entityPool.Release(entity.Config.Prefab, entity);
    }

    public EntityConfigSO GetEntityConfig(EntityID entityID)
    {
        if (entityDatabase.TryGetValue(entityID, out var config))
        {
            return config;
        }
        Debug.LogError($"Entity Config for ID {entityID} not found!");
        return null;
    }   

    public void PreloadEntity(EntityID entityID, int count = 10)
    {
        var config = GetEntityConfig(entityID);
        if (config != null)
        {
            entityPool.Preload(config.Prefab, count);
        }
    }

#if UNITY_EDITOR
    const string MONSTERS_PATH = "Assets/_Project/DataObject/Entity Config/Monsters";
    const string HEROES_PATH = "Assets/_Project/DataObject/Entity Config/Heroes";

    [Button]
    void FetchMonsters()
    {
        var guids = UnityEditor.AssetDatabase.FindAssets("t:EntityConfigSO", new[] { MONSTERS_PATH });
        foreach (var guid in guids)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var config = UnityEditor.AssetDatabase.LoadAssetAtPath<EntityConfigSO>(path);
            if (config != null)
            {
                entityDatabase[config.Id] = config;
            }
        }
    }

    [Button]
    void FetchHeroes()
    {
        var heroes = UnityEditor.AssetDatabase.FindAssets("t:EntityConfigSO", new[] { HEROES_PATH });
        foreach (var guid in heroes)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var config = UnityEditor.AssetDatabase.LoadAssetAtPath<EntityConfigSO>(path);
            if (config != null)
            {
                entityDatabase[config.Id] = config;
            }
        }
    }
    
#endif
}
