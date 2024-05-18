using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void EndGame();
    void StartGame();
    void UnloadLevel() { }

}
