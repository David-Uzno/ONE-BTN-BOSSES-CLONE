using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAction
{
    void Execute(LevelLoader.Move move);
}
