using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{    [SerializeField]
    private float moveSpeed;
    private GameManager _gameManager;
    private int index = 0;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        HandleMovement();
    }

    public async void HandleMovement()
    {
        index = 0;

        while (index < _gameManager.EnemyPathFinding.Count)
        {
            await Move();
        }
    }

    public async Task Move()
    {
        float t = 0;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = _gameManager.EnemyPathFinding[index++];

        while (t <= 1.0f)
        {
            t += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, t));

            await Task.Yield();
        }
    }
}
