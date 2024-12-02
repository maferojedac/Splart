using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBlend : Effect
{
    public Material _effectMaterial;
    public Material _defaultMaterial;

    public bool _affectAll;

    public float _duration;

    private List<LevelObject> _terrainSprites = new();

    private float _strength;

    public override void Execute()
    {
        base.Execute();

        if (_terrainSprites == null)
            return;

        _strength = 1f;
        _effectMaterial.SetFloat("_Strength", _strength);

        UpdateTerrainSprites();

        foreach (var item in _terrainSprites)
        {
            item.SetMaterial(_effectMaterial);
        }

        StartCoroutine(BlendMaterialCoroutine());
    }

    public override void Cancel()
    {
        // Override so that effect cannot be cut during application
    }

    private void UpdateTerrainSprites() // Function that updates the terrain sprites we're working with
    {
        _terrainSprites.Clear();

        Transform terrainSprites = GameObject.Find("TerrainSprites").transform;

        if (terrainSprites == null)
            return;

        foreach (Transform item in terrainSprites)
        {
            if (_affectAll || item.gameObject.CompareTag("Far Background"))
            {
                _terrainSprites.Add(item.GetComponent<LevelObject>());
            }
        }
    }

    IEnumerator BlendMaterialCoroutine()
    {
        yield return new WaitForSeconds(_duration);

        while (_strength > 0f)
        {
            _strength -= Time.deltaTime;
            _effectMaterial.SetFloat("_Strength", _strength);
            yield return null;
        }

        foreach(var item in _terrainSprites)
        {
            item.SetMaterial(_defaultMaterial);
        }

        gameObject.SetActive(false);
    }
}
