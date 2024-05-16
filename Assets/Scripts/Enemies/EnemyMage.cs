using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : MonoBehaviour, IEnemy
{

    [SerializeField] Material skyboxMaterial;
    [SerializeField] Material skyboxMaterialNormal;
    private ArrayColor _colors = new();

    public Material _defaultMaterial;
    public Material _selectedMaterial;

    public SpriteRenderer spriteRenderer;

    private bool Selected;

    public void OnDie()
    {
        Debug.Log("Player beat me!");
        RenderSettings.skybox = skyboxMaterialNormal;
        Destroy(gameObject);
    }

    void IEnemy.OnReach(Vector3 dir)
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
        RenderSettings.skybox = skyboxMaterial;
    }

    void Update()
    {

    }
}
