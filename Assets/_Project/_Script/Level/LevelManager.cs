using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private TextMeshProUGUI startLevelText;
    [SerializeField] private FloatValueSO crystalValue;
    [SerializeField] private FloatValueSO levelProgressValue;
    private LevelData levelData;
    private Dictionary<Collider, Entity> entityColliderMap = new();
    public float levelTotalTime;
    public float levelTimer;
    public float levelDisplayTimer;
    private List<Entity> previewMonsters = new();

    public void Init(int levelIndex)
    {
        LoadLevel(levelIndex);
        GameManager.Instance.SetGameState(GameState.Paused);

        crystalValue.Value = levelData.StartCrystal;
        levelTimer = levelDisplayTimer = levelTotalTime = 0f;
        PreloadEntities();
        StartSelection();
    }

    public async void StartSelection()
    {
        //preload avoid stutter mid game
        CardManager.Instance.SpawnDefaultCards(levelData.ForcedHeroes);
        UIManager.Instance.OpenImmediate<SelectCardCanvas>().Init(levelData.SelectableHeroes);
        UIManager.Instance.CloseImmediate<SelectCardCanvas>();

        await UniTask.Delay(1000);
        CameraManager.Instance.ToRoadView(duration: 1f);
        await UniTask.Delay(2000);
        GameManager.Instance.SetGameState(GameState.SelectCard);
        UIManager.Instance.Open<DeckCanvas>().SlotInCards();
        UIManager.Instance.Open<SelectCardCanvas>();
    }

    public async void StartLevel()
    {
        GameManager.Instance.SetGameState(GameState.Paused);
        SetupLevelProgress();
        CardManager.Instance.SetDeckActive(false);
        await PlayStartLevelText(); //4.5s
        ClearPreviewMonsters();
        CardManager.Instance.SetDeckActive(true);
        GameManager.Instance.SetGameState(GameState.Playing);
        WaveManager.Instance.gameObject.SetActive(false); //stop update
        await UniTask.Delay(5000);
        WaveManager.Instance.gameObject.SetActive(true);
        WaveManager.Instance.Init(levelData.Waves);
    }

    async UniTask PlayStartLevelText()
    {
        await UniTask.Delay(500);
        startLevelText.gameObject.SetActive(true);
        var textTf = startLevelText.transform;
        var targetScale = Vector3.one * 2.5f;
        var originalScale = textTf.localScale;
        startLevelText.SetText<string>("Ready?");
        await Tween.Scale(textTf, targetScale, 1f, ease: Ease.Linear);
        textTf.localScale = originalScale;
        startLevelText.SetText<string>("Set...");
        await Tween.Scale(textTf, targetScale, 1f, ease: Ease.Linear);
        textTf.localScale = originalScale;
        startLevelText.SetText<string>("FIGHT!");
        await Tween.Scale(textTf, targetScale, 1f, ease: Ease.Linear);
        await UniTask.Delay(1000);
        startLevelText.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (GameManager.GameState != GameState.Playing) return;
        UpdateLevelProgress();
    }

    void UpdateLevelProgress()
    {
        float baseSpeed = Time.deltaTime;
        levelTimer = Mathf.MoveTowards(levelTimer, levelTotalTime, baseSpeed);

        float catchupSpeed = Mathf.Lerp(baseSpeed * 10, baseSpeed, levelDisplayTimer / levelTotalTime);
        levelDisplayTimer = Mathf.MoveTowards(levelDisplayTimer, levelTimer, catchupSpeed);

        levelProgressValue.Value = levelDisplayTimer / levelTotalTime;
    }

    public void OnLevelComplete()
    {
        Debug.Log("Level Completed!");

    }

    void PreloadEntities()
    {
        HashSet<EntityID> uniqueEntityIDs = new();
        foreach (var wave in levelData.Waves)
        {
            foreach (var spawn in wave.SpawnDataList)
            {
                if (!uniqueEntityIDs.Add(spawn.EntityID)) continue;
            }
        }

        int previewCount = Mathf.Min(uniqueEntityIDs.Count, 10);
        foreach (var id in uniqueEntityIDs) //spawn each unique first
        {
            var previewSpawn = EntityFactory.Instance.SpawnPreviewMode(id);
            previewMonsters.Add(previewSpawn);
            previewCount--;
        }
        while(previewCount > 0) //pick random till 10
        {
            WaveData randWave = levelData.Waves[Random.Range(0, levelData.Waves.Count)];
            SpawnData randSpawnData = randWave.SpawnDataList[Random.Range(0, randWave.SpawnDataList.Count)];
            var previewSpawn = EntityFactory.Instance.SpawnPreviewMode(randSpawnData.EntityID);
            previewMonsters.Add(previewSpawn);
            previewCount--;
        }
    }

    public void ClearPreviewMonsters()
    {
        foreach (var monster in previewMonsters)
        {
            monster.Despawn();
        }
        previewMonsters.Clear();
    }

    void SetupLevelProgress()
    {
        var gameplayCanvas = UIManager.Instance.GetCanvas<GameplayCanvas>();
        gameplayCanvas.ClearFlags();
        int waveCount = levelData.Waves.Count;
        for (int i = 0; i < waveCount; i++)
        {
            levelTotalTime += levelData.Waves[i].WaveTime;
            if (!levelData.Waves[i].IsFlag) continue;

            float lerpFactor = (float)i / (waveCount - 1);
            gameplayCanvas.AddProgressFlag(lerpFactor);
        }
    }

    [Button]
    void LoadLevel(int index)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>($"Levels/Level_{index}");
        levelData = JsonUtility.FromJson<LevelData>(jsonAsset.text);
        Debug.Log($"Loaded Level {levelData.LevelIndex}");
    }

    [Button]
    void SaveLevel()
    {
        const string levelPath = "Assets/_Project/Resources/Levels";
        string fileName = $"Level_{levelData.LevelIndex}.json";
        string filePath = Path.Combine(levelPath, fileName);
        string json = JsonUtility.ToJson(levelData);
        File.WriteAllText(filePath, json);

        AssetDatabase.ImportAsset(filePath);
        Debug.Log($"Saved Level {levelData.LevelIndex} to: " + levelPath);
    }

    public void SkipLevelTime(float skippedTime)
    {
        levelTimer += skippedTime;
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
        return entityColliderMap.TryGetValue(collider, out entity);
    }
}
