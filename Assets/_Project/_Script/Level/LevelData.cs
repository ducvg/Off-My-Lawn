using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int LevelIndex;
    public float StartCrystal = 100f;
    public List<EntityID> SelectableHeroes; //can add to deck
    public List<EntityID> ForcedHeroes; //auto in deck, cant remove
    public List<WaveData> Waves;
}