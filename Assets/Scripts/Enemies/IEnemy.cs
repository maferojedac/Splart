using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(GameColor color);
    void OnReach(Vector3 dir);
    void OnDie();
    void SetColor(ArrayColor startColor);
    Color GetColor();
}
