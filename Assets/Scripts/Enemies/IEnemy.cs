using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(GameColor color);
    void OnReach();
    void OnDie();
}
