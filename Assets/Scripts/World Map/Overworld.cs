using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.U2D;

public class Overworld : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private List<SceneAsset> _availableLevels;
    [SerializeField] private int _currentLevelIndex;

    [Header("Zoom Transition")]
    [SerializeField] private float _transitionDuration = 0.5f;
    [SerializeField] private float _moveLeftAmount = 2.75f;
    [SerializeField] private float _zoomScale = 2f;

    [Header("Dependencies")]
    [SerializeField] private GameObject _powerUpsUI;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;

    [Header("Button Management")]
    [SerializeField] private List<ButtonLevel> _buttons;

    private void OnEnable()
    {
        ButtonLevel.OnButtonClicked += HandleButtonClicked;
    }

    private void OnDisable()
    {
        ButtonLevel.OnButtonClicked -= HandleButtonClicked;
    }

    private void HandleButtonClicked(ButtonLevel button)
    {
        _currentLevelIndex = button.buttonIndex;
        if (_currentLevelIndex >= 0 && _currentLevelIndex < _availableLevels.Count)
        {
            //StartCoroutine(SmoothTransition(_pixelPerfectCamera.assetsPPU, _pixelPerfectCamera.assetsPPU * _zoomScale, _transitionDuration));
            _powerUpsUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Número de nivel inválido");
        }
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(_availableLevels[_currentLevelIndex].name);
    }

    IEnumerator SmoothTransition(float start, float end, float duration)
    {
        float elapsed = 0f;
        Vector3 startPosition = _cameraTransform.position;
        Vector3 endPosition = startPosition + Vector3.left * _moveLeftAmount;

        while (elapsed < duration)
        {
            _pixelPerfectCamera.assetsPPU = (int)Mathf.Lerp(start, end, elapsed / duration);
            _cameraTransform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _pixelPerfectCamera.assetsPPU = (int)end;
        _cameraTransform.position = endPosition;
    }
}
