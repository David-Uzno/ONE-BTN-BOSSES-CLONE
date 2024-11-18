using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoAleatorioAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject _obstaclePrefab; // Prefab del obst�culo
    [SerializeField] private float _activationDelay = 3f; // Tiempo inicial antes del primer obst�culo
    [SerializeField] private float _intervalBetweenAttacks = 5f; // Intervalo entre cada ataque
    [SerializeField] private bool _autoStart = true; // Inicia autom�ticamente el ataque al comienzo

    private Camera _mainCamera;

    private void Start()
    {
        // Obt�n la referencia a la c�mara principal
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
            // Genera un obst�culo en una posici�n aleatoria dentro del �rea visible de la c�mara
            Vector2 randomPosition = GetRandomPositionInCameraView();
            GameObject obstacle = Instantiate(_obstaclePrefab, randomPosition, Quaternion.identity);

            // Activa el colisionador del obst�culo
            Collider2D collider = obstacle.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Espera el tiempo configurado antes del pr�ximo ataque
            yield return new WaitForSeconds(_intervalBetweenAttacks);
        }
    }

    private Vector2 GetRandomPositionInCameraView()
    {
        // Obt�n las dimensiones de la c�mara
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        // Calcula las coordenadas aleatorias dentro del �rea visible
        float randomX = Random.Range(_mainCamera.transform.position.x - width / 2, _mainCamera.transform.position.x + width / 2);
        float randomY = Random.Range(_mainCamera.transform.position.y - height / 2, _mainCamera.transform.position.y + height / 2);

        return new Vector2(randomX, randomY);
    }
}
