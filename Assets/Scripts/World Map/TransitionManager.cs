using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TransitionMaganer : MonoBehaviour
{
    [Header("Zoom Transition")]
    [SerializeField] private float _transitionDuration = 0.25f;
    [SerializeField] private float _moveLeftAmount = 2.75f;
    [SerializeField] private float _zoomScale = 2f;

    [Header("Dependencies")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;

    private Vector3 _originalCameraPosition;
    private int _originalAssetsPPU;

    private void Start()
    {
        _originalCameraPosition = _cameraTransform.position;
        _originalAssetsPPU = _pixelPerfectCamera.assetsPPU;
    }

    public void SaveCameraPosition()
    {
        _originalCameraPosition = _cameraTransform.position;
    }

    public void StartTransition()
    {
        Vector3 targetPosition = _originalCameraPosition - new Vector3(_moveLeftAmount, 0, 0);
        StartCoroutine(SmoothTransition(_pixelPerfectCamera.assetsPPU, _pixelPerfectCamera.assetsPPU * _zoomScale, _transitionDuration, targetPosition));
    }

    public void RevertTransition()
    {
        Vector3 revertTargetPosition = _originalCameraPosition + new Vector3(_moveLeftAmount, 0, 0);
        StartCoroutine(SmoothTransition(_pixelPerfectCamera.assetsPPU, _originalAssetsPPU, _transitionDuration, revertTargetPosition));
    }

    private IEnumerator SmoothTransition(float start, float end, float duration, Vector3 targetCameraPosition)
    {
        float elapsed = 0f;
        Vector3 startPosition = _cameraTransform.position;
        Vector3 endPosition = targetCameraPosition;

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
