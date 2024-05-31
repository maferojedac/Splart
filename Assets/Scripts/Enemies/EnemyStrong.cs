using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class EnemyStrong : MonoBehaviour, IEnemy
{
    
    [SerializeField] private GameObject flashbang;
    [SerializeField] private LevelData _levelData;

    private ArrayColor _colors = new();

    private bool _canBeDamaged;
    private float _punchDuration;

    private Rigidbody _rigidBody;
    public SpriteRenderer spriteRenderer;

    public PlayerData playerData;

    public void OnDie()
    {
        if (playerData.Booster_ScoreUpgrade >= 0)
        {
            int scoreMultiplier = playerData.Booster_ScoreUpgrade +1;
            _levelData.SumScore(100 * scoreMultiplier);
        }
        else
            _levelData.SumScore(100);
        Destroy(gameObject);
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

    void SelfDestruct()
    {
        if (_levelData._gameRunning)
        {
            if (playerData.BoosterLife <= 0)
            {
                GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage();
                GameObject fb = Instantiate(flashbang);
                fb.transform.parent = transform.parent;
            }else    playerData.BoosterLife--;
        }
        Destroy(gameObject);
    }
}
