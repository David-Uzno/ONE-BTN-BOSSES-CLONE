using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerScriptAssigner : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _movementScripts;

    private void OnEnable()
    {
        SelectMovement.OnMovementSelected.AddListener(AssignScriptToPlayer);
    }

    private void OnDisable()
    {
        SelectMovement.OnMovementSelected.RemoveListener(AssignScriptToPlayer);
    }

    private void Start()
    {
        AssignScriptToPlayer(SelectMovement.SelectedIndex);
    }

    private void AssignScriptToPlayer(int index)
    {
        if (index >= 0 && index < _movementScripts.Length)
        {
            _movementScripts[index].enabled = true;
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango.");
            _movementScripts[0].enabled = true;
        }
    }
}
