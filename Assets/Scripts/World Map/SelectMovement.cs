using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectMovement : MonoBehaviour
{
    [SerializeField] List<GameObject> _movements;
    private int _currentIndex = 0;

    void Start()
    {
        foreach (GameObject movement in _movements)
        {
            movement.SetActive(false);
        }

        if (_movements.Count > 0)
        {
            _movements[_currentIndex].SetActive(true);
        }
    }

    public void OnPreviousMovement()
    {
        if (_movements.Count == 0) return;

        _movements[_currentIndex].SetActive(false);

        _currentIndex = (_currentIndex - 1 + _movements.Count) % _movements.Count;

        _movements[_currentIndex].SetActive(true);
    }

    public void OnNextMovement()
    {
        if (_movements.Count == 0) return;

        _movements[_currentIndex].SetActive(false);

        _currentIndex = (_currentIndex + 1) % _movements.Count;

        _movements[_currentIndex].SetActive(true);
    }
}
