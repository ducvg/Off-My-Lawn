using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int LevelIndex;
    public float StartCrystal = 100f;
    public List<EntityConfigSO> PlayableEntities; //can choose to add to deck
    public List<EntityConfigSO> ForcedEntities; //auto start with these hero cards
    public List<WaveData> Waves;
}
