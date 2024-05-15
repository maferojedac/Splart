using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{

    [SerializeField] private float ColorDelay;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalHSV;
    private Vector3 _grayscaleHSV;

    private float _time;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 hsv = Vector3.zero;
        Color.RGBToHSV(_spriteRenderer.color, out hsv.x, out hsv.y, out hsv.z) ;
        _originalHSV = hsv;
        hsv.y = 0;
        hsv.z = 1f;
        _grayscaleHSV = hsv;
        _spriteRenderer.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
    }

    void Update() 
    {
        _time += Time.deltaTime;
        if(_time > ColorDelay)
        {
            Vector3 hsv = Vector3.zero;
            hsv = Vector3.Lerp(_grayscaleHSV, _originalHSV, _time - ColorDelay);
            _spriteRenderer.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }
    }
}
