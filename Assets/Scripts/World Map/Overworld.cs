using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Overworld : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private List<SceneAsset> _availableLevels;
    [SerializeField] private int _currentLevelIndex;

/*  [Header("Button Management")]
    [SerializeField] private List<ButtonLevel> _buttons;*/

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
