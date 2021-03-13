using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ITakeDamage
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private float Health;

    //internal variables
    private float nextSpawn;

    private void Awake()
    {
        nextSpawn = float.MaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn && LevelRules.Instance.CanSpawnEnemies())
        {
            SpawnEnemies();
            nextSpawn += LevelRules.Instance.SpawnRate;
        }
    }

    private void OnDestroy()
    {
        LevelRules.Instance.RemoveSpawner();
    }

    private void SpawnEnemies()
    {
        List<Vector2Int> openSpaces = new List<Vector2Int>();
        Vector2Int pos = Vector2Int.RoundToInt(transform.position);

        for (int i = pos.x - 1; i <= pos.x + 1; i++)
        {
            for (int j = pos.y - 1; j <= pos.y + 1; j++)
            {
                if (i != pos.x && j != pos.y)
                {
                    Vector2Int testPos = new Vector2Int(i, j);
                    var isOpen = Physics2D.OverlapCircle(testPos, 0.2f);
                    if (!isOpen)
                    {
                        openSpaces.Add(testPos);
                    }
                }
            }
        }

        Vector3 enemySpawnPos = (Vector2)openSpaces[UnityEngine.Random.Range(0, openSpaces.Count)];
        Instantiate(Enemy, enemySpawnPos, Quaternion.identity);
    }

    public void StartSpawner()
    {
        nextSpawn = Time.time;
    }

    public void SetHealth(float _health)
    {
        Health = _health;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
