using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GameDatabase : GlobalConfig<GameDatabase>
{
    [field: SerializeField] public SerializedDictionary<EntityID, EntityConfigSO> EntityDictionary {get; private set;}


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
                EntityDictionary[config.Id] = config;
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
                EntityDictionary[config.Id] = config;
            }
        }
    }
    
#endif
}
