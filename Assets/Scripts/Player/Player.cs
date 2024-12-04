// Script for player

using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // Communication objects
    private LevelData _levelData;
    private PlayerData _playerData;

    [Header("Player Setup")]
    public GameObject _bulletPrefab;    // Bullet prefab
    public float ScreenPercentageError;

    public Transform _bulletYRefTransform;  // Y value at which bullets should be spawned

    // Is bullet held and which bullet is held
    private Ally _heldBullet;
    private bool _held;

    private Camera _mainCam;

    private LayerMask _entityMask;  // Layer with which raycasts can interact

    private AllyPooling _allyPooler;    // Bullet pooling
    private EnemyPooling _enemyPooler;  // Enemy pooler

    [Header("Player State")]
    public int _HP;
    public bool _ShieldActive;
    // public int _Shields;     // Shield rework pending!

    public bool _isActive;
    public bool _controlLocked;

    public void NewGame()
    {
        _HP = 3;
        // _Shields = _playerData.BoosterLife;
        // if (_Shields > 2)
        //     _Shields = 2;
        // _playerData.BoosterLife -= _Shields;
        _held = false;
        _isActive = true;
        _controlLocked = false;
    }

    public bool TakeDamage()    // Function returns a bool value so that the attacker can react accordingly
    {
        if(_ShieldActive)
        {
            _ShieldActive = false;
            return false;
        }
        else
        {
            _HP--;
            if (_HP < 1 && _isActive)
            {
                _isActive = false;
                _levelData.GameOver();
            }
            return true;
        }
    }

    void Awake()
    {
        _levelData = transform.parent.GetComponent<PlayerManager>()._levelData;
        _playerData = transform.parent.GetComponent<PlayerManager>()._playerData;

        _allyPooler = GameObject.Find("Allies").GetComponent<AllyPooling>();
        _enemyPooler = GameObject.Find("Enemies").GetComponent<EnemyPooling>();

        _mainCam = Camera.main;

        _entityMask = LayerMask.GetMask("Entity");
    }

    void OnEnable() // Reset values for new games
    {
        NewGame();
    }

    void Update()
    {
        if(_heldBullet != null)
        {
            if (_held)
            {
                Vector3 pos = Input.mousePosition;

                GameObject closestEnemy = GetClosestEnemy(pos);

                _heldBullet.SetTarget(closestEnemy);

                // Transformations to pointer for proper bullet positioning
                pos.z = transform.position.z + 2f;
                pos.y = _bulletYRefTransform.position.y;

                pos = Camera.main.ScreenToWorldPoint(pos);

                _heldBullet.transform.position = pos;
            }
            else
            {
                _heldBullet.Release();
                _heldBullet = null;
            }
        }

        if(Input.GetMouseButton(0))
            _held = true;
        else
            _held = false;
    }

    private GameObject GetClosestEnemy(Vector2 ScreenPosition)
    {
        List<Enemy> enemies = _enemyPooler.GetAllEnemies();

        if (enemies.Count == 0)
            return null;

        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;
        float Radius = Mathf.Min(Screen.width, Screen.height) * ScreenPercentageError;

        foreach (Enemy enemy in enemies)
        {
            Vector2 EnemyPosition = _mainCam.WorldToScreenPoint(enemy.transform.position);

            float CurrentDistance = Vector2.Distance(EnemyPosition, ScreenPosition);

            if(CurrentDistance < closestDistance && CurrentDistance < Radius)
            {
                closestDistance = CurrentDistance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy == null)
            return null;

        return closestEnemy.gameObject;
    }

    public void SelectRed()
    {
        CreateBullet(GameColor.Red);
    }

    public void SelectYellow()
    {
        CreateBullet(GameColor.Yellow);
    }

    public void SelectBlue()
    {
        CreateBullet(GameColor.Blue);
    }

    public void SelectWhite()
    {
        CreateBullet(GameColor.White);
    }

    public void SelectBlack()
    {
        CreateBullet(GameColor.Black);
    }

    private void CreateBullet(GameColor color)
    {
        if (_isActive && !_controlLocked)
        {
            if(_heldBullet != null)
                _heldBullet.Release();

            _heldBullet = _allyPooler.Spawn(_bulletPrefab);
            _heldBullet.SetColor(color);
            _heldBullet.gameObject.SetActive(true);
            _held = true;
        }
    }
}
