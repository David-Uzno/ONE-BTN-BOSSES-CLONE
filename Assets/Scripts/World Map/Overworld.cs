using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Overworld : MonoBehaviour
{
    [SerializeField] private int _numerLevel;
    [SerializeField] private List<SceneAsset> _levels;

    [Header("UI")]
    [SerializeField] private GameObject _powerUpsUI;

    void OnMouseDown()
    {
        if (_numerLevel >= 0 && _numerLevel < _levels.Count)
        {
            _powerUpsUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Número de nivel inválido");
        }
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(_levels[_numerLevel].name);
    }
}
