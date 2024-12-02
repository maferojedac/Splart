using TMPro;
using UnityEngine;
using System.Collections;

public class EnemyStrong : Enemy, IRusherEnemy
{
    [Header("Enemy Setup")]
    public GameObject flashbang;
    public float JumpTime;

    [Header("Sumo Sound Clips")]
    public AudioClip _spawn;
    public AudioClip _reach;

    private bool _canBeDamaged;
    private float _punchDuration;


    private Vector3 _startScale;
    private float _timer;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        _soundManager.PlaySound(_spawn);
    }

    public void OnReach(Vector3 targetPosition)    // Update for precision PLEASE
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

    public override void OnAttack()
    {
        SelfDestruct();
    }


    void SelfDestruct()
    {
        if (_levelData._gameRunning)
        {
            if (GameObject.Find("Player").GetComponent<Player>().TakeDamage())
            {
                _soundManager.PlaySound(_reach);

                _fxPool.Spawn(flashbang);
            }
        }
    }
}
