using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public int minX = -14;
    public int maxX = 14;
    public int minZ = -14;
    public int maxZ = 14;

    public float gridSize = 1f;

    private snakeMovement sm;
    private EnemySpawner enemySpawner;

    public void SpawnFood()
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

        Instantiate(foodPrefab, pos, Quaternion.identity);
    }

    void Start()
    {
        sm = FindFirstObjectByType<snakeMovement>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();

        SpawnFood();
    }

    bool IsPositionOccupied(Vector3 pos)
{
    foreach (Transform segment in sm.GetSegments())
    {
        if (Vector3.Distance(segment.position, pos) < 0.1f)
            return true;
    }

    if (enemySpawner != null)
    {
        foreach (GameObject enemy in enemySpawner.GetEnemies())
        {
            if (Vector3.Distance(enemy.transform.position, pos) < 0.1f)
                return true;
        }
    }

    return false;
}
}