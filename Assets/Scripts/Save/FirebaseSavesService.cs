using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#region Config
public class FirebaseSavesConfig : MonoBehaviour
{
    public static FirebaseSavesConfig Instance;
    [Header("Límite de saves por usuario (nuevo save será el límite + 1)")]
    public int MaxSaves = 9;

    private void Awake()
    {
        Instance = this;
    }
}
#endregion

#region MainService
public class FirebaseSavesService
{
    public static readonly FirebaseSavesService Instance = new();
    private FirebaseSavesService() { }

    #region SaveData
    // Guarda los datos del juego en Firebase, si cumple las condiciones
    public void SaveData(DataGame gameData, string userId, int decimals, Action onSuccess = null, Action<string> onFail = null)
    {
        FirebaseService firebaseService = FirebaseService.Instance;
        if (firebaseService == null || !firebaseService.IsReady)
        {
            InvokeFail(onFail, "Firebase no está listo.");
            return;
        }

        // Trunca los puntos y genera el JSON
        DataTruncateHelper.TruncatePointsPerLevel(gameData, decimals);
        string saveJson = DataTruncateHelper.BuildTruncatedJson(gameData, decimals);
        float newComparableValue = GetComparableValueFromDataGame(gameData);

        DatabaseReference savesReference = firebaseService.RootReference.Child("users").Child(userId).Child("saves");
        savesReference.GetValueAsync().ContinueWith(getTask =>
        {
            if (getTask.IsFaulted || getTask.IsCanceled)
            {
                InvokeFail(onFail, "Error al obtener saves del usuario en Firebase.");
                return;
            }

            DataSnapshot savesSnapshot = getTask.Result;
            ProcessSavesSnapshot(savesSnapshot, saveJson, newComparableValue, savesReference, onSuccess, onFail);
        });
    }
    #endregion

    #region SaveProcessing
    // Procesa los saves existentes y decide si guardar el nuevo
    private void ProcessSavesSnapshot(DataSnapshot savesSnapshot, string saveJson, float newComparableValue, DatabaseReference savesReference, Action onSuccess, Action<string> onFail)
    {
        List<(string saveKey, float saveValue)> saveEntries = new();
        foreach (DataSnapshot saveChild in savesSnapshot.Children)
        {
            saveEntries.Add((saveChild.Key, GetSaveComparableValue(saveChild)));
        }

        // Acción para guardar el nuevo save
        void SaveNewEntry()
        {
            savesReference.Push().SetRawJsonValueAsync(saveJson).ContinueWith(saveTask =>
            {
                if (saveTask.IsCompleted && !saveTask.IsFaulted)
                {
                    InvokeSuccess(onSuccess);
                }
                else
                {
                    string errorMsg = "Error al guardar DataGame en Firebase.";
                    if (saveTask.Exception != null)
                    {
                        errorMsg = saveTask.Exception.ToString();
                    }
                    InvokeFail(onFail, errorMsg);
                }
            });
        }

        int maxSaves = FirebaseSavesConfig.Instance != null ? FirebaseSavesConfig.Instance.MaxSaves : 9;

        // Ordena los saves por valor comparable descendente
        saveEntries.Sort((a, b) =>
        {
            int compare = b.saveValue.CompareTo(a.saveValue);
            if (compare != 0)
            {
                return compare;
            }
            return string.CompareOrdinal(a.saveKey, b.saveKey);
        });

        // Calcula el mínimo existente
        float minExistingValue = float.MaxValue;
        for (int i = 0; i < saveEntries.Count; i++)
        {
            if (saveEntries[i].saveValue < minExistingValue)
            {
                minExistingValue = saveEntries[i].saveValue;
            }
        }

        // Rechaza si el nuevo valor es mayor que cualquiera de los existentes
        if (saveEntries.Count > 0 && newComparableValue > minExistingValue)
        {
            InvokeFail(onFail, "No se guarda el save porque es mayor que un valor existente.");
            return;
        }

        // Caso: aún no se alcanza el límite, guardar directamente
        if (saveEntries.Count < maxSaves)
        {
            SaveNewEntry();
            return;
        }

        // Caso: límite alcanzado o superado, eliminar solo el mayor y luego guardar
        if (saveEntries.Count > 0)
        {
            var largestEntry = saveEntries[0]; // mayor valor comparable
            RemoveSequential(savesReference, new List<string> { largestEntry.saveKey }, 0, onFail, SaveNewEntry);
            return;
        }

        // Si no había entradas (caso defensivo)
        SaveNewEntry();
    }
    #endregion

