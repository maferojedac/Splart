using System.Collections;
using UnityEngine;

public class EnemyBlot : Enemy, IRusherEnemy
{
    [Header("Enemy Setup")]
    public GameObject InkBlotPrefab;
    public float JumpTime;

    [Header("Blot Sound Effects")]
    public AudioClip _reach;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
    }

    public override void OnAttack()
    {
        CreateSplat();
    }

    public void OnReach(Vector3 targetPosition)
    {
        _enemyState = EnemyState.Attack;
        _isVulnerable = false;

        Vector3 Speed = (targetPosition - transform.position);
        Speed.y = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * Mathf.Abs(Speed.y));
        Speed *= 5f;

        _rigidBody.AddForce(Speed, ForceMode.Impulse);

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
        }
    }
}
