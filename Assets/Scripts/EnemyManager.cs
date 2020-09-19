using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Evader
}

[Serializable]
public class LevelData
{
    public Dictionary<EnemyType, int> enemiesAmount = new Dictionary<EnemyType, int>();
    public Dictionary<EnemyType, int> maxEnemiesAtTheSameTime = new Dictionary<EnemyType, int>();
}

public class EnemyManager : OdinserializedSingletonBehaviour<EnemyManager>
{
    public Dictionary<EnemyType, EnemySettings> enemySettings = new Dictionary<EnemyType, EnemySettings>();
    [NonSerialized, OdinSerialize]
    public LevelData levelData;

    public Dictionary<EnemyType, List<Enemy>> RuntimeEnemiesByType { get; private set; } = new Dictionary<EnemyType, List<Enemy>>();
    public Dictionary<EnemyType, int> EnemiesSpawnedByType { get; private set; } = new Dictionary<EnemyType, int>();
    
    private void Start()
    {
        foreach (var kvp in levelData.enemiesAmount)
        {
            EnemyType enemyType = kvp.Key;

            EnemiesSpawnedByType.Add(enemyType, 0);

            RuntimeEnemiesByType.Add(enemyType, new List<Enemy>());
        }
    }

    private void Update()
    {
        foreach (var kvp in levelData.enemiesAmount)
        {
            EnemyType enemyType = kvp.Key;
            if (EnemiesSpawnedByType[enemyType] < levelData.enemiesAmount[enemyType])
            {
                SpawnMissingEnemiesForType(enemyType);
            }
        }
    }

    private void SpawnMissingEnemiesForType(EnemyType enemyType)
    {
        while (RuntimeEnemiesByType[enemyType].Count < levelData.maxEnemiesAtTheSameTime[enemyType])
        {
            SpawnEnemy(enemyType);
        }
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 20f;

        Enemy enemy = Instantiate(enemySettings[enemyType].enemyPrefab, position, Quaternion.identity);
        enemy.EnemySpawner = this;
        enemy.enemyType = enemyType;

        RuntimeEnemiesByType[enemyType].Add(enemy);
        EnemiesSpawnedByType[enemyType]++;
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        if (RuntimeEnemiesByType[enemyToRemove.enemyType].Contains(enemyToRemove))
            RuntimeEnemiesByType[enemyToRemove.enemyType].Remove(enemyToRemove);
    }
}

[Serializable]
public struct EnemySettings
{
    public Enemy enemyPrefab;
    public BoidSettings boidSettings;
}
