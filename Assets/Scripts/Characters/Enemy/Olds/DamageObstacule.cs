using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObstacule : MonoBehaviour

{
    [Header("Settings")]
    [SerializeField] private float _lifetime = 5f; 
    [SerializeField] private string _targetTag = "Player"; 

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_targetTag))
        {
            Destroy(other.gameObject); 
            Destroy(gameObject); 
        }
    }
}


