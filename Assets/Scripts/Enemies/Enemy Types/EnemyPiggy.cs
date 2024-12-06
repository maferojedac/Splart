using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiggy : Enemy
{
    // Caracteristicas que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public GameObject CoinPrefab;
    [Range(0, 1)] public float Generosity;
    [Range(1, 10)] public int ExplodeCoinAmount;

    public float RunSpeed;

    public int lifeAtStart = 10;

    public float spawnTossSpeedY;
    public float spawnTossSpeedXZ;

    private MapNode _targetNode;     // Siguiente nodo del mapa

    [Header("Enemy Setup")]
    public float verticalOffset = 50f;

    [Header("Piggy Sound Effects")]
    public AudioClip _hitGround;

    private bool touchingGround;

    public int life;

    private EnemyPooling _enemyPooling;

    void Start()
    {
        if (transform.parent.gameObject.name == "Enemies")
            _enemyPooling = transform.parent.GetComponent<EnemyPooling>();
        else
            _enemyPooling = GameObject.Find("Enemies").GetComponent<EnemyPooling>();
    }

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);

        Entity.DisableCollision(GetComponent<BoxCollider>());

        _rigidBody.velocity = Vector3.zero;

        life = lifeAtStart;
        _spriteRenderer.color = Color.white;

        touchingGround = false;

        _targetNode = _levelData.RandomNode();
        transform.position = _targetNode.Position + (verticalOffset * Vector3.up);

        _originalColorCount = 10;

         // Drop from sky
        StartCoroutine(Drop());
    }
     
    IEnumerator Drop()
    {
        yield return null;

        touchingGround = false;

        while (!touchingGround)
        {
            yield return null;
        }

        _soundManager.PlaySound(_hitGround);

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(15f);
        StartRunning();
    }

    public override void OnDamageTaken()
    {
        base.OnDamageTaken();
        life--;
        if (life > 0)
        {
            if (Random.value < Generosity)
            {
                ShootCoin();
            }
        }
        else
        {
            Kill();
        }
        
    }

    public override void OnDie()
    {
        base.OnDie();
        StopAllCoroutines();
        for (int i = 0; i < ExplodeCoinAmount; i++)
        {
            ShootCoin();
        }
    }

    private void ShootCoin()
    {
        Vector3 randomVector = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
        randomVector.Normalize();
        randomVector *= spawnTossSpeedXZ;
        randomVector.y = spawnTossSpeedY;

        Enemy coin = _enemyPooling.Spawn(CoinPrefab);
        coin.SetSoundManager(_soundManager);
        coin.Spawn(transform.position);

        coin.gameObject.SetActive(true);

        coin.SetSpeed(randomVector);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6)
            touchingGround = true;
    }

    IEnumerator FollowNode()
    {
        while ((_targetNode.Position - transform.position).magnitude > 3f)
        {
            Vector3 newSpeed = _rigidBody.velocity;
            float SpeedY = _rigidBody.velocity.y;
            // _targetNode.Position.y = transform.position.y;   // I don't feel comfortable leaving this commented, but also it would change the Y position of the nodes. Make radius more generous?

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_targetNode.Position - transform.position).normalized), Time.fixedDeltaTime * 0.1f);

            newSpeed = transform.rotation * Vector3.forward * RunSpeed * _levelData.GetGlobalSpeedMultiplier();
            newSpeed.y = SpeedY;

            _rigidBody.velocity = newSpeed;

            yield return null;
        }

        _targetNode = _levelData.RandomNode();
        StartCoroutine(FollowNode());
    }

    void OnBecameInvisible()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void StartRunning()
    {
        _targetNode = _levelData.RandomNode();
        StartCoroutine(FollowNode());
    }
}