    #region SequentialRemoval
    // Elimina saves de forma secuencial y luego ejecuta la acción final
    private void RemoveSequential(DatabaseReference savesReference, List<string> keysToRemove, int currentIndex, Action<string> onFail, Action onDone)
    {
        if (currentIndex >= keysToRemove.Count)
        {
            if (onDone != null)
            {
                onDone();
            }
            return;
        }

        savesReference.Child(keysToRemove[currentIndex]).RemoveValueAsync().ContinueWith(removeTask =>
        {
            if (removeTask.IsCompleted && !removeTask.IsFaulted)
            {
                RemoveSequential(savesReference, keysToRemove, currentIndex + 1, onFail, onDone);
            }
            else
            {
                string deleteSaveErrorMessage = "Error al eliminar save más alto.";
                if (removeTask.Exception != null)
                {
                    deleteSaveErrorMessage = removeTask.Exception.ToString();
                }
                InvokeFail(onFail, deleteSaveErrorMessage);
            }
        });
    }
    #endregion

    #region ComparisonUtils
    // Obtiene el valor comparable máximo de un DataGame
    private float GetComparableValueFromDataGame(DataGame gameData)
    {
        float maxValue = float.MinValue;

        if (gameData == null)
        {
            return maxValue;
        }

        Type dataType = gameData.GetType();
        FieldInfo[] fields = dataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; i++)
        {
            object fieldValue = fields[i].GetValue(gameData);
            UpdateMaxFromObject(fieldValue, ref maxValue);
        }

        PropertyInfo[] properties = dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < properties.Length; i++)
        {
            if (!properties[i].CanRead)
            {
                continue;
            }
            object propertyValue = properties[i].GetValue(gameData, null);
            UpdateMaxFromObject(propertyValue, ref maxValue);
        }

        return maxValue;
    }

    // Actualiza el valor máximo si el objeto es numérico
    private void UpdateMaxFromObject(object numericValue, ref float maxValue)
    {
        if (numericValue == null)
        {
            return;
        }

        if (numericValue is float)
        {
            float floatVal = (float)numericValue;
            if (floatVal > maxValue)
            {
                maxValue = floatVal;
            }
            return;
        }

        if (numericValue is int)
        {
            float floatVal = (int)numericValue;
            if (floatVal > maxValue)
            {
                maxValue = floatVal;
            }
            return;
        }

        if (numericValue is double firebaseSaveTimestamp)
        {
            float floatVal = (float)firebaseSaveTimestamp;
            if (floatVal > maxValue)
            {
                maxValue = floatVal;
            }
            return;
        }

        if (numericValue is long)
        {
            float floatVal = (long)numericValue;
            if (floatVal > maxValue)
            {
                maxValue = floatVal;
            }
            return;
        }
    }
    #endregion

    #region Callbacks
    // Invoca el callback de éxito
    private void InvokeSuccess(Action callback)
    {
        if (callback != null)
        {
            callback();
        }
    }

    // Invoca el callback de error con mensaje
    private void InvokeFail(Action<string> callback, string message)
    {
        if (callback != null)
        {
            callback(message);
        }
    }
    #endregion

    #region Unity Snapshots
    // Obtiene el valor comparable máximo de un snapshot de save
    private float GetSaveComparableValue(DataSnapshot saveSnapshot)
    {
        float maxValue = float.MinValue;

        // Recorre recursivamente el snapshot para encontrar el valor máximo
        void Traverse(DataSnapshot node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Value != null)
            {
                if (float.TryParse(node.Value.ToString(), out float value))
                {
                    if (value > maxValue)
                    {
                        maxValue = value;
                    }
                }
            }
            foreach (DataSnapshot childNode in node.Children)
            {
                Traverse(childNode);
            }
        }

        Traverse(saveSnapshot);
        return maxValue;
    }
    #endregion
}
#endregion
