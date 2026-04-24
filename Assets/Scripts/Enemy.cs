using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySpawner spawner;
    public snakeMovement sm;
    

    private void Awake()
    {
        sm = FindFirstObjectByType<snakeMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sm.DecreaseScore();

            spawner.RemoveEnemy(gameObject);
        }
    }
}