using System.IO;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _timer;

    [Header("Game Data")]
    private string _saveFilePath;
    private DataGame _gameData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _saveFilePath = Application.dataPath + "/save.json";
    }

    public DataGame LoadData()
    {
        if(File.Exists(_saveFilePath))
        {
            string content = File.ReadAllText(_saveFilePath);
            _gameData = JsonUtility.FromJson<DataGame>(content);
        }
        else
        {
            Debug.Log($"Archivo de guardado no existe en la ruta: {_saveFilePath}");
            _gameData = new DataGame();
        }
        return _gameData;
    }

    public void SavePoint(float bestTime, ushort level)
    {
        if (_gameData == null)
        {
            _gameData = new DataGame();
        }

        if (_gameData._pointsPerLevel.Count > level)
        {
            _gameData._pointsPerLevel[level] = bestTime;
        }
        else
        {
            while (_gameData._pointsPerLevel.Count < level)
            {
                _gameData._pointsPerLevel.Add(float.MaxValue);
            }
            _gameData._pointsPerLevel.Add(bestTime);
        }

        string JSON = JsonUtility.ToJson(_gameData);
        File.WriteAllText(_saveFilePath, JSON);

        Debug.Log($"Archivo guardado en la ruta: {_saveFilePath}");
    }
}
