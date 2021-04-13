using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int SpawnerRadius;

    [SerializeField] private int EnemyStartNumber;
    [Range(0, 1)]
    [SerializeField] private float EnemyStartNumberLevelIncrease;

    [SerializeField] private float SpawnerRate;
    [Range(0, 1)]
    [SerializeField] private float SpawnerRateLevelIncrease;
    
    [SerializeField] private int EnemyLimit;
    [Range(0, 1)]
    [SerializeField] private float EnemyLimitLevelIncrease;

    [SerializeField] private LevelGenerator LevelGenerator;
    [SerializeField] private OnDeathPopup OnDeathPopup;

    [SerializeField] private List<LevelPrefab> LevelPrefabs;

    private float _enemyStartNumber, _spawnerRate, _enemyLimit;

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

        LevelPrefabs.ForEach(x => x.Start());

        LevelRules.Instance.SetSpawnerData(SpawnerRate, SpawnerRadius, EnemyLimit);
        LevelGenerator.StartLevelCreation(EnemyStartNumber, LevelPrefabs);

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

        LevelPrefabs.ForEach(x => x.NextLevel());

        LevelRules.Instance.SetSpawnerData(_spawnerRate, SpawnerRadius, Mathf.RoundToInt(_enemyLimit));

        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        LevelGenerator.NextLevel(Mathf.RoundToInt(_enemyStartNumber), playerPos);

        yield return StartSpawners();
    }

    private void RestartLevel()
    {

        StartLevel();
    }
}
