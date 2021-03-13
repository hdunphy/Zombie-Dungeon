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

    // Start is called before the first frame update
    void Start()
    {
        LevelRules.Instance.SetSpawnerData(SpawnerRate, SpawnerRadius, EnemyLimit, SpawnerStartNumber);
        LevelGenerator.StartLevelCreation(EnemyStartNumber, SpawnerStartNumber);

        StartCoroutine(StartSpawners());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator StartSpawners()
    {
        yield return new WaitForSeconds(1f);

        foreach (EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.StartSpawner();
        }
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        
    }
}
