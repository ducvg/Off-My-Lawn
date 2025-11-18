using System;
using System.Collections.Generic;

[Serializable]
public class WaveData
{
    public bool IsFlag = false;
    public float WaveTime = 15;
    public int WaveSpawnPoint = 10;
    public List<SpawnData> SpawnDataList = new();
}

[Serializable]
public class SpawnData
{
    public EntityID EntityID = EntityID.Skeleton;
    public int PickWeight = 1;
    public int MinSpawn;
    public int MaxSpawn = 999;
}