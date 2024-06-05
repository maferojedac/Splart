using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : MonoBehaviour, IEnemy
{

    [SerializeField] GameObject Effect;
    [SerializeField] LevelData _levelData;

    private ArrayColor _colors = new();

    public SpriteRenderer spriteRenderer;
    public GameObject particleRenderer;

    private BoxCollider _boxCollider;
    private Rigidbody _rigidBody;

    public GameObject _damageExplosion;
    public GameObject _deathExplosion;

    public float _life;
    public bool _depleteLife;
    private bool _isDespawning;
    private int _nodeTeleport;
    private bool _isDoingNothing;

    private IEnumerator _actionCoroutine;

    private Vector3 _startScale;
    private float _timer;

    private Vector3 SizeOffset;

    private MapNode _nextNode;

    public void OnDie()
    {
        /*if (playerData.Booster_ScoreUpgrade >= 0)
        {
            int scoreMultiplier = playerData.Booster_ScoreUpgrade +1;
            _levelData.SumScore(100 * scoreMultiplier);
        }
        else*/
        _levelData.SumScore(50);
        StartCoroutine(DeathByDefeat());
    }

    Color IEnemy.GetColor()
    {
        return _colors.toRGB();
    }

    void IEnemy.OnReach(Vector3 dir)
    {
        Destroy(gameObject);
    }

    void IEnemy.TakeDamage(GameColor color)
    {
        if (_colors.Contains(color))
        {
            Debug.Log("Ouch!");
            _levelData.SumScore(10);

            _colors.Remove(color);
            spriteRenderer.color = _colors.toRGB();

            GameObject exp = Instantiate(_damageExplosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule colorAdjuster = exp.GetComponent<ParticleSystem>().main;
            colorAdjuster.startColor = ArrayColor.makeRGB(color);

            StopCoroutine(_actionCoroutine);

            _isDoingNothing = true;
            _life -= 7f;

            _actionCoroutine = PenalizedWait(1);
            StartCoroutine(_actionCoroutine);
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

        _life = 25f;
        _depleteLife = false;

        
        _nodeTeleport = Random.Range(2, 4);
        _isDoingNothing = true;
        _isDespawning = false;
        SizeOffset = new Vector3(0, 3f, 0);

        // color related
        _colors = new ArrayColor(GenerateColor());
        spriteRenderer.color = _colors.toRGB();

        // component related
        _boxCollider = GetComponent<BoxCollider>();
        _rigidBody = GetComponent<Rigidbody>();

        particleRenderer.SetActive(false);

        Entity.DisableCollision(GetComponent<Collider>());
    }

    void Update()
    {
        _rigidBody.velocity = Vector3.zero;
        if(_life > 0)
        {
            if (_depleteLife)
            {
                _life -= Time.deltaTime;
                if (_isDoingNothing)
                {
                    _actionCoroutine = ChargeAttack();
                    StartCoroutine(_actionCoroutine);
                }
            }
            else
            {
                if (_isDoingNothing)
                {
                    if (_nodeTeleport > 0)
                    {
                        _nextNode = _levelData.RandomNode();
                        _actionCoroutine = Teleport();
                        StartCoroutine(_actionCoroutine);
                    }
                    else
                    {
                        _depleteLife = true;
                    }
                }
            }
        }
        else
        {
            if (!_isDespawning && _isDoingNothing)
            {
                _isDespawning = true;

                StopCoroutine(_actionCoroutine);

                _actionCoroutine = Disappear();
                StartCoroutine(_actionCoroutine);
            }
        }
    }

    IEnumerator PenalizedWait(float seconds)
    {
        particleRenderer.SetActive(false);
        _boxCollider.enabled = false;    // can be hit when waiting
        _isDoingNothing = false;
        _depleteLife = true;

        yield return new WaitForSeconds(seconds);

        _nodeTeleport = Random.Range(2, 4);
        _depleteLife = false;
        _isDoingNothing = true;
    }

    IEnumerator Wait(float seconds)
    {
        particleRenderer.SetActive(false);
        particleRenderer.SetActive(false);
        _boxCollider.enabled = true;    // can be hit when waiting
        _isDoingNothing = false;

        yield return new WaitForSeconds(seconds); 

        _isDoingNothing = true;
    }

    IEnumerator ChargeAttack()
    {
        particleRenderer.SetActive(true);
        _boxCollider.enabled = true;    // can be hit when charging

        _isDoingNothing = false;

        yield return new WaitForSeconds(5f);
        Attack();

        _nodeTeleport = Random.Range(2, 4);
        // _isDoingNothing = true;
        _depleteLife = false;

        _actionCoroutine = Wait(1);
        StartCoroutine(_actionCoroutine);
    }

    IEnumerator Teleport()
    {
        particleRenderer.SetActive(false);
        _boxCollider.enabled = false;   // can't be hit when teleporting
        _isDoingNothing = false;
        _nodeTeleport--;

        _timer = 0;
        while (_timer < 2)
        {
            _timer += Time.deltaTime * 5f;
            transform.rotation = Quaternion.Euler(Vector3.Slerp(Vector3.zero,  new Vector3(0, 720, 0), _timer));
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.up, _timer);
            yield return null;
        }

        transform.position = _nextNode.Position + SizeOffset;

        _colors = new ArrayColor(GenerateColor());
        spriteRenderer.color = _colors.toRGB();

        _timer = 0;
        while (_timer < 2)
        {
            _timer += Time.deltaTime * 5f;
            transform.rotation = Quaternion.Euler(Vector3.Slerp(new Vector3(0, 720, 0), Vector3.zero, _timer));
            transform.localScale = Vector3.Lerp(Vector3.up, Vector3.one, _timer);
            yield return null;
        }

        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        _isDoingNothing = true;

        _actionCoroutine = Wait(1);
        StartCoroutine(_actionCoroutine);
    }

    IEnumerator Disappear()
    {
        particleRenderer.SetActive(false);
        _boxCollider.enabled = false;   // can't be hit when despawning
        _isDoingNothing = false;

        _timer = 0;
        while (_timer < 5)
        {
            _timer += Time.deltaTime * 5f;
            transform.rotation = Quaternion.Euler(Vector3.Slerp(Vector3.zero, new Vector3(0, 720, 0), _timer));
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.up, _timer);
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator DeathByDefeat()
    {
        _timer = 0f;
        _startScale = transform.localScale;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime * 5f;
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, _timer);
            yield return null;
        }
        Instantiate(_deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Attack()
    {
        GameObject fb = Instantiate(Effect);
        fb.transform.parent = transform.parent;
    }

    private GameColor GenerateColor()
    {
        return (GameColor)Random.Range(0, 5);
    }
}
