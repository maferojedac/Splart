using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected ArrayColor _colors = new(); // Color array
    protected int _originalColorCount;    // Color count the droplet spawned with
    protected Color _originalColor;    // Original color

    protected bool _isVulnerable;

    [Header("Prefab Settings")]
    // Components
    protected Rigidbody _rigidBody;
    protected Animator _animator;
    protected EnemySoundManager _soundManager;
    protected FXPooling _fxPool;
    public SpriteRenderer _spriteRenderer;

    [Header("Level Communications")]
    public /*static*/ LevelData _levelData;

    [Header("General Sound Effects")]
    public AudioClip _spawn;
    public /*static*/ AudioClip _damage;
    public /*static*/ AudioClip _resist;
    public /*static*/ AudioClip _die;

    [Header("FX")]
    public /*static*/ GameObject _damageExplosion;
    public /*static*/ GameObject _deathExplosion;

    [Header("General Enemy Settings")]
    public int DefeatScore = 10;
    public bool CanDamage = true;
    public bool IgnoreColor = false;    // If ignorecolor is true, death has to be manually managed by class

    public EnemyState _enemyState;

    protected List<Coroutine> SubscribedCoroutines;

    void Awake()
    {
        if (_spriteRenderer == null)
            Debug.LogError("Enemy sprite not found!");

        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _fxPool = GameObject.Find("FX").GetComponent<FXPooling>();

        _isVulnerable = true;
    }

    public void SetSoundManager(EnemySoundManager soundManager)
    {
        _soundManager = soundManager;  
    }

    #region Behavior Script Communication
    public void TakeDamage(GameColor color)
    {
        if (_isVulnerable)
        {
            bool didHit = _colors.Contains(color) || IgnoreColor;
            if (didHit)
            {
                OnDamageTaken();

                Effect explosion = _fxPool.Spawn(_damageExplosion);
                explosion.SetColor(ArrayColor.makeRGB(color));
                explosion.SetPosition(transform.position);
            }

            _colors.Remove(color);
            if(_colors.Count() > 0 && !IgnoreColor)
                _spriteRenderer.color = _colors.toRGB();

            if (didHit)
                if (_colors.Count() == 0)
                    _soundManager.PlaySound(_die);
                else
                    _soundManager.PlaySound(_damage, 2f - (_colors.Count() * 1f / _originalColorCount));
            else
                _soundManager.PlaySound(_resist);

            if (_colors.Count() == 0)
                Kill();
        }
    }

    public virtual void Kill(bool ForceKill = false)
    {
        _enemyState = EnemyState.Die;
        _rigidBody.velocity = Vector3.zero;

        if (!ForceKill)
            _levelData.SumScore(DefeatScore * _originalColorCount);

        _animator.SetTrigger("Death");
    }
    #endregion

    #region Special Behaving

    public virtual void OnDamageTaken() { } // Overridable method to be called when damage is taken
    public virtual void OnAttack() { } // Overridable method to be called when enemy atatcks
    public virtual void OnDie() { } // Overridable method to be called when enemy atatcks

    #endregion

    #region Spawning Setup

    public void SetSpeed(Vector3 speed)
    {
        _rigidBody.velocity = speed;
        Debug.Log("For "+gameObject.name+", Received speed > " + speed + " > Applied speed > " + _rigidBody.velocity);
    }

    public void SetColor(ArrayColor startColor)
    {
        _colors = startColor;

        _originalColorCount = _colors.Count();
        _originalColor = _colors.toRGB();
    }

    public Color GetColor()
    {
        return _colors.toRGB();
    }

    public virtual void Spawn(Vector3 position)
    {
        gameObject.SetActive(true);
        _enemyState = EnemyState.Rush;

        _soundManager.PlaySound(_spawn);

        _isVulnerable = true;

        _animator.SetTrigger("Reset");

        _spriteRenderer.color = _colors.toRGB();

        transform.position = position;
    }

    #endregion

    #region Animation Event Functions
    public void OnDeathAnimationEnd()
    {
        Effect explosion = _fxPool.Spawn(_damageExplosion);
        explosion.SetColor(_originalColor);
        explosion.SetPosition(transform.position);

        OnDie();

        gameObject.SetActive(false);    
    }

    public void OnAttackAnimationEnd()
    {
        if (CanDamage) { 
            if(GameObject.Find("Player").GetComponent<Player>().TakeDamage())
                OnAttack();
        }
        else
            OnAttack();

        gameObject.SetActive(false);
    }
    #endregion
}

public enum EnemyState
{
    Rush,
    Attack,
    Die
}