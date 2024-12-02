using System.Collections;
using UnityEngine;

public class EnemyBlot : Enemy, IRusherEnemy
{
    [Header("Enemy Setup")]
    public GameObject InkBlotPrefab;
    public float JumpTime;

    [Header("Blot Sound Effects")]
    public AudioClip _spawn;
    public AudioClip _reach;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        _soundManager.PlaySound(_spawn);
    }

    public override void OnAttack()
    {
        CreateSplat();
    }

    public void OnReach(Vector3 targetPosition)
    {
        _enemyState = EnemyState.Attack;
        _isVulnerable = false;

        float Speed = (targetPosition - transform.position).magnitude / JumpTime;
        Vector3 Direction = (targetPosition - transform.position).normalized;

        _rigidBody.AddForce(Speed * Direction, ForceMode.Impulse);

        StartCoroutine(OnReachCoroutine(targetPosition));
    }

    IEnumerator OnReachCoroutine(Vector3 targetPosition)
    {
        while (transform.position.z > targetPosition.z) { yield return null; }
        OnAttackAnimationEnd();
    }

    void CreateSplat()
    {
        if (_levelData._gameRunning)
        {
            Effect newSplat = _fxPool.Spawn(InkBlotPrefab);
            _soundManager.PlaySound(_reach);

            newSplat.SetColor(_colors.toRGB());
            newSplat.Execute();
        }
    }
}
