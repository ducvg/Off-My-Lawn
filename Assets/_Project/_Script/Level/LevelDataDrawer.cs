using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class LevelDataDrawer : Attribute{}

public sealed class LevelDataDrawerAttribute : OdinAttributeDrawer<LevelDataDrawer, LevelData>
{
    LevelData LevelData;
    int removeWaveIndex, addWaveIndex;
    (int oldIndex, int newIndex) moveWaveElement;
    (WaveData wave, int dataIndex) removeSpawnData;

    private void SetupValues()
    {
        addWaveIndex = removeWaveIndex = -1; 
        moveWaveElement = (-1,-1);
        removeSpawnData = (null, -1);
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        LevelData = ValueEntry.SmartValue;
        if (LevelData == null) return;

        SetupValues();
        EditorGUILayout.Space(10);
        DrawLevelGeneralInfo(LevelData);
        EditorGUILayout.Space(5);

        if (LevelData.Waves == null) LevelData.Waves = new();

        int count = LevelData.Waves.Count;

        for (int i = 0; i < count; i++)
        {
            var wave = LevelData.Waves[i];
            SirenixEditorGUI.HorizontalLineSeparator(2);
            EditorGUILayout.BeginHorizontal();
            DrawWaveHeader(wave);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            {
                SirenixEditorGUI.BeginVerticalList(drawBorder: true, drawDarkBg: false, GUILayout.Width(100));
                DrawWaveGeneralInfo(wave); 
                SirenixEditorGUI.EndVerticalList();

                SirenixEditorGUI.BeginVerticalList(drawBorder: true, drawDarkBg: false, GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal();
                DrawWaveSpawns(wave);
                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndVerticalList();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.Space(5);

        CheckModification();
    }
    
    void DrawLevelGeneralInfo(LevelData levelData)
    {
        levelData.LevelIndex = SirenixEditorFields.IntField("Level Index", levelData.LevelIndex);
        levelData.StartCrystal = SirenixEditorFields.FloatField("Start Crystal", levelData.StartCrystal);
        //List<Config>
    }

    private void DrawWaveHeader(WaveData waveData)
    {
        int waveIndex = LevelData.Waves.IndexOf(waveData);
        GUILayout.Label($"Wave {waveIndex + 1}", EditorStyles.boldLabel);
        if(SirenixEditorGUI.IconButton(EditorIcons.ArrowUp))
        {
            if(waveIndex > 1) moveWaveElement = (waveIndex, waveIndex-1);
        }
        if(SirenixEditorGUI.IconButton(EditorIcons.ArrowDown))
        {
            if(waveIndex < LevelData.Waves.Count - 1) moveWaveElement = (waveIndex, waveIndex+1);
        }
        if(SirenixEditorGUI.IconButton(EditorIcons.Plus))
        {
            addWaveIndex = waveIndex;
        }
        if(SirenixEditorGUI.IconButton(EditorIcons.Minus))
        {
            removeWaveIndex = waveIndex;
        }
        
    }

    void DrawWaveGeneralInfo(WaveData wave)
    {
        var prevLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50;
        wave.IsFlag = EditorGUILayout.Toggle("Is Flag", wave.IsFlag );
        wave.WaveTime = SirenixEditorFields.FloatField("Duration", wave.WaveTime);
        wave.WaveSpawnPoint = SirenixEditorFields.IntField("Costs", wave.WaveSpawnPoint);
        EditorGUIUtility.labelWidth = prevLabelWidth;
        EditorGUILayout.Space(20);
        if (GUI.Button(GUILayoutUtility.GetRect(0, 30),"Add Spawn")) wave.MonsterSpawnData.Add(new SpawnData());
    }

    void DrawWaveSpawns(WaveData wave)
    {
        var prevLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50;
        var spawnCount = wave.MonsterSpawnData.Count;
        for(int i=0; i<spawnCount; i++)
        {
            var spawnData = wave.MonsterSpawnData[i];
            SirenixEditorGUI.BeginVerticalList(drawBorder: true, drawDarkBg: false, GUILayout.Width(200));
            {
                DrawSpawnData(spawnData);
                if (GUI.Button(GUILayoutUtility.GetRect(0, 20), "Remove Spawn"))
                {
                    removeSpawnData = (wave, i);
                }
            }
            SirenixEditorGUI.EndVerticalList();
        }
        EditorGUIUtility.labelWidth = prevLabelWidth;   
    }

    void DrawSpawnData(SpawnData spawnData)
    {
        var config = GameDatabase.Instance.EntityDictionary[spawnData.EntityID];
        config = SirenixEditorFields.UnityObjectField(
            label: GUIContent.none, allowSceneObjects: false,
            value: config, objectType: typeof(EntityConfigSO)
        ) as EntityConfigSO;
        spawnData.EntityID = config.Id; 
        var iconRect = GUILayoutUtility.GetRect(100, 100); // size in GUI
        SirenixEditorFields.PreviewObjectField(iconRect, config.Icon, 
            dragOnly: false, allowMove: false, allowSwap: false, allowSceneObjects: false
        );
        var costStyle = new GUIStyle(EditorStyles.boldLabel){alignment = TextAnchor.MiddleCenter};
        GUILayout.Label($"Spawn Cost: {config.SpawnCost}", costStyle);
        spawnData.PickWeight = SirenixEditorFields.IntField("Weight", spawnData.PickWeight);

        EditorGUILayout.BeginHorizontal();
        {
            GUIContent spawnLabel = new("Spawn");
            var rect = EditorGUILayout.GetControlRect(); rect = EditorGUI.PrefixLabel(rect, spawnLabel);
            var prev = EditorGUIUtility.labelWidth; EditorGUIUtility.labelWidth = 25;
            spawnData.MinSpawn = EditorGUI.IntField(rect.AlignLeft(rect.width * 0.5f), new GUIContent("min"), spawnData.MinSpawn);
            spawnData.MaxSpawn = EditorGUI.IntField(rect.AlignRight(rect.width * 0.5f), new GUIContent("max"), spawnData.MaxSpawn);
            EditorGUIUtility.labelWidth = prev;
        }
        EditorGUILayout.EndHorizontal();
    }

    void CheckModification()
    {
        if (moveWaveElement.oldIndex >= 0 && moveWaveElement.newIndex >= 0)
        {
            LevelData.Waves.MoveElement(moveWaveElement.oldIndex, moveWaveElement.newIndex);
        }
        if (removeWaveIndex >= 0) LevelData.Waves.RemoveAt(removeWaveIndex);
        if (addWaveIndex >= 0) LevelData.Waves.Insert(addWaveIndex+1, new WaveData());
        if (removeSpawnData.wave != null && removeSpawnData.dataIndex >= 0)
        {
            removeSpawnData.wave.MonsterSpawnData.RemoveAt(removeSpawnData.dataIndex);
        }
    }
}

public static class ListExtensions
{
    public static void MoveElement<T>(this List<T> list, int currentIndex, int newIndex)
    {
        if (currentIndex == newIndex) return;
        T item = list[currentIndex];
        list.RemoveAt(currentIndex);
        if (newIndex > currentIndex+1) newIndex--;
        list.Insert(newIndex, item);
    }
}
