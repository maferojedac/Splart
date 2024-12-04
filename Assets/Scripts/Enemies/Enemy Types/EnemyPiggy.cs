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

    private MapNode _targetNode;     // Siguiente nodo del mapa

    [Header("Enemy Setup")]
    public float verticalOffset = 50f;

    [Header("Piggy Sound Effects")]
    public AudioClip _spawn;
    public AudioClip _hitGround;

    private bool touchingGround;

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
        _soundManager.PlaySound(_spawn);

        Entity.DisableCollision(GetComponent<BoxCollider>());

        // Triplicate piggy life
        List<GameColor> duplicated = new List<GameColor>();

        foreach (GameColor color in _colors)
        {
            duplicated.Add(color);
        }

        _colors.Add(duplicated);
        _colors.Add(duplicated);

        // Adjust coin to new color
        _spriteRenderer.color = _colors.toRGB();

         // Drop from sky
        StartCoroutine(Drop());
    }
     
    IEnumerator Drop()
    {
        yield return null;
        _targetNode = _levelData.RandomNode();
        transform.position = _targetNode.Position + (verticalOffset * Vector3.up);

        touchingGround = false;

        while (!touchingGround)
        {
            yield return null;
        }

        _soundManager.PlaySound(_hitGround);
    }

    public override void OnDamageTaken()
    {
        base.OnDamageTaken();
        if (Random.value < Generosity)
        {
            Enemy coin = _enemyPooling.Spawn(CoinPrefab);
            coin.SetSoundManager(_soundManager);
            coin.Spawn(transform.position);
        }
    }

    public override void OnDie()
    {
        base.OnDie();
        _levelData.PaintObject();

        for(int i = 0; i < ExplodeCoinAmount; i++)
        {
            Enemy coin = _enemyPooling.Spawn(CoinPrefab);
            coin.SetSoundManager(_soundManager);
            coin.Spawn(transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == 6)
            touchingGround = true;
    }
}
