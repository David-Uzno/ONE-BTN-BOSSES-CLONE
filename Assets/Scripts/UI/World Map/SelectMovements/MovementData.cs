using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptablesObjects/Movements")]
public class MovementData : ScriptableObject
{
    [System.Serializable]
    public class Character
    {
        public GameObject Player;
        public string Title;
        public string Description;
        public VideoClip VideoPlayer;
        public GameObject Icon;
    }
    public List<Character> Characters;
}
