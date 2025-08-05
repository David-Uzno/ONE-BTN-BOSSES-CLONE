using Firebase.Database;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveScore(int score)
    {
        string userId = "user123"; // Cambia esto por un identificador único para el usuario
        databaseReference.Child("scores").Child(userId).SetValueAsync(score).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score guardado exitosamente.");
            }
            else
            {
                Debug.LogError("Error al guardar el score: " + task.Exception);
            }
        });
    }
}
