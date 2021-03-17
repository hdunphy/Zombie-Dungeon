using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int SpawnerRadius;

    [SerializeField] private int EnemyStartNumber;
    [Range(0, 1)]
    [SerializeField] private float EnemyStartNumberLevelIncrease;

    [SerializeField] private int SpawnerStartNumber;
    [Range(0, 1)]
    [SerializeField] private float SpawnerStartNumberLevelIncrease;

    [SerializeField] private float SpawnerRate;
    [Range(0, 1)]
    [SerializeField] private float SpawnerRateLevelIncrease;
    
    [SerializeField] private int EnemyLimit;
    [Range(0, 1)]
    [SerializeField] private float EnemyLimitLevelIncrease;


    [SerializeField] private LevelGenerator LevelGenerator;

    private float _enemyStartNumber, _spawnerStartNumber, _spawnerRate, _enemyLimit;
    private int waveNumber;

    // Start is called before the first frame update
    void Start()
    {
        LevelRules.Instance.EndLevel += EndLevel;

        waveNumber = 0;
        _enemyLimit = EnemyLimit;
        _enemyStartNumber = EnemyStartNumber;
        _spawnerRate = SpawnerRate;
        _spawnerStartNumber = SpawnerStartNumber;

        LevelRules.Instance.SetSpawnerData(SpawnerRate, SpawnerRadius, EnemyLimit, SpawnerStartNumber);
        LevelGenerator.StartLevelCreation(EnemyStartNumber, SpawnerStartNumber);
        PlayerHUD.Instance.SetWaveNumber(waveNumber);

        StartCoroutine(StartSpawners());
    }

    private void OnDestroy()
    {
        LevelRules.Instance.EndLevel -= EndLevel;
    }

    private IEnumerator StartSpawners()
    {
        yield return new WaitForSeconds(1f);

        foreach (EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.StartSpawner();
        }
    }

    private void EndLevel(bool playerWin)
    {
        if (playerWin)
        {
            StartCoroutine(NextLevel());
        }
    }

    private IEnumerator NextLevel()
    {
        PlayerHUD.Instance.SetWaveNumber(++waveNumber);
        yield return new WaitForSeconds(3f);

        _enemyStartNumber += _enemyStartNumber * EnemyStartNumberLevelIncrease;
        _enemyLimit += _enemyLimit * EnemyLimitLevelIncrease;
        _spawnerRate += _spawnerRate * SpawnerRateLevelIncrease;
        _spawnerStartNumber += _spawnerStartNumber * SpawnerStartNumberLevelIncrease;

        EnemyStartNumber = Mathf.RoundToInt(_enemyStartNumber);
        EnemyLimit = Mathf.RoundToInt(_enemyLimit);
        SpawnerRate = Mathf.RoundToInt(_spawnerRate);
        SpawnerStartNumber = Mathf.RoundToInt(_spawnerStartNumber);


        LevelRules.Instance.SetSpawnerData(SpawnerRate, SpawnerRadius, EnemyLimit, SpawnerStartNumber);

        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        LevelGenerator.NextLevel(EnemyStartNumber, SpawnerStartNumber, playerPos);

        yield return StartSpawners();
    }
}
