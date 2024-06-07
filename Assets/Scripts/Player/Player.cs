using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{

    public float BulletSpeed;

    public LevelData _levelData;
    public PlayerData _playerData;

    public GameObject _bulletPrefab;
    public Transform _bulletYRefTransform;
    private Bullet _heldBullet;
    private float _canRegretBullet;
    private bool _held;

    private LayerMask _entityMask;
    private LineRenderer _lineRenderer;
    private Vector3 _cameraPos;

    private float _bulletYRef;

    // public float CooldownTime;
    public int _HP;
    public int _Shields;

    public bool _isActive;

    public void NewGame()
    {
        _HP = 3;
        _Shields = _playerData.BoosterLife;
        if (_Shields > 2)
            _Shields = 2;
        _playerData.BoosterLife -= _Shields;
        _held = false;
        _isActive = true;
    }

    bool IPlayer.TakeDamage()
    {
        if(_Shields <= 0)
        {
            _HP--;
            if (_HP < 1 && _isActive)
            {
                _isActive = false;
                _levelData.GameOver();
            }
            return true;
        }
        else
        {
            _Shields--;
            return false;
        }
    }

    void Start()
    {
        _entityMask = LayerMask.GetMask("Entity");

        _lineRenderer = GetComponent<LineRenderer>();   
        _cameraPos = Camera.main.transform.position;

        _bulletYRef = _bulletYRefTransform.position.y;

        NewGame();
    }

    void Update()
    {
        if(_heldBullet != null)
        {
            _canRegretBullet -= Time.deltaTime;
            if (_held)
            {
                Vector3 pos = Input.mousePosition;
                pos.z = transform.position.z + 2f;
                pos.y = _bulletYRefTransform.position.y;
                pos = Camera.main.ScreenToWorldPoint(pos);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                _heldBullet.transform.position = pos;
                
                if (Physics.Raycast(ray, out hit, 500f, _entityMask))
                {
                    EnableLine();

                    UpdateLine(transform.position - new Vector3(0, 5f, 0), hit.transform.position, ArrayColor.makeRGB(_heldBullet._color), hit.collider.gameObject.GetComponent<IEnemy>().GetColor());
                    _heldBullet._target = hit.collider.gameObject;
                }
                else
                {
                    DisableLine();
                    _heldBullet._target = null;
                }
            }
            else
            {
                DisableLine();
                _heldBullet.Release();
                _heldBullet = null;
            }
        }
        if(Input.GetMouseButton(0))
            _held = true;
        else
            _held = false;
        
    }

    private void UpdateLine(Vector3 from,  Vector3 to, Color start, Color end)
    {
        _lineRenderer.SetPosition(0, from);
        _lineRenderer.SetPosition(1, to);
        _lineRenderer.endColor = end;
        _lineRenderer.startColor = start;
    }

    private void EnableLine()
    {
        _lineRenderer.enabled = true;
    }

    private void DisableLine()
    {
        _lineRenderer.enabled = false;
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
        if (_isActive)
        {
            if(_heldBullet != null)
                _heldBullet.Release();
            _heldBullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
            _heldBullet._color = color;
            _canRegretBullet = 0.2f;
            _held = true;
        }
    }
}
