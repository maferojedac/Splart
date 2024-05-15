using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Flashbang : MonoBehaviour
{

    public float FadeIn;
    public float FadeOut;

    public float MaxContrast;

    VolumeProfile postProcessVolume;
    ColorAdjustments colorAdjustments;

    private float _timer;

    void Start()
    {
        postProcessVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>()?.profile;
        if (!postProcessVolume) throw new System.NullReferenceException(nameof(VolumeProfile));

        // get used volume parts
        if (!postProcessVolume.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));

        colorAdjustments.saturation.Override(-100.0f);
    }


    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer < FadeIn)
        {
            colorAdjustments.saturation.Override(
                Mathf.Lerp(0, -100f, _timer / FadeIn)
                );
            colorAdjustments.contrast.Override(
                Mathf.Lerp(0, MaxContrast, _timer / FadeIn)
                );
            colorAdjustments.postExposure.Override(
                Mathf.Lerp(0, 1f, _timer / FadeIn)
                );
        }
        else
        {
            colorAdjustments.saturation.Override(
                Mathf.SmoothStep(-100f, 0, (_timer / FadeOut) - FadeIn)
                );
            colorAdjustments.contrast.Override(
                Mathf.Lerp(MaxContrast, 0, (_timer / FadeOut) - FadeIn)
                );
            colorAdjustments.postExposure.Override(
                Mathf.SmoothStep(1f, 0, (_timer / FadeOut) - FadeIn)
                );
            if (_timer  > FadeOut) 
                Destroy(gameObject);
        }
    }
}
