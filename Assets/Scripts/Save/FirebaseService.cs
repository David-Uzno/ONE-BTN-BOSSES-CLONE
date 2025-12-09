using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine;

public class FirebaseService : MonoBehaviour
{
    public static FirebaseService Instance;

    #region Variables
    private DatabaseReference _databaseReference;
    private bool _isInitializing;
    public bool IsReady => _databaseReference != null;
    public DatabaseReference RootReference => _databaseReference;
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
        Initialize();
    }
    #endregion

    #region Initialization
    public void Initialize(Action onReady = null, Action<string> onError = null)
    {
        if (IsReady)
        {
            InvokeSuccess(onReady);
            return;
        }

        if (_isInitializing)
            return;

        _isInitializing = true;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            _isInitializing = false;
            if (task.Result == DependencyStatus.Available)
            {
                _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase inicializado correctamente (FirebaseService).");
                InvokeSuccess(onReady);
            }
            else
            {
                string error = "No se pudo inicializar Firebase: " + task.Result;
                Debug.LogError(error);
                InvokeFail(onError, error);
            }
        });
    }
    #endregion

    #region Public API
    public void SaveScore(float score, string userId, int decimals, Action onSuccess = null, Action<string> onFail = null)
    {
        if (!IsReady)
        {
            InvokeFail(onFail, "Firebase no está listo.");
            return;
        }

        float truncatedScore = DataTruncateHelper.TruncateValue(score, decimals);
        DatabaseReference userScoresRef = _databaseReference.Child("scores").Child(userId);

        userScoresRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                InvokeFail(onFail, "Error al obtener los scores del usuario en Firebase.");
                return;
            }

            DataSnapshot snapshot = task.Result;
            ProcessScoresSnapshot(snapshot, truncatedScore, userScoresRef, onSuccess, onFail);
        });
    }

    public void SaveData(DataGame gameData, string userId, int decimals, Action onSuccess = null, Action<string> onFail = null)
    {
        FirebaseSavesService.Instance.SaveData(gameData, userId, decimals, onSuccess, onFail);
    }
    #endregion

    #region Helpers
    private void ProcessScoresSnapshot(DataSnapshot snapshot, float truncatedScore, DatabaseReference userScoresRef, Action onSuccess, Action<string> onFail)
    {
        int count = 0;
        float minScore = float.MaxValue;
        string minScoreKey = null;

        foreach (DataSnapshot child in snapshot.Children)
        {
            count++;
            float.TryParse(child.Value.ToString(), out float childScore);
            if (childScore < minScore)
            {
                minScore = childScore;
                minScoreKey = child.Key;
            }
        }

        if (count < 10)
        {
            userScoresRef.Push().SetValueAsync(truncatedScore).ContinueWith(saveTask =>
            {
                if (saveTask.IsCompleted)
                {
                    InvokeSuccess(onSuccess);
                }
                else
                {
                    string errorMsg = "Error al guardar score.";
                    if (saveTask.Exception != null)
                    {
                        errorMsg = saveTask.Exception.ToString();
                    }
                    InvokeFail(onFail, errorMsg);
                }
            });
            return;
        }

        if (truncatedScore > minScore && !string.IsNullOrEmpty(minScoreKey))
        {
            userScoresRef.Child(minScoreKey).RemoveValueAsync().ContinueWith(removeTask =>
            {
                if (removeTask.IsCompleted)
                {
                    userScoresRef.Push().SetValueAsync(truncatedScore).ContinueWith(saveTask =>
                    {
                        if (saveTask.IsCompleted)
                        {
                            InvokeSuccess(onSuccess);
                        }
                        else
                        {
                            string scoreSavingError = "Error al guardar score.";
                            if (saveTask.Exception != null)
                            {
                                scoreSavingError = saveTask.Exception.ToString();
                            }
                            InvokeFail(onFail, scoreSavingError);
                        }
                    });
                }
                else
                {
                    string scoreSavingError = "Error al eliminar score.";
                    if (removeTask.Exception != null)
                    {
                        scoreSavingError = removeTask.Exception.ToString();
                    }
                    InvokeFail(onFail, scoreSavingError);
                }
            });
            return;
        }

        InvokeFail(onFail, "No se guarda el score porque no mejora los 10 existentes.");
    }

    private void InvokeSuccess(Action callback)
    {
        if (callback != null)
        {
            callback();
        }
    }

    private void InvokeFail(Action<string> callback, string message)
    {
        if (callback != null)
        {
            callback(message);
        }
    }
    #endregion
}
