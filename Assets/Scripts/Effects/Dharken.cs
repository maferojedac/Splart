using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dhakrken : Effect
{
    public float _duration;

    private List<LevelObject> _terrainSprites = new();

    private float _strength;

    public override void Execute()
    {
        base.Execute();

        if (_terrainSprites == null)
            return;

        UpdateTerrainSprites();

        foreach (var item in _terrainSprites)
        {
            item.Dharken(_duration);
        }

        gameObject.SetActive(false);
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
            if (item.gameObject.CompareTag("Far Background"))
            {
                _terrainSprites.Add(item.GetComponent<LevelObject>());
            }
        }
    }
}