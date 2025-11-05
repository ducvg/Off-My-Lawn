using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZLinq;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private FloatValueSO crystalValue;
    [SerializeField] private LevelData levelData;
    private ColliderMap<Entity> entityColliderMap = new();

    void Start()
    {
        Init();
    }

    public void Init()
    {
        crystalValue.Value = levelData.StartCrystal;

        var gameplayCanvas = UIManager.Instance.Open<GameplayCanvas>();
        foreach (var config in levelData.ForcedEntities)
        {
            gameplayCanvas.AddCard(config);
        }

        PreloadEntities();
    }

    [Button]
    public void TestWave(int waveIndex)
    {
        var entities = GetWaveEntities(waveIndex);
        SpawnAtGraves(entities);
        SpawnRandomRows(entities);
    }

    private void SpawnAtGraves(List<EntityConfigSO> entities)
    {
        
    }
    private void SpawnRandomRows(List<EntityConfigSO> entities)
    {
        foreach (var entity in entities)
        {
            var pos = new Vector3(
                GameConstant.GRID_BOUND_X_MAX + 0.5f + Random.Range(0, GameConstant.MONSTER_SPAWN_RANGE_X),
                GameConstant.LAWN_ELEVATION_Y,
                0.5f + GameGrid.Instance.GetRandomRowIndex()
            );
            var m = EntityFactory.Instance.Spawn(entity.Id, pos);
            m.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            m.ChangeState(new WalkState());
        }
    }

    public List<EntityConfigSO> GetWaveEntities(int waveIndex)
    {
        var wave = levelData.Waves[waveIndex];
        var monsters = new List<EntityConfigSO>();

        foreach (var monster in wave.SpawnDatas) //spawn forced pick first
        {
            for (int i = 0; i < monster.ForcedPickCount; i++)
            {
                var config = EntityFactory.Instance.GetEntityConfig(monster.EntityID);
                monsters.Add(config);
                wave.WaveSpawnPoint -= config.SpawnCost;
            }
        }

        int safeLock = 10_000;
        var weights = wave.SpawnDatas.AsValueEnumerable()
            .Select(m => m.PickWeight)
            .ToList();
        while (wave.WaveSpawnPoint > 0 && --safeLock > 0)
        {
            int index = weights.GetRandomWeightedIndex();
            if (index == -1) continue; 
            if (wave.SpawnDatas[index].MaxInWave <= 0)
            {
                weights.RemoveAt(index);
                continue;
            }

            var config = EntityFactory.Instance.GetEntityConfig(wave.SpawnDatas[index].EntityID);
            monsters.Add(config);
            wave.WaveSpawnPoint -= config.SpawnCost;
            wave.SpawnDatas[index].MaxInWave--;
        }
        return monsters;
    }

    private void PreloadEntities() //create pool for each entity type in level
    {
        foreach (var wave in levelData.Waves)
        {
            foreach (var spawn in wave.SpawnDatas)
            {
                EntityFactory.Instance.PreloadEntity(spawn.EntityID);
            }
        }
    }

    public void RegisterEntityCollider(Collider collider, Entity entity)
    {
        entityColliderMap.Add(collider, entity);
    }
    public void UnregisterEntityCollider(Collider collider)
    {
        entityColliderMap.Remove(collider);
    }
    public bool TryGetEntityByCollider(Collider collider, out Entity entity)
    {
        return entityColliderMap.TryGetEntity(collider, out entity);
    }
}
