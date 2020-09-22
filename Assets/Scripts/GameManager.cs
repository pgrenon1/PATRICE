using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : OdinserializedSingletonBehaviour<GameManager>
{
    public GameObject splashScreenParent;

    public Player playerPrefab;
    [NonSerialized, OdinSerialize]
    public List<LevelData> levelDatas = new List<LevelData>();

    public float delayBeforeEffectiveSwitch = 1f;
    public AudioSource dimensionSwitchSound;
    public PostProcessEffectData dimensionSwitchEffect;

    public float timePerLevel = 60f;

    public Dimension ActiveDimension { get; set; }
    public bool IsGodMode { get; private set; }
    public float LevelTimer { get; private set; }
    public int Score { get; private set; }
    public bool GameIsStarted { get; private set; }
    public int LevelIndex { get; private set; }
    public Player Player { get; private set; }
    public bool IsPaused { get; private set; }

    public delegate void OnSwitchDimension(Dimension newActiveDimension);
    public event OnSwitchDimension DimensionSwitched;

    private PostProcessVolume _globalPPVolume;

    private void Start()
    {
        _globalPPVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
    }

    private void Update()
    {
        if (!GameIsStarted)
        {
            UpdateStartGame();

            return;
        }

        UpdateCheats();

        UpdateLevelTimer();

        UpdateLevelDatas();
    }

    private void UpdateLevelDatas()
    {
        foreach (var levelData in levelDatas)
        {
            if (levelData.IsActive)
                EnemyManager.Instance.UpdateSpawning(levelData);
        }
    }

    private void UpdateCheats()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            IsGodMode = !IsGodMode;

        if (Input.GetKeyDown(KeyCode.Return))
            NextLevel();

        if (Input.GetKeyDown(KeyCode.Space))
            Reset();
    }

    private void UpdateStartGame()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        LevelTimer = timePerLevel;
        GameIsStarted = true;

        InitLevelData(levelDatas[0]);

        CreatePlayer();

        StartCoroutine(WaitAndSwitch());
    }

    private IEnumerator WaitAndSwitch()
    {
        yield return 0;

        SwitchDimension(false);

        splashScreenParent.SetActive(false);
    }

    private void CreatePlayer()
    {
        Player = Instantiate(playerPrefab);
    }

    public void NextLevel()
    {
        LevelIndex++;

        LevelData newLevelData = null;
        if (LevelIndex > levelDatas.Count - 1)
        {
            newLevelData = AddLevel(levelDatas[levelDatas.Count - 1]);
        }
        else
        {
            newLevelData = levelDatas[LevelIndex];
        }

        InitLevelData(newLevelData);
    }

    private void InitLevelData(LevelData levelData)
    {
        levelData.SpawnedCountByType = new Dictionary<EnemyType, int>();

        foreach (var kvp in levelData.spawningData)
        {
            EnemyType enemyType = kvp.Key;
            levelData.SpawnedCountByType.Add(enemyType, 0);
        }

        levelData.IsActive = true;
    }

    public void TogglePause()
    {
        Time.timeScale = IsPaused ? 0f : 1f;

        IsPaused = !IsPaused;
    }

    private LevelData AddLevel(LevelData levelDataToCopy)
    {
        foreach (var kvp in levelDataToCopy.spawningData)
        {
            levelDataToCopy.spawningData[kvp.Key].enemiesAmount *= 2;
            levelDataToCopy.spawningData[kvp.Key].maxEnemiesAtSameTime *= 2;
        }

        levelDatas.Add(levelDataToCopy);

        return levelDataToCopy;
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void UpdateLevelTimer()
    {
        LevelTimer -= Time.deltaTime;

        if (LevelTimer <= 0)
        {
            NextLevel();

            LevelTimer = timePerLevel;
        }
    }

    public void SwitchDimension(bool triggerPostProcessEffect = true)
    {
        if (dimensionSwitchSound && triggerPostProcessEffect)
            dimensionSwitchSound.Play();

        StartCoroutine(DoSwitchDimension(triggerPostProcessEffect));
    }

    private IEnumerator DoSwitchDimension(bool triggerPostProcessEffect)
    {
        if (triggerPostProcessEffect)
        {
            StartCoroutine(DoEffect(dimensionSwitchEffect, _globalPPVolume));
            yield return new WaitForSeconds(delayBeforeEffectiveSwitch);
        }

        if (ActiveDimension == Dimension.Ice)
        {
            ActiveDimension = Dimension.Fire;
        }
        else if (ActiveDimension == Dimension.Fire)
        {
            ActiveDimension = Dimension.Ice;
        }

        if (DimensionSwitched != null)
            DimensionSwitched(ActiveDimension);
    }

    public void ScorePoints(int scoreValue)
    {
        Score += scoreValue;
    }

    public IEnumerator DoEffect(PostProcessEffectData postProcessEffectData, PostProcessVolume volume)
    {
        Bloom bloomSetting = null;
        ChromaticAberration chromaticAberrationSetting = null;
        Grain grainSetting = null;
        LensDistortion lensDistortionSetting = null;

        volume.profile.TryGetSettings(out bloomSetting);
        volume.profile.TryGetSettings(out chromaticAberrationSetting);
        volume.profile.TryGetSettings(out grainSetting);
        volume.profile.TryGetSettings(out lensDistortionSetting);

        float cachedBloom = bloomSetting.intensity.value;
        float cachedChromaticAberration = chromaticAberrationSetting.intensity.value;
        float cachedGrain = grainSetting.intensity.value;
        float cachedLensDistortion = lensDistortionSetting.intensity.value;

        float t = 0;
        while (t < postProcessEffectData.duration)
        {
            float ratio = t / postProcessEffectData.duration;

            bloomSetting.intensity.value = cachedBloom + postProcessEffectData.bloomOverTime.Evaluate(ratio) * postProcessEffectData.bloomStrength;
            chromaticAberrationSetting.intensity.value = cachedChromaticAberration + postProcessEffectData.chromaticAberrationOverTime.Evaluate(ratio) * postProcessEffectData.chromaticAberrationStrength;
            grainSetting.intensity.value = cachedGrain + postProcessEffectData.grainOverTime.Evaluate(ratio) * postProcessEffectData.grainStrength;
            lensDistortionSetting.intensity.value = cachedLensDistortion + postProcessEffectData.lensDistortionOverTime.Evaluate(ratio) * postProcessEffectData.lensDistortionStrength;

            t += Time.deltaTime;
            yield return 0;
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
