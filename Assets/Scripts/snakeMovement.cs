using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class snakeMovement : MonoBehaviour
{
    public float moveInterval = 0.2f;
    public int startSize = 1;

    public GameObject bodyPrefab;

    public GameObject UIpanel;
    public GameObject gameoverpanel;
    public float mapLimitX = 14f;
    public float mapLimitZ = 14f;
    public float gridSize = 1f;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI finalScoreText;

    public AudioSource hitSound;

    public EnemySpawner enemySpawner;


    private Vector3 direction = Vector3.right;
    private List<Transform> segments = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();
    private bool GameOver = false;
    private int score;

    void Start()
    {
        score = -startSize;
        segments.Add(this.transform);

        for (int i = 0; i < startSize; i++)
        {
            Grow();
        }

        StartCoroutine(MoveCoroutine());

        enemySpawner = FindFirstObjectByType<EnemySpawner>();
    }

    void Update()
    {
        if (!GameOver)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame && direction != Vector3.back)
            direction = Vector3.forward;

        if (Keyboard.current.sKey.wasPressedThisFrame && direction != Vector3.forward)
            direction = Vector3.back;

        if (Keyboard.current.aKey.wasPressedThisFrame && direction != Vector3.right)
            direction = Vector3.left;

        if (Keyboard.current.dKey.wasPressedThisFrame && direction != Vector3.left)
            direction = Vector3.right;
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            if (!GameOver)
            {
                Move();
                CheckSelfCol();
            }
        }
    }

    void Move()
    {
        positions.Insert(0, transform.position);

        transform.position += direction * gridSize;

        for (int i = 1; i < segments.Count && i - 1 < positions.Count; i++)
        {
            segments[i].position = positions[i - 1];
        }

        if (positions.Count > segments.Count)
        {
            positions.RemoveAt(positions.Count - 1);
        }

        CheckMapLimits();
    }

    void CheckMapLimits()
    {
        Vector3 pos = transform.position;

        if (pos.x > mapLimitX || pos.x < -mapLimitX ||
            pos.z > mapLimitZ || pos.z < -mapLimitZ)
        {
            GameOverFunc();
        }
    }


    public void Grow()
    {
        GameObject segment = Instantiate(bodyPrefab);
        segment.transform.position = segments[segments.Count - 1].position;

        segments.Add(segment.transform);

        score ++;
        Score.text = "Score: " + score.ToString();

        enemySpawner.TrySpawnEnemies(score);
    }

    public void DecreaseScore()
    {
        score--;
        Score.text = "Score: " + score.ToString();
    }

    public void CheckSelfCol()
    {
        if (segments.Count < 2)
            return;

        Vector3 headPosition = segments[0].position;

        for (int i = 1; i < segments.Count; i++)
        {
            if (headPosition == segments[i].position)
            {
                GameOverFunc();
                break;
            }
        }
    }

    public List<Transform> GetSegments()
    {
        return segments;
    }

    public void GameOverFunc()
    {
        Destroy(gameObject);
        GameOver = true;

        UIpanel.SetActive(false);
        gameoverpanel.SetActive(true);
        hitSound.Play();

        finalScoreText.text = "Final Score: " + score.ToString();

        for (int i = 0; i < segments.Count; i++)
        {
            MeshRenderer renderer = segments[i].GetComponent<MeshRenderer>();

            if (renderer != null)
                renderer.material.color = Color.black;

            Rigidbody rb = segments[i].GetComponent<Rigidbody>();

            if (rb == null)
                rb = segments[i].gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.useGravity = true;

            segments[i].transform.localScale = Vector3.one * 0.75f;

            rb.AddForce(200 * Vector3.up);
        }
    }
}
