using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour, IEnemy
{

    [SerializeField] Material skyboxMaterial;
    [SerializeField] Material skyboxMaterialNormal;
    private ArrayColor _colors = new();

    public SpriteRenderer spriteRenderer;

    public void OnDie()
    {
        Debug.Log("Player beat me!");
        RenderSettings.skybox = skyboxMaterialNormal;
        Destroy(gameObject);
    }

    void IEnemy.OnReach()
    {
        Debug.Log("Mage Reached");
        RenderSettings.skybox = skyboxMaterialNormal;
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
        RenderSettings.skybox = skyboxMaterial;
    }

    void Update()
    {
        
    }
}
