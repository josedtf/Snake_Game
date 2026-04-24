using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int maxEnemies = 3;

    public int minX = -14;
    public int maxX = 14;
    public int minZ = -14;
    public int maxZ = 14;

    public float gridSize = 1f;

    public AudioSource hurtSound;

    private List<GameObject> enemies = new List<GameObject>();
    private snakeMovement sm;

    void Start()
    {
        sm = FindFirstObjectByType<snakeMovement>();
    }

     bool IsPositionOccupied(Vector3 pos)
    {
        foreach (Transform segment in sm.GetSegments())
        {
            if (Vector3.Distance(segment.position, pos) < 0.1f)
                return true;
        }

        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foods)
        {
            if (Vector3.Distance(food.transform.position, pos) < 0.1f)
                return true;
        }

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, pos) < 0.1f)
                return true;
        }

        return false;
    }

    public void TrySpawnEnemies(int score)
    {
        if (score < 10) return;

        while (enemies.Count < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos;
        int attempts = 0;

        do
        {
            int x = Random.Range(minX, maxX + 1);
            int z = Random.Range(minZ, maxZ + 1);

            pos = new Vector3(x * gridSize, 0, z * gridSize);

            attempts++;
        }
        while (IsPositionOccupied(pos) && attempts < 30);

        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

        enemies.Add(enemy);

        enemy.GetComponent<Enemy>().spawner = this;
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy);
        hurtSound.Play();

        SpawnEnemy();
    }
}