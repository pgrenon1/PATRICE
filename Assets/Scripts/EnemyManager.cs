using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Wanderer = 0,
    Seeker = 1
}

[Serializable]
public class LevelData
{
    public Dictionary<EnemyType, SpawningData> spawningData = new Dictionary<EnemyType, SpawningData>();

    public Dictionary<EnemyType, int> SpawnedCountByType { get; set; } = new Dictionary<EnemyType, int>();
    public bool IsActive { get; set; } = false;

    public bool IsCompleted()
    {
        foreach (var kvp in spawningData)
        {
            if (SpawnedCountByType[kvp.Key] < kvp.Value.enemiesAmount)
                return false;
        }

        return true;
    }
}

[Serializable]
public class SpawningData
{
    public int enemiesAmount;
    public int maxEnemiesAtSameTime;
}

public class EnemyManager : OdinserializedSingletonBehaviour<EnemyManager>
{
    public GameObject spawnVFXPrefab;

    public Dictionary<EnemyType, EnemySettings> enemySettings = new Dictionary<EnemyType, EnemySettings>();
    [NonSerialized, OdinSerialize]
    public List<LevelData> levelDatas = new List<LevelData>();

    //public Dictionary<EnemyType, List<Enemy>> RuntimeEnemiesByType { get; private set; } = new Dictionary<EnemyType, List<Enemy>>();
    //public Dictionary<EnemyType, int> SpawnedCountByType { get; private set; } = new Dictionary<EnemyType, int>();

    public int LevelIndex { get; private set; } = 0;
    //public LevelData CurrentLevelData { get { return levelDatas[LevelIndex]; } }
    //public List<LevelData> ActiveLevelDatas { get; set; } = new List<LevelData>();

    private void Start()
    {
        InitLevelData(levelDatas[0]);
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
        //ActiveLevelDatas.Add(levelData);
    }

    private void Update()
    {
        foreach (var levelData in levelDatas)
        {
            if (levelData.IsActive)
                UpdateLevelData(levelData);
        }
    }

    private void UpdateLevelData(LevelData levelData)
    {
        foreach (var kvp in levelData.spawningData)
        {
            EnemyType enemyType = kvp.Key;
            if (levelData.SpawnedCountByType[enemyType] < levelData.spawningData[enemyType].enemiesAmount)
            {
                SpawnMissingEnemiesForType(enemyType, levelData);
            }
        }
    }

    private void SpawnMissingEnemiesForType(EnemyType enemyType, LevelData levelData)
    {
        while (levelData.SpawnedCountByType[enemyType] < levelData.spawningData[enemyType].maxEnemiesAtSameTime)
        {
            SpawnEnemy(enemyType, levelData);
        }
    }

    private void SpawnEnemy(EnemyType enemyType, LevelData levelData)
    {
        RaycastHit hitInfo;
        Vector3 direction = BoidHelper.directions[UnityEngine.Random.Range(0, BoidHelper.directions.Length - 1)];
        Vector3 position = UnityEngine.Random.insideUnitSphere * 250f;
        if (Physics.Raycast(Vector3.zero, direction, out hitInfo, 500f))
        {
            if (hitInfo.collider.gameObject.GetComponent<DamageZone>())
            {
                position = hitInfo.point - direction.normalized * 30f;
                GameObject vfx = Utility.SpawnVFX(spawnVFXPrefab, hitInfo.point, Quaternion.identity, 1f);
                vfx.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }
        }

        Enemy enemy = Instantiate(enemySettings[enemyType].enemyPrefab, position, Quaternion.identity);
        enemy.EnemySpawner = this;
        enemy.ScoreValue = (LevelIndex + 1) * enemySettings[enemyType].scoreValue;
        enemy.transform.LookAt(Vector3.zero);
        enemy.OriginDimension = (Dimension)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Dimension)).Length);
        enemy.EnemyType = enemyType;

        levelData.SpawnedCountByType[enemyType]++;

        var str = "Level " + LevelIndex + "\n";
        foreach (var kvp in levelData.SpawnedCountByType)
        {
            str += kvp.Key + " : " + kvp.Value + "/" + levelData.spawningData[kvp.Key].enemiesAmount + "\n";
        }

        Debug.Log(str);

        if (levelData.IsCompleted())
        {
            levelData.IsActive = false;
        }
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        BoidManager.Instance.RemoveBoid(enemyToRemove.Boid, enemyToRemove.EnemyType);
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
}

[Serializable]
public struct EnemySettings
{
    public Enemy enemyPrefab;
    public BoidSettings boidSettings;
    public int scoreValue;
}
