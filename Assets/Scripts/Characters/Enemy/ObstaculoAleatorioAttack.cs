using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoAleatorioAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject _obstaclePrefab; 
    [SerializeField] private float _circleRadius = 5f;   
    [SerializeField] private float _activationDelay = 3f; 

    public void ActivateAttack()
    {
        StartCoroutine(SpawnObstacleWithDelay());
    }

    private IEnumerator SpawnObstacleWithDelay()
    {
        yield return new WaitForSeconds(_activationDelay);

        Vector2 randomPosition = Random.insideUnitCircle.normalized * _circleRadius;

        GameObject obstacle = Instantiate(_obstaclePrefab, randomPosition, Quaternion.identity);

        Collider2D collider = obstacle.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
}