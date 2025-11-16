using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class LevelEditor : MonoBehaviour
{
    [field: SerializeField] public List<EntityConfigSO> SelectableHeroes {get; private set;} = new();
    [field: SerializeField] public List<EntityConfigSO> ForcedHeroes {get; private set;} = new();
    [SerializeField] int levelIndex;
    [SerializeField, ReadOnly] LevelData currentLevelData;
    const string levelPath = "Assets/_Project/Resources/Levels";

    [Button, Title(""), PropertySpace(SpaceBefore = 10)]
    public void EditLevelSpawns()
    {
        var window = EditorWindow.GetWindow<LevelEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1500, 900);
        window.SetLevelData(currentLevelData);
        window.Show();
    }

    
    [ButtonGroup]
    void LoadLevel()
    {
        string fileName = $"Level_{levelIndex}.json";
        var path = Path.Combine(levelPath, fileName);
        string json = File.ReadAllText(path);
        currentLevelData = JsonUtility.FromJson<LevelData>(json);
        LoadLevelHeroes(currentLevelData);
        Debug.Log($"Loaded level {currentLevelData.LevelIndex} at: {path}");
    }


    [ButtonGroup]
    void SaveLevel()
    {
        string fileName = $"Level_{levelIndex}.json";
        var path = Path.Combine(levelPath, fileName);
        SaveLevelHeroConfigs();
        string json = JsonUtility.ToJson(currentLevelData);
        File.WriteAllText(path, json);
        AssetDatabase.ImportAsset(path);
        Debug.Log($"Saved Level {currentLevelData.LevelIndex} to: " + path);
    }

    void LoadLevelHeroes(LevelData loadedData)
    {
        SelectableHeroes.Clear(); ForcedHeroes.Clear();
        foreach(EntityID selectableId in loadedData.SelectableHeroes)
        {
            SelectableHeroes.Add(GameDatabase.Instance.EntityDictionary[selectableId]);
        }
        foreach(EntityID forcedId in loadedData.ForcedHeroes)
        {
            ForcedHeroes.Add(GameDatabase.Instance.EntityDictionary[forcedId]);
        }
    }

    void SaveLevelHeroConfigs()
    {
        currentLevelData.ForcedHeroes.Clear();
        currentLevelData.SelectableHeroes.Clear();
        foreach(var config in SelectableHeroes)
        {
            currentLevelData.SelectableHeroes.Add(config.Id);
        }
        foreach(var config in ForcedHeroes)
        {
            currentLevelData.ForcedHeroes.Add(config.Id);
        }
    }
}
