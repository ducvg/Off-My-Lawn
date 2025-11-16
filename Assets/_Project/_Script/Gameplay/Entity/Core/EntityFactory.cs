using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityFactory : Singleton<EntityFactory>
{
    private PoolFactory<Entity> entityPool = new();

    public Entity Spawn(EntityID Id, Vector3 position)
    {
        var config = GameDatabase.Instance.EntityDictionary[Id];
        if (config == null) return null;

        var entity = entityPool.Spawn(config.Prefab, position, transform);
        entity.Init(config);
        return entity;
    }

    public Entity SpawnRandomRow(EntityID Id)
    {
        var pos = new Vector3(
            GameConstant.GRID_BOUND_X_MAX + 0.5f + Random.Range(0, GameConstant.MONSTER_SPAWN_RANGE_X),
            GameConstant.LAWN_ELEVATION_Y,
            0.5f + GameGrid.Instance.GetRandomRowIndex()
        );
        Entity spawned = Spawn(Id, pos);
        spawned.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        spawned.ChangeState(new WalkState());

        return spawned;
    }

    public Entity SpawnPreviewMode(EntityID Id)
    {
        var pos = new Vector3(
            GameConstant.GRID_BOUND_X_MAX + Random.Range(0, GameConstant.MONSTER_SPAWN_RANGE_X),
            GameConstant.LAWN_ELEVATION_Y,
            Random.Range(GameConstant.GRID_BOUND_Y_MIN, GameConstant.GRID_BOUND_Y_MAX)
        );
        Entity spawned = Spawn(Id, pos);
        spawned.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        spawned.ChangeState(new PreviewState());

        return spawned;
    }

    public void Release(Entity entity)
    {
        entityPool.Release(entity.Config.Prefab, entity);
    }

    public void PreloadEntity(EntityID entityID, int count = 10)
    {
        var config = GameDatabase.Instance.EntityDictionary[entityID];
        if (config != null)
        {
            entityPool.Preload(config.Prefab, count);
        }
    }
}
