using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class BW : MonoBehaviour, IEnemy
{
    /*
        UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.Vignette vignette;
        
        if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        
        vignette.intensity.Override(0.5f);
    */
    VolumeProfile postProcessVolume;
    ColorAdjustments colorAdjustments;
    private ArrayColor _colors = new();
    public SpriteRenderer spriteRenderer;

    public void OnDie()
    {
        Debug.Log("Player beat me!");
        Destroy(gameObject);
    }

    void IEnemy.OnReach()
    {
        Debug.Log("BW Reached");
        if(!postProcessVolume.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));
        
        colorAdjustments.saturation.Override(-100.0f);
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
        Debug.Log("BW Instantated");
        if (spriteRenderer == null)
        {
            Debug.Log("Sprite not found!");
            Destroy(gameObject);
        }
        
        postProcessVolume = GameObject.Find("PostProcessVolume").GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!postProcessVolume) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

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
