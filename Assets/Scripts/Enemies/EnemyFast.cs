using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour, IEnemy
{

    private ArrayColor _colors = new();
    public SpriteRenderer spriteRenderer;

    private Transform _mainCam;

    [SerializeField] Splat splat;

    public void OnDie()
    {
        Destroy(gameObject);
    }

    void IEnemy.OnReach()
    {
        GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage();

        Color splat_color = spriteRenderer.color;
        splat_color.a = 0.7f;
        splat.ChangeColor(splat_color);
        Splat splatobj = Instantiate(splat);
        splatobj.transform.parent = _mainCam;
        splatobj.transform.localPosition = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-1, 1.3f), 2.5f);
        splatobj.transform.parent = transform.parent;

        Destroy(gameObject);
    }

    void IEnemy.TakeDamage(GameColor color)
    {
        _colors.Remove(color);
        spriteRenderer.color = _colors.toRGB();

        if (_colors.Count() == 0)
            OnDie();
    }

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("Sprite not found!");
            Destroy(gameObject);
        }

        for(int i = 0; i < Random.Range(1, 4);  i++)
        {
            _colors.Add((GameColor)System.Enum.ToObject(typeof(GameColor), Random.Range(0, 5)));
        }

        spriteRenderer.color = _colors.toRGB();
        _mainCam = Camera.main.transform;
    }

    void Update()
    {
        
    }

    void CreateSplat()
    {

    }
}
