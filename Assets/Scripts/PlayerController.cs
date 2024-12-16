using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private GameObject _playerGameObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_playerGameObject == null)
        {
            _playerGameObject = GameObject.Find("PlayerOverworld");
            if (_playerGameObject == null)
            {
                Debug.LogError("PlayerOverworld GameObject no encontrado.");
            }
        }
    }

    public GameObject GetPlayerGameObject()
    {
        return _playerGameObject;
    }
}