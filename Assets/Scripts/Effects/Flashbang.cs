// Script to be used in a Flashbang effect. Attach to Flashbang prefab.

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Flashbang : Effect
{

    public float FadeIn;
    public float FadeOut;

    public float MaxContrast;

    VolumeProfile postProcessVolume;
    ColorAdjustments colorAdjustments;

    private float _timer;

    void Awake()
    {
        postProcessVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>()?.profile;
        if (!postProcessVolume) throw new System.NullReferenceException(nameof(VolumeProfile));

        // get used volume parts
        if (!postProcessVolume.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));
    }

    public override void Execute()
    {
        base.Execute();
        StartCoroutine(FlashbangStartCoroutine());
    }

    public override void Cancel()
    {
        StopAllCoroutines();
        StartCoroutine(FlashbangVanishCoroutine());
    }

    IEnumerator FlashbangStartCoroutine()
    {
        Debug.Log("Flashbang start");

        _timer = 0f;
        while(_timer < FadeIn)
        {
            float Progress = _timer / FadeIn;

            colorAdjustments.saturation.Override(   Mathf.Lerp(0, -100f, Progress)       );
            // colorAdjustments.contrast.Override(     Mathf.Lerp(0, MaxContrast, Progress) );
            // colorAdjustments.postExposure.Override( Mathf.Lerp(0, 1f, Progress)          );

            _timer += Time.deltaTime;

            yield return null;
        }
        StartCoroutine(FlashbangVanishCoroutine());
    }

    IEnumerator FlashbangVanishCoroutine()
    {
        _timer = 0f;
        while (_timer < FadeOut)
        {
            float Progress = _timer / FadeOut;

            colorAdjustments.saturation.Override(Mathf.Lerp(-100f, 0, Progress));
            // colorAdjustments.contrast.Override(Mathf.Lerp(MaxContrast, 0, Progress));
            // colorAdjustments.postExposure.Override(Mathf.Lerp(1f, 0, Progress));

            _timer += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Flashbang end");
        gameObject.SetActive(false);
    }
}
