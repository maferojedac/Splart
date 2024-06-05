using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class EnemyStrong : MonoBehaviour, IEnemy
{
    
    [SerializeField] private GameObject flashbang;
    [SerializeField] private LevelData _levelData;

    public GameObject _damageExplosion;
    public GameObject _deathExplosion;


    private ArrayColor _colors = new();

    private bool _canBeDamaged;
    private float _punchDuration;

    private Rigidbody _rigidBody;
    public SpriteRenderer spriteRenderer;

    private Vector3 _startScale;
    private float _timer;

    public void OnDie()
    {
        _levelData.SumScore(10);
        StartCoroutine(DeathByDefeat());
    }

    Color IEnemy.GetColor()
    {
        return _colors.toRGB();
    }

    void IEnemy.OnReach(Vector3 dir)
    {
        _canBeDamaged = false;
        dir.y = 0f;
        dir = dir.normalized * 25f;
        dir.y = 5f;
        _rigidBody.AddForce(dir, ForceMode.Impulse);
    }

    void IEnemy.TakeDamage(GameColor color)
    {
        if(_canBeDamaged)
        {
            if (_colors.Contains(color))
            {
                GameObject exp = Instantiate(_damageExplosion, transform.position, Quaternion.identity);
                ParticleSystem.MainModule colorAdjuster = exp.GetComponent<ParticleSystem>().main;
                colorAdjuster.startColor = ArrayColor.makeRGB(color);
            }

            _colors.Remove(color);
            spriteRenderer.color = _colors.toRGB();

            if (_colors.Count() == 0)
                OnDie();
        }
    }

    void IEnemy.SetColor(ArrayColor startColor)
    {
        _colors = startColor;
    }

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("Sprite not found!");
            Destroy(gameObject);
        }

        spriteRenderer.color = _colors.toRGB();
        _rigidBody = GetComponent<Rigidbody>();
        _canBeDamaged = true;
        _punchDuration = 0f;
    }

    void Update()
    {
        if (!_canBeDamaged)
        {
            _punchDuration += Time.deltaTime;
            if (_rigidBody.velocity.y < 0)
            {
                SelfDestruct();
            }
        }
    }
    IEnumerator DeathByDefeat()
    {
        _timer = 0f;
        _startScale = transform.localScale;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime * 10f;
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, _timer);
            yield return null;
        }
        Instantiate(_deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void SelfDestruct()
    {
        if (_levelData._gameRunning)
        {
            if (GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage())
            {
                GameObject fb = Instantiate(flashbang);
                fb.transform.parent = transform.parent;
            }
        }
        Destroy(gameObject);
    }
}
