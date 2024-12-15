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
        public List<Move> Moves;

        [Header("Boss")]
        public float EnemyScale;
        public float TimeToScale;

        [Header("Triangle")]
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

    public void LoadLevel(string filePath)
    {
        string json = File.ReadAllText(filePath);
        CurrentLevel = JsonUtility.FromJson<Level>(json);
    }
}
