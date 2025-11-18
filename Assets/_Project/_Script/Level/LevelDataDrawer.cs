using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class LevelDataDrawer : Attribute{}

public sealed class LevelDataDrawerAttribute : OdinAttributeDrawer<LevelDataDrawer, LevelData>
{
    private LevelData LevelData;

    protected override void DrawPropertyLayout(GUIContent label)
    {
        LevelData = ValueEntry.SmartValue;
        if (LevelData == null) return;

        EditorGUILayout.Space(10);
        DrawLevelGeneralInfo(LevelData);
        EditorGUILayout.Space(5);

        if (LevelData.Waves == null) LevelData.Waves = new();

        var modifiedWaveList = LevelData.Waves; 
        int count = LevelData.Waves.Count;
        for (int i = 0; i < count; i++)
        {
            var wave = LevelData.Waves[i];
            SirenixEditorGUI.HorizontalLineSeparator(2);
            EditorGUILayout.BeginHorizontal();
            {
                DrawWaveHeader(i);
                if(SirenixEditorGUI.IconButton(EditorIcons.ArrowUp) && i > 0)
                {
                    modifiedWaveList = modifiedWaveList.ToList();
                    modifiedWaveList.MoveElement(i, i-1);
                }
                if(SirenixEditorGUI.IconButton(EditorIcons.ArrowDown) && i < count-1)
                {
                    modifiedWaveList = modifiedWaveList.ToList();
                    modifiedWaveList.MoveElement(i, i+1);
                }
                if(SirenixEditorGUI.IconButton(EditorIcons.Plus))
                {
                    modifiedWaveList = modifiedWaveList.ToList();
                    modifiedWaveList.Insert(i+1, new WaveData());
                }
                if(SirenixEditorGUI.IconButton(EditorIcons.Minus))
                {
                    modifiedWaveList = modifiedWaveList.ToList();
                    modifiedWaveList.RemoveAt(i);
                }
            }
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
    }
    
    void DrawLevelGeneralInfo(LevelData levelData)
    {
        levelData.LevelIndex = SirenixEditorFields.IntField("Level Index", levelData.LevelIndex);
        levelData.StartCrystal = SirenixEditorFields.FloatField("Start Crystal", levelData.StartCrystal);
    }

    private void DrawWaveHeader(int waveIndex)
    {
        GUILayout.Label($"Wave {waveIndex + 1}", EditorStyles.boldLabel);
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
        if (GUI.Button(GUILayoutUtility.GetRect(0, 30),"Add Spawn")) wave.SpawnDataList.Add(new SpawnData());
    }

    void DrawWaveSpawns(WaveData wave)
    {
        var prevLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50;
        var modifiedSpawnList = wave.SpawnDataList;
        var spawnCount = wave.SpawnDataList.Count;
        for(int i=0; i<spawnCount; i++)
        {
            var spawnData = wave.SpawnDataList[i];
            SirenixEditorGUI.BeginVerticalList(drawBorder: true, drawDarkBg: false, GUILayout.Width(200));
            {
                DrawSpawnData(spawnData);
                if (GUI.Button(GUILayoutUtility.GetRect(0, 20), "Remove"))
                {
                    modifiedSpawnList = modifiedSpawnList.ToList();
                    modifiedSpawnList.RemoveAt(i);
                }
            }
            SirenixEditorGUI.EndVerticalList();
        }
        wave.SpawnDataList = modifiedSpawnList;
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
        SirenixEditorFields.PreviewObjectField(iconRect, config.Icon, dragOnly: false, allowMove: false, allowSwap: false, allowSceneObjects: false);

        var costStyle = new GUIStyle(EditorStyles.boldLabel){alignment = TextAnchor.MiddleCenter};
        GUILayout.Label($"Spawn Cost: {config.SpawnCost}", costStyle);
        spawnData.PickWeight = SirenixEditorFields.IntField("Weight", spawnData.PickWeight);

        EditorGUILayout.BeginHorizontal();
        {
            var rect = EditorGUILayout.GetControlRect(); rect = EditorGUI.PrefixLabel(rect,  new GUIContent("Spawn"));
            var prev = EditorGUIUtility.labelWidth; EditorGUIUtility.labelWidth = 25;
            spawnData.MinSpawn = SirenixEditorFields.IntField(rect.AlignLeft(rect.width * 0.5f), new GUIContent("min"), spawnData.MinSpawn);
            spawnData.MaxSpawn = SirenixEditorFields.IntField(rect.AlignRight(rect.width * 0.5f), new GUIContent("max"), spawnData.MaxSpawn);
            EditorGUIUtility.labelWidth = prev;
        }
        EditorGUILayout.EndHorizontal();
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
