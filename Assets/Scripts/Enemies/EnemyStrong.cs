using UnityEngine;

public class EnemyStrong : Enemy, IRusherEnemy
{
    [Header("Effects")]
    public GameObject flashbang;

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

    public void OnReach(Vector3 dir)    // Update for precision PLEASE
    {
        _enemyState = EnemyState.Jump;
        _isVulnerable = false;

        dir.y = 0f;
        dir = dir.normalized * 20f;
        dir.y = 10f;

        _rigidBody.AddForce(dir, ForceMode.Impulse);
    }

    public override void OnAttack()
    {
        SelfDestruct();
    }


    void SelfDestruct()
    {
        if (_levelData._gameRunning)
        {
            if (GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage())
            {
                _soundManager.PlaySound(_reach);

                GameObject fb = Instantiate(flashbang);
                fb.transform.parent = transform.parent;
            }
        }
    }
}
