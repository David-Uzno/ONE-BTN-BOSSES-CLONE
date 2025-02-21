using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LoadLevelIndex : MonoBehaviour
{
    public static LoadLevelIndex Instance {get; private set;}

    [Header("Levels")]
    [SerializeField] private List<SceneAsset> _availableLevels;
    public ushort _currentLevelIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLevel()
    {
        if (_currentLevelIndex >= 0 && _currentLevelIndex < _availableLevels.Count)
        {
            SceneManager.LoadScene(_availableLevels[_currentLevelIndex].name);
        }
        else
        {
            Debug.LogError("Nivel inválido.");
        }
    }
}
