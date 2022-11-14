using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{   
    [SerializeField] private float moveSpeed;

    private GameManager _gameManager;
    private int index = 0;

    //runs code when the game starts
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        HandleMovement();
    }

    //starts the movment 
    public async void HandleMovement()
    {
        index = 0;

        while (index < _gameManager.EnemyPathFinding.Count)
        {
            await Move();
        }
    }

    //makes the enemy move to though the list setup in the gamemanager
    //to remove the step by step remove smoot Mathf.SmoothStep(0.0f, 1.0f, t) and replace it with "t"
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
