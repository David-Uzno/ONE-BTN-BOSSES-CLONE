using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [System.Serializable]
    public class Move
    {
        public string Type;            // Tipo de movimiento o acción a ejecutar
        public float Tick;             // Tiempo de espera antes de ejecutar el movimiento
        public float EnemyScale;       // Escalado del enemigo
        public float TimeToScale;      // Tiempo para aplicar el escalado
        public int Count;              // Cantidad de elementos a generar
        public RandomRange StartAngle; // Rango de ángulo inicial para el movimiento
        public bool Cw;                // Dirección del movimiento (sentido horario)
        public bool ShouldDelay;       // Indica si debe haber un retraso antes de ejecutar
    }

    [System.Serializable]
    public class RandomRange
    {
        public float Min;
        public float Max;

        public float GetRandomValue()
        {
            return Random.Range(Min, Max);
        }
    }

    [System.Serializable]
    public class MoveGroup : Move
    {
        public List<Move> Moves;
    }

    [System.Serializable]
    public class Level
    {
        public List<MoveGroup> Moves;
    }

    public Level CurrentLevel;

    public void LoadLevel(TextAsset jsonFile)
    {
        if (jsonFile != null)
        {
            CurrentLevel = JsonUtility.FromJson<Level>(jsonFile.text);
        }
        else
        {
            Debug.LogError("TextAsset no asignado.");
        }
    }
}
