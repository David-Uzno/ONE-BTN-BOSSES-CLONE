using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacleAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private float _activationDelay = 3f;
    [SerializeField] private float _intervalBetweenAttacks = 5f;
    [SerializeField] private bool _autoStart = true;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;

        if (_autoStart)
        {
            StartRepeatingAttack();
        }
    }

    public void StartRepeatingAttack()
    {
        StartCoroutine(RepeatingObstacleSpawning());
    }

    private IEnumerator RepeatingObstacleSpawning()
    {
        // Espera el tiempo inicial antes del primer ataque
        yield return new WaitForSeconds(_activationDelay);

        while (true)
        {
            // Genera un obstáculo en una posición aleatoria dentro del área visible de la cámara
            Vector2 randomPosition = GetRandomPositionInCameraView();
            GameObject obstacle = Instantiate(_obstaclePrefab, randomPosition, Quaternion.identity);

            // Activa el colisionador del obstáculo
            Collider2D collider = obstacle.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Espera el tiempo configurado antes del próximo ataque
            yield return new WaitForSeconds(_intervalBetweenAttacks);
        }
    }

    private Vector2 GetRandomPositionInCameraView()
    {
        // Obtén las dimensiones de la cámara
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        // Calcula las coordenadas aleatorias dentro del área visible
        float randomX = Random.Range(_mainCamera.transform.position.x - width / 2, _mainCamera.transform.position.x + width / 2);
        float randomY = Random.Range(_mainCamera.transform.position.y - height / 2, _mainCamera.transform.position.y + height / 2);

        return new Vector2(randomX, randomY);
    }
}
