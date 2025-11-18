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
    private LevelEditorWindow editWindow;

    [Button, Title(""), PropertySpace(SpaceBefore = 10)]
    public void OpenEditWindow()
    {
        editWindow = EditorWindow.GetWindow<LevelEditorWindow>();
        editWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(1500, 900);
        editWindow.SetLevelData(currentLevelData);
    }

    [ButtonGroup]
    void LoadLevel()
    {
        currentLevelData = Data.LoadLevel(levelIndex);
        LoadLevelHeroes(currentLevelData);
        if(editWindow) editWindow.SetLevelData(currentLevelData);
    }

    [ButtonGroup]
    void SaveLevel()
    {
        SaveLevelHeroConfigs();
        currentLevelData.LevelIndex = levelIndex;
        Data.SaveLevel(currentLevelData);
    }

    void LoadLevelHeroes(LevelData loadedData)
    {
        SelectableHeroes.Clear(); ForcedHeroes.Clear();
        foreach(var selectableId in loadedData.SelectableHeroes)
        {
            SelectableHeroes.Add(GameDatabase.Instance.EntityDictionary[selectableId]);
        }
        foreach(var forcedId in loadedData.ForcedHeroes)
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
