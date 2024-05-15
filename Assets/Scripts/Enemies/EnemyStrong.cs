using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class EnemyStrong : MonoBehaviour, IEnemy
{
    
    [SerializeField] private GameObject flashbang;

    private ArrayColor _colors = new();
    public SpriteRenderer spriteRenderer;

    public void OnDie()
    {
        Destroy(gameObject);
    }

    void IEnemy.OnReach()
    {
        GameObject.Find("Player").GetComponent<IPlayer>().TakeDamage();
        GameObject fb = Instantiate(flashbang);
        fb.transform.parent = transform.parent;
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

        int[] ColorsProto = new int[5];
        for(int i = 0; i < Random.Range(1, 4);  i++)
        {
            _colors.Add((GameColor)System.Enum.ToObject(typeof(GameColor), Random.Range(0, 3)));
        }

        spriteRenderer.color = _colors.toRGB();
    }

    void Update()
    {
    }
}
