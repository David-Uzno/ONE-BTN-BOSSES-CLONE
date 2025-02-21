using System;
using System.Collections;
using System.Collections.Generic;
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
    public string ScriptReferenceName;
    private Type ScriptReference => Type.GetType(ScriptReferenceName);
    public GameObject MovementPrefab;
}
