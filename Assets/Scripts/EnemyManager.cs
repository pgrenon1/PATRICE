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
}

[Serializable]
public class SpawningData
{
    public int enemiesAmount;
    public int maxEnemiesAtSameTime;
}

public class EnemyManager : OdinserializedSingletonBehaviour<EnemyManager>
{
    public Dictionary<EnemyType, EnemySettings> enemySettings = new Dictionary<EnemyType, EnemySettings>();
    [NonSerialized, OdinSerialize]
    public List<LevelData> levelDatas = new List<LevelData>();

    public Dictionary<EnemyType, List<Enemy>> RuntimeEnemiesByType { get; private set; } = new Dictionary<EnemyType, List<Enemy>>();
    //public Dictionary<EnemyType, int> SpawnedCountByType { get; private set; } = new Dictionary<EnemyType, int>();

    public int LevelIndex { get; private set; } = 0;
    public LevelData CurrentLevelData { get { return levelDatas[LevelIndex]; } }

    private void Start()
    {
        foreach (var kvp in CurrentLevelData.spawningData)
        {
            EnemyType enemyType = kvp.Key;

            CurrentLevelData.SpawnedCountByType = new Dictionary<EnemyType, int>();
            CurrentLevelData.SpawnedCountByType.Add(enemyType, 0);

            RuntimeEnemiesByType.Add(enemyType, new List<Enemy>());
        }
    }

    private void Update()
    {
        foreach (var kvp in CurrentLevelData.spawningData)
        {
            EnemyType enemyType = kvp.Key;
            if (CurrentLevelData.SpawnedCountByType[enemyType] < CurrentLevelData.spawningData[enemyType].enemiesAmount)
            {
                SpawnMissingEnemiesForType(enemyType);
            }
        }
    }

    private void SpawnMissingEnemiesForType(EnemyType enemyType)
    {
        while (RuntimeEnemiesByType[enemyType].Count < CurrentLevelData.spawningData[enemyType].maxEnemiesAtSameTime)
        {
            SpawnEnemy(enemyType);
        }
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 20f;

        Enemy enemy = Instantiate(enemySettings[enemyType].enemyPrefab, position, Quaternion.identity);
        enemy.EnemySpawner = this;
        enemy.OriginDimension = (Dimension)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Dimension)).Length);
        enemy.EnemyType = enemyType;

        RuntimeEnemiesByType[enemyType].Add(enemy);
        CurrentLevelData.SpawnedCountByType[enemyType]++;

        var str = "Level " + LevelIndex + "\n";
        foreach (var kvp in CurrentLevelData.SpawnedCountByType)
        {
            str += kvp.Key + " : " + kvp.Value + "/" + CurrentLevelData.spawningData[kvp.Key].enemiesAmount + "\n";
        }

        Debug.Log(str);
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        BoidManager.Instance.RemoveBoid(enemyToRemove.Boid, enemyToRemove.EnemyType);

        if (RuntimeEnemiesByType[enemyToRemove.EnemyType].Contains(enemyToRemove))
            RuntimeEnemiesByType[enemyToRemove.EnemyType].Remove(enemyToRemove);
    }

    public void NextLevel()
    {
        LevelIndex++;
    }
}

[Serializable]
public struct EnemySettings
{
    public Enemy enemyPrefab;
    public BoidSettings boidSettings;
}
