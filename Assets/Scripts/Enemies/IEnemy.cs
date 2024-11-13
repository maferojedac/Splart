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
    protected EnemySoundManager _soundManager;
    public SpriteRenderer _spriteRenderer;

    [Header("Level Communications")]
    public /*static*/ LevelData _levelData;

    [Header("General Sound Effects")]
    public /*static*/ AudioClip _damage;
    public /*static*/ AudioClip _resist;
    public /*static*/ AudioClip _die;

    [Header("FX / SET TO POOLING THINGY !!!!!!!")]
    public static GameObject _damageExplosion;
    public static GameObject _deathExplosion;

    [Header("General Enemy Settings")]
    public int DefeatScore = 10;

    protected EnemyState _enemyState;

    void Awake()
    {
        if (_spriteRenderer == null)
            Debug.LogError("Enemy sprite not found!");

        _rigidBody = GetComponent<Rigidbody>();

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
            bool didHit = _colors.Contains(color);
            if (didHit)
            {
                OnDamageTaken();
                // GameObject exp = Instantiate(_damageExplosion, transform.position, Quaternion.identity);
                // ParticleSystem.MainModule colorAdjuster = exp.GetComponent<ParticleSystem>().main;
                // colorAdjuster.startColor = ArrayColor.makeRGB(color);
            }

            _colors.Remove(color);
            _spriteRenderer.color = _colors.toRGB();

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
    public virtual void OnDie()
    {
        _enemyState = EnemyState.Die;
        _levelData.SumScore(DefeatScore);
        // StartCoroutine(DeathByDefeat()); Changed to animation events!!!
    }

    #endregion

    #region Special Behaving

    public virtual void OnDamageTaken() { } // Overridable method to be called when damage is taken
    public virtual void OnAttack() { } // Overridable method to be called when enemy atatcks

    #endregion

    #region Spawning Setup

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

        _spriteRenderer.color = _colors.toRGB();

        transform.position = position;
    }

    #endregion

    #region Animation Event Functions
    public void OnDeathAnimationEnd()
    {
        //Instantiate(_deathExplosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);    
    }

    public void OnAttackAnimationEnd()
    {
        if(GameObject.Find("Player").GetComponent<Player>().TakeDamage())
            OnAttack();

        gameObject.SetActive(false);
    }
    #endregion
}

public enum EnemyState
{
    Rush,
    Jump,
    Attack,
    Die
}