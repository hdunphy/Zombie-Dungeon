using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRules : MonoBehaviour
{
    [SerializeField] private float WaveScoreMultiplier;
    [SerializeField] private float EnemyScoreMultiplier;
    [SerializeField] private float SpawnerScoreMultiplier;


    public static LevelRules Instance;

    public float SpawnRate { get; private set; }
    public int SpawnRadius { get; private set; }
    public int EnemyLimit { get; private set; }
    public int NumberOfSpawners { get; private set; }

    private int EnemyCount;
    private int KillCount;
    private int waveNumber;
    private float score;

    public Action<bool> EndLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        waveNumber = 1;
        PlayerHUD.Instance.SetWaveNumber(waveNumber);
    }

    public void SetSpawnerData(float _spawnRate, int _spawnRadius, int _enemyLimit, int _numberOfSpawners)
    {
        SpawnRate = _spawnRate;
        SpawnRadius = _spawnRadius;
        EnemyLimit = _enemyLimit;
        NumberOfSpawners = _numberOfSpawners;
    }

    public void KilledEnemy(int enemies)
    {
        EnemyCount -= enemies;
        KillCount += enemies;
        CalculateScore(enemies, EnemyScoreMultiplier);
        CheckWinCondition();
    }

    private void CalculateScore(int count, float multiplier)
    {
        score += (count *  multiplier) * WaveScoreMultiplier * waveNumber;
        PlayerHUD.Instance.SetScore(Mathf.RoundToInt(score));
    }

    public void AddEnemy()
    {
        EnemyCount++;
    }

    public void RemoveSpawner()
    {
        NumberOfSpawners--;
        CalculateScore(1, SpawnerScoreMultiplier);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        //Debug.Log($"Enemy Count {EnemyCount}, Spawner Count {NumberOfSpawners}");
        if(NumberOfSpawners + EnemyCount <= 0)
        {
            PlayerHUD.Instance.SetWaveNumber(++waveNumber);
            EndLevel?.Invoke(true);
        }
    }

    public bool CanSpawnEnemies()
    {
        return EnemyCount < EnemyLimit;
    }
}
