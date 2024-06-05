using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyFast : MonoBehaviour, IEnemy
{

    private ArrayColor _colors = new();
    public SpriteRenderer spriteRenderer;

    private Transform _mainCam;

    private bool _canBeDamaged;

    [SerializeField] Splat splat;
    [SerializeField] LevelData _levelData;

    public GameObject _damageExplosion;
    public GameObject _deathExplosion;

    private Rigidbody _rigidBody;

    public PlayerData playerData;

    private Vector3 _startScale;
    private float _timer;

    public void OnDie()
    {
        /*if (playerData.Booster_ScoreUpgrade >= 0)
        {
            int scoreMultiplier = playerData.Booster_ScoreUpgrade +1;
            _levelData.SumScore(100 * scoreMultiplier);
        }
        else*/
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
        dir = dir.normalized * 20f;
        dir.y = 10f;
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
        _mainCam = Camera.main.transform;
        _rigidBody = GetComponent<Rigidbody>();
        _canBeDamaged = true;
    }

    void Update()
    {
        if (!_canBeDamaged)
        {
            if(_rigidBody.velocity.y < 0)
            {
                SelfDestruct();
            }
        }
    }

    void SelfDestruct()
    {
        if (playerData.BoosterLife <= 0)
        {
            GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage();
            CreateSplat();
        }
        else    playerData.BoosterLife--;
        Destroy(gameObject);
    }

    IEnumerator DeathByDefeat()
    {
        _timer = 0f;
        _startScale = transform.localScale;

        while(_timer < 1f)
        {
            _timer += Time.deltaTime * 10f;
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, _timer);
            yield return null;
        }
        Instantiate(_deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void CreateSplat()
    {
        if (_levelData._gameRunning)
        {
            Color splat_color = _colors.toRGB();
            splat_color.a = 0.7f;
            splat.ChangeColor(splat_color);
            Splat splatobj = Instantiate(splat);
            splatobj.transform.parent = _mainCam;
            splatobj.transform.localPosition = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-1, 1.3f), 2.5f);
            splatobj.transform.parent = transform.parent;
        }
    }
}
