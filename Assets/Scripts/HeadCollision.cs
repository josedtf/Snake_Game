using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    public snakeMovement sm;

    public FoodSpawner spawner;

    public AudioSource eatSound;

    private void Awake()
    {
        sm = FindFirstObjectByType<snakeMovement>();

        spawner = FindObjectOfType<FoodSpawner>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            sm.GameOverFunc();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);

            eatSound.Play();
            sm.Grow();

            spawner.SpawnFood();
        }
    }
}
