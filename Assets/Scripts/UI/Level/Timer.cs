using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    #region Variables
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _timeText;         
    [SerializeField] private TextMeshProUGUI _finalTimeText;     
    [SerializeField] private TextMeshProUGUI _highScoreMessage;  

    [Header("Messages")]
    [SerializeField] private string _finalTimeTextTemplate = "Tiempo Total: ";
    [SerializeField] private string _newRecordMessage = "¡Nuevo Récord!";

    [Header("Level Settings")]
    private ushort _elapsedTime;
    private ushort _bestTime;
    private float _timeAccumulator;

    private bool _isGameOver = false;    

    [Header("References")]
    private SaveManager _saveManager;
    private GameManager _gameManager;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _saveManager = FindObjectOfType<SaveManager>();
        
        if (_timeText == null)
        {
            Debug.LogError("_timeText no está asignado en el Inspector.");
        }
    }

    private void Start()
    {
        LoadBestTime();
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            AccumulateTime();
            UpdateTimeUI();
        }
    }
    #endregion

    #region Time Management
    private void AccumulateTime()
    {
        _timeAccumulator += Time.deltaTime;

        if (_timeAccumulator >= 1f)
        {
            _elapsedTime += 1;
            _timeAccumulator -= 1f;
        }
    }

    private void UpdateTimeUI()
    {
        ushort minutes = (ushort)(_elapsedTime / 60);
        ushort seconds = (ushort)(_elapsedTime % 60);

        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopGameTimer()
    {
        _isGameOver = true;

        _finalTimeText.text = _finalTimeTextTemplate + FormatTime(_elapsedTime);

        if (_elapsedTime < _bestTime)
        {
            _bestTime = _elapsedTime;
            _saveManager.SavePoint(_bestTime, _gameManager._currentLevel);
            _highScoreMessage.text = _newRecordMessage;
        }
        else
        {
            _highScoreMessage.text = "";
        }
    }

    string FormatTime(ushort time)
    {
        ushort minutes = (ushort)(time / 60);
        ushort seconds = (ushort)(time % 60);
        
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion

    #region Data Handling
    private void LoadBestTime()
    {
        if (_saveManager != null)
        {
            if (_gameManager._currentLevel < 0)
            {
                Debug.LogError("El nivel actual no puede ser negativo.");
                _bestTime = ushort.MaxValue;
                return;
            }

            var data = _saveManager.LoadData();
            if (data._pointsPerLevel.Count > _gameManager._currentLevel)
            {
                _bestTime = data._pointsPerLevel[_gameManager._currentLevel];
            }
            else
            {
                _bestTime = ushort.MaxValue;
            }
        }
        else
        {
            Debug.LogError("SaveManager no encontrado en la escena.");
            _bestTime = ushort.MaxValue;
        }
    }
    #endregion
}
