using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [System.Serializable]
    public class Move
    {
        public string Type;
        public float Tick;
        public float EnemyScale;
        public float TimeToScale;
        public List<Move> Moves;   // Subacciones
        public int Count;
        public float StartAngle;
        public bool Cw;
        public bool ShouldDelay;
    }

    [System.Serializable]
    public class Level
    {
        public List<Move> Moves;
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
