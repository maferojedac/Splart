using System.Collections;
using UnityEngine;

public class EnemyCoin : Enemy
{
    // Caracteristicas que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public /*static*/ float RunSpeed;   // Upon discussion, it was decided that all enemies should share the same speed
    public float TurnSpeed;

    private MapNode _targetNode;     // Siguiente nodo del mapa

    [Header("Enemy Setup")]
    public float spawnTossSpeed = 5f;
    public float spawnTossSpeedY = 5f;

    [Header("Blot Sound Effects")]
    public AudioClip _spawn;

    private bool touchingGround;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        _soundManager.PlaySound(_spawn);

        Entity.DisableCollision(GetComponent<BoxCollider>());

        // Coin should only have one color.
        if (_colors.Count() == 0)
            _colors.Add((GameColor)Random.Range(0, 3));
        else
            _colors = new ArrayColor(_colors[0]);

        // Adjust coin to new color
        _spriteRenderer.color = _colors.toRGB();

         // Toss coin into the air
        StartCoroutine(CoinToss());
    }

    IEnumerator CoinToss()
    {
        yield return null;
        // Get random direction
        Vector3 randomDirection = new Vector3(Random.value - 0.5f, 0.5f, Random.value - 0.5f);
        randomDirection.Normalize();
        randomDirection.y = spawnTossSpeedY;

        _rigidBody.velocity = randomDirection * spawnTossSpeed;

        while (!touchingGround)
        {
            yield return null;
        }

        _targetNode = _levelData.RandomNode();
        StartRunning();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider.gameObject.layer == 6)  // Six for terrain layer
        {
            touchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == 6)  // Six for terrain layer
        {
            touchingGround = false;
        }
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator FollowNode()
    {
        while (true)
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
    }

    public void StartRunning()
    {
        StartCoroutine(FollowNode());
    }
}
