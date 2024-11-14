using UnityEngine;

public class EnemyBlot : Enemy, IRusherEnemy
{
        public Splat splat;

    [Header("Blot Sound Effects")]
    public AudioClip _spawn;
    public AudioClip _reach;

    // priv

    private Transform _mainCam; // Update this thing pleaseeeeeeeee

    private Vector3 _startScale;
    private float _timer;

    void Start()
    {
        _mainCam = Camera.main.transform;
    }

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        _soundManager.PlaySound(_spawn);
    }

    public override void OnAttack()
    {
        if(GameObject.Find("Player").GetComponent<Player>().TakeDamage())
            CreateSplat();
    }

    public void OnReach(Vector3 dir)    // Update for precision PLEASE
    {
        _enemyState = EnemyState.Jump;
        _isVulnerable = false;

        dir.y = 0f;
        dir = dir.normalized * 20f;
        dir.y = 10f;

        _rigidBody.AddForce(dir, ForceMode.Impulse);
    }

    void CreateSplat()
    {
        if (_levelData._gameRunning)
        {
            _soundManager.PlaySound(_reach);

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
