using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewMovementData", menuName = "ScriptableObjects/MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Information")]
    public string Title;
    public string Description;
    public VideoClip VideoClip;

    [Header("Dependencies")]
    public MonoScript ScriptReference;
    public GameObject MovementPrefab;
}
