// Script for Enemy Sumo

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStrong : Enemy, IRusherEnemy
{
    [Header("Enemy Setup")]
    public GameObject flashbang;
    public float JumpTime;

    [Header("Sumo Sound Clips")]
    public AudioClip _reach;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);

        List<GameColor> duplicated = new List<GameColor>();

        foreach(GameColor color in _colors)
        {
            duplicated.Add(color);
        }

        _colors.Add(duplicated);
        _originalColorCount = _colors.Count();
        
    }

    public override void OnDie()
    {
        base.OnDie();
        if (_levelData._gameRunning)
            _levelData.PaintObject();   // Boss enemies paint the background
    }

    public void OnReach(Vector3 targetPosition)    
    {
        _enemyState = EnemyState.Attack;
        _isVulnerable = false;

        Vector3 Speed = (targetPosition - transform.position);
        Speed.y = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * Speed.y);
        Speed *= 5f;

        _rigidBody.AddForce(Speed, ForceMode.Impulse);

        StartCoroutine(OnReachCoroutine(targetPosition));
    }

    IEnumerator OnReachCoroutine(Vector3 targetPosition)
    {
        while (transform.position.z > targetPosition.z) { yield return null; }
        OnAttackAnimationEnd();
    }

    public override void OnAttack()
    {
        SelfDestruct();
    }

    void SelfDestruct()
    {
        if (_levelData._gameRunning)
        {
            _soundManager.PlaySound(_reach);

            _fxPool.Spawn(flashbang);
        }
    }
}
