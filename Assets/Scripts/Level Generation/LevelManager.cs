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
    [SerializeField] private OnDeathPopup OnDeathPopup;

    private float _enemyStartNumber, _spawnerStartNumber, _spawnerRate, _enemyLimit;

    // Start is called before the first frame update
    void Start()
    {
        LevelRules.Instance.EndLevel += EndLevel;
        StartLevel();
    }

    private void StartLevel()
    {
        _enemyLimit = EnemyLimit;
        _enemyStartNumber = EnemyStartNumber;
        _spawnerRate = SpawnerRate;
        _spawnerStartNumber = SpawnerStartNumber;

        LevelRules.Instance.SetSpawnerData(SpawnerRate, SpawnerRadius, EnemyLimit, SpawnerStartNumber);
        LevelGenerator.StartLevelCreation(EnemyStartNumber, SpawnerStartNumber);

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
            AudioManager.Instance.PlaySound("End of Wave");
            StartCoroutine(NextLevel());
        }
        else
        {
            OnDeathPopup.PlayerDied();
        }
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlaySound("Next Wave");

        _enemyStartNumber += _enemyStartNumber * EnemyStartNumberLevelIncrease;
        _enemyLimit += _enemyLimit * EnemyLimitLevelIncrease;
        _spawnerRate -= _spawnerRate * SpawnerRateLevelIncrease;
        _spawnerStartNumber += _spawnerStartNumber * SpawnerStartNumberLevelIncrease;

        LevelRules.Instance.SetSpawnerData(_spawnerRate, SpawnerRadius, Mathf.RoundToInt(_enemyLimit), Mathf.RoundToInt(_spawnerStartNumber));

        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        LevelGenerator.NextLevel(Mathf.RoundToInt(_enemyStartNumber), Mathf.RoundToInt(_spawnerStartNumber), playerPos);

        yield return StartSpawners();
    }

    private void RestartLevel()
    {

        StartLevel();
    }
}
