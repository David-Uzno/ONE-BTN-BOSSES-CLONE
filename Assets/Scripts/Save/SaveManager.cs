using System.IO;
using TMPro;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Fields
    public static SaveManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _timer;
    private readonly int _decimalsToTruncate = 3;

    [Header("Game Data")]
    [SerializeField] private string _saveFilePath;
    private DataGame _gameData;
    [SerializeField] private FirebaseService _firebaseService;
    #endregion

    #region Unity Events
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureFirebaseService();
        _firebaseService.Initialize();
        _saveFilePath = Application.dataPath + "/save.json";
    }

    private void EnsureFirebaseService()
    {
        if (_firebaseService != null)
        {
            return;
        }

        FirebaseService found = FindFirstObjectByType<FirebaseService>();
        if (found != null)
        {
            _firebaseService = found;
            return;
        }

        GameObject go = new GameObject(nameof(FirebaseService));
        _firebaseService = go.AddComponent<FirebaseService>();
    }
    #endregion

    #region Public API
    public DataGame LoadData()
    {
        if (File.Exists(_saveFilePath))
        {
            string content = File.ReadAllText(_saveFilePath);
            _gameData = JsonUtility.FromJson<DataGame>(content);
            DataTruncateHelper.TruncatePointsPerLevel(_gameData, _decimalsToTruncate);
        }
        else
        {
            Debug.Log($"Archivo de guardado no existe en la ruta: {_saveFilePath}");
            _gameData = new DataGame();
        }
        return _gameData;
    }

    public void SaveScore(float bestTime, ushort level)
    {
        if (_gameData == null)
            _gameData = new DataGame();

        DataGameHelper.UpdatePointsPerLevel(_gameData, bestTime, level, _decimalsToTruncate);
        DataTruncateHelper.TruncatePointsPerLevel(_gameData, _decimalsToTruncate);
        WriteDataToJsonFile();
        _firebaseService.SaveData(_gameData, "userId_placeholder", _decimalsToTruncate);
        Debug.Log($"Archivo guardado en la ruta: {_saveFilePath}");
    }

    public void SaveScoreToFirebase(float score, string userId)
    {
        _firebaseService.SaveScore(
            score,
            userId,
            _decimalsToTruncate,
            OnSaveSuccess,
            OnFail
        );
    }

    public void SaveDataToFirebase(DataGame gameData, string userId)
    {
        _firebaseService.SaveData(
            gameData,
            userId,
            _decimalsToTruncate,
            OnSaveDataSuccess,
            OnFail
        );
    }

    private void OnSaveSuccess()
    {
        Debug.Log("Score guardado exitosamente en Firebase.");
    }

    private void OnSaveDataSuccess()
    {
        Debug.Log("Nuevo DataGame guardado exitosamente en Firebase.");
    }

    private void OnFail(string error)
    {
        Debug.LogError(error);
    }
    #endregion

    #region File IO
    private void WriteDataToJsonFile()
    {
        DataTruncateHelper.TruncatePointsPerLevel(_gameData, _decimalsToTruncate);
        string json = DataTruncateHelper.BuildTruncatedJson(_gameData, _decimalsToTruncate);
        File.WriteAllText(_saveFilePath, json);
    }
    #endregion
}
