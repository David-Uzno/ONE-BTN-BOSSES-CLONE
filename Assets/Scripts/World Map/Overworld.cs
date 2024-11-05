using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Overworld : MonoBehaviour
{
    [SerializeField] private int _numerLevel;
    [SerializeField] private List<SceneAsset> _levels;

    void OnMouseDown()
    {
        if (_numerLevel >= 0 && _numerLevel < _levels.Count)
        {
            SceneManager.LoadScene(_levels[_numerLevel].name);
        }
        else
        {
            Debug.LogError("Número de nivel inválido");
        }
    }
}
