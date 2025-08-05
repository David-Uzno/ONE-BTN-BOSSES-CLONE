using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _timer;

    [Header("Game Data")]
    [SerializeField] private string _saveFilePath;
    private DataGame _gameData;
    private DatabaseReference _databaseReference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        FirebaseInitializer();
        _saveFilePath = Application.dataPath + "/save.json";       
    }

    private void FirebaseInitializer()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase está inicializado correctamente.");
            }
            else
            {
                Debug.LogError($"No se pudo inicializar Firebase: {task.Result}");
            }
        });
    }

    public DataGame LoadData()
    {
        if (File.Exists(_saveFilePath))
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

    public void SaveScore(float bestTime, ushort level)
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

        SaveScoreToFirebase(bestTime, "userId_placeholder");

        Debug.Log($"Archivo guardado en la ruta: {_saveFilePath}");      
    }

    public void SaveScoreToFirebase(float score, string userId)
    {
        if (_databaseReference == null)
        {
            Debug.LogError("Firebase no está inicializado. Intentando inicializar...");
            FirebaseInitializer();
            return;
        }

        _databaseReference.Child("scores").Child(userId).SetValueAsync(score).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score guardado exitosamente en Firebase.");
            }
            else
            {
                Debug.LogError("Error al guardar el score en Firebase: " + task.Exception);
            }
        });
    }
}
