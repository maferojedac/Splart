using System.Collections;
using UnityEngine;

public class EnemyCoin : Enemy
{
    // Caracteristicas que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public /*static*/ float RunSpeed;   // Upon discussion, it was decided that all enemies should share the same speed
    public float TurnSpeed;
    public float NodeProximity;

    private MapNode _targetNode;     // Siguiente nodo del mapa

    private PlayerData _playerData;

    private bool touchingGround;
    private bool canTouchGround;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);

        if (_playerData == null)
            _playerData = GameObject.Find("CommunicationPrefab").GetComponent<CommunicationPrefabScript>()._playerData;

        Entity.DisableCollision(GetComponent<BoxCollider>());

        _spriteRenderer.color = Color.white;

        // Adjust coin to new color
        touchingGround = false;
        canTouchGround = false;

        // Toss coin into the air
        StartCoroutine(CoinToss());
    }

    IEnumerator CoinToss()
    {
        yield return null;

        while (!touchingGround)
        {
            Debug.Log("Coin Speed > " + _rigidBody.velocity);
            canTouchGround = _rigidBody.velocity.y < 0;
            yield return null;
        }

        _targetNode = _levelData.RandomNode();
        StartRunning();
    }

    public override void OnDamageTaken()
    {
        base.OnDamageTaken();

        Kill();
    }

    public override void OnDie()
    {
        base.OnDie();
        if(_levelData._gameRunning)
            _playerData.SumMoney(10);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 6 && canTouchGround)  // Six for terrain layer
        {
            touchingGround = true;
        }
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator FollowNode()
    {
        while ((_targetNode.Position - transform.position).magnitude > NodeProximity)
        {
            Vector3 newSpeed = _rigidBody.velocity;
            float SpeedY = _rigidBody.velocity.y;
            // _targetNode.Position.y = transform.position.y;   // I don't feel comfortable leaving this commented, but also it would change the Y position of the nodes. Make radius more generous?

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_targetNode.Position - transform.position).normalized), Time.fixedDeltaTime * TurnSpeed);

            newSpeed = transform.rotation * Vector3.forward * RunSpeed * _levelData.GetGlobalSpeedMultiplier();
            newSpeed.y = SpeedY;

            _rigidBody.velocity = newSpeed;

            yield return null;
        }

        _targetNode = _levelData.RandomNode();
        StartCoroutine(FollowNode());
    }

    public void StartRunning()
    {
        StartCoroutine(FollowNode());
    }
}
