using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    public snakeMovement sm;

    private void Awake()
    {
        sm = FindFirstObjectByType<snakeMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            Debug.Log("Hit Wall - Game Over");
            sm.GameOverFunc();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject, 0.1f);
            Debug.Log("Growing snake");
            sm.Grow();
        }
    }
}
