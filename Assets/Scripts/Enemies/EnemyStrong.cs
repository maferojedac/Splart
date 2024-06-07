using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class EnemyStrong : MonoBehaviour, IEnemy
{
    [Header("Components")]
    public SpriteRenderer spriteRenderer;

    [Header("Effects")]
    public GameObject _damageExplosion;
    public GameObject _deathExplosion;

    public GameObject flashbang;

    [Header("Level communication")]
    public LevelData _levelData;

    [Header("Sound clips")]
    public AudioClip _spawn;
    public AudioClip _damage;
    public AudioClip _die;
    public AudioClip _reach;
    public AudioClip _resist;

    // priv

    private ArrayColor _colors = new();

    private bool _canBeDamaged;
    private float _punchDuration;

    private Rigidbody _rigidBody;

    private Vector3 _startScale;
    private float _timer;

    private int _originalColorCount;

    private EnemySoundManager _soundManager;

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
            bool didHit = _colors.Contains(color);

            if (didHit)
            {
                GameObject exp = Instantiate(_damageExplosion, transform.position, Quaternion.identity);
                ParticleSystem.MainModule colorAdjuster = exp.GetComponent<ParticleSystem>().main;
                colorAdjuster.startColor = ArrayColor.makeRGB(color);
            }

            _colors.Remove(color);
            spriteRenderer.color = _colors.toRGB();

            if (didHit)
                if (_colors.Count() == 0)
                    _soundManager.PlaySound(_die);
                else
                    _soundManager.PlaySound(_damage, 2f - (_colors.Count() * 1f / _originalColorCount));
            else
                _soundManager.PlaySound(_resist);

            if (_colors.Count() == 0)
                OnDie();
        }
    }

    void IEnemy.SetColor(ArrayColor startColor)
    {
        _colors = startColor;
        _originalColorCount = _colors.Count();
    }

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("Sprite not found!");
            Destroy(gameObject);
        }

        _soundManager = transform.parent.GetComponent<EnemySoundManager>();

        _soundManager.PlaySound(_spawn);

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
                _soundManager.PlaySound(_reach);

                GameObject fb = Instantiate(flashbang);
                fb.transform.parent = transform.parent;
            }
        }
        Destroy(gameObject);
    }
}
