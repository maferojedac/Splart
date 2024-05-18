using System;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{

    public float BulletSpeed;

    public LevelData _levelData;

    public GameObject _bulletPrefab;
    private GameObject _heldBullet;
    private float _canRegretBullet;
    private bool _held;

    private LayerMask _entityMask;
    private LineRenderer _lineRenderer;
    private Vector3 _cameraPos;

    // public float CooldownTime;
    public int _HP;

    private bool _isActive;

    void IPlayer.NewGame()
    {
        Start();
    }

    void IPlayer.TakeDamage()
    {
        _HP -= 1;
        if(_HP < 1 && _isActive)
        {
            _isActive = false;
            _levelData.EndGame();
        }
    }

    void Start()
    {
        _entityMask = LayerMask.GetMask("Entity");
        _HP = 3;
        _held = false;
        _isActive = true;

        _lineRenderer = GetComponent<LineRenderer>();   
        _cameraPos = Camera.main.transform.position;
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
                pos.y = 150f;
                pos = Camera.main.ScreenToWorldPoint(pos);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                _heldBullet.transform.position = pos;
                if (Physics.Raycast(ray, out hit, 500f, _entityMask))
                {
                    _lineRenderer.SetPosition(0, _cameraPos - new Vector3(0, 1, 0));
                    _lineRenderer.SetPosition(1, hit.point);
                    _heldBullet.GetComponent<Bullet>()._target = hit.collider.gameObject;
                }
                else
                {
                    _lineRenderer.SetPosition(0, new Vector3(0, 0, 1));
                    _lineRenderer.SetPosition(1, new Vector3(0, 0, 1));
                    _heldBullet.GetComponent<Bullet>()._target = null;
                }
            }
            else
            {
                _lineRenderer.SetPosition(0, new Vector3(0, 0, 1));
                _lineRenderer.SetPosition(1, new Vector3(0, 0, 1));
                _heldBullet.GetComponent<Bullet>().Release();
                _heldBullet = null;
            }
        }
        if(Input.GetMouseButton(0))
            _held = true;
        else
            _held = false;
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
            _heldBullet = Instantiate(_bulletPrefab);
            _heldBullet.GetComponent<Bullet>()._color = color;
            _canRegretBullet = 0.2f;
            _held = true;
        }
    }
}
