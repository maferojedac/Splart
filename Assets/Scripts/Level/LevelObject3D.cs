using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject3D : MonoBehaviour
{
    [SerializeField] private float ColorDelay;
    [SerializeField] private Color MyColor;

    private MeshRenderer _meshRenderer;
    private Vector3 _originalHSV;
    private Vector3 _grayscaleHSV;

    private float _time;
    private Material _myMaterial;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _myMaterial = _meshRenderer.material;

        Vector3 hsv = Vector3.zero;
        Color.RGBToHSV(MyColor, out hsv.x, out hsv.y, out hsv.z); 
        _originalHSV = hsv;
        hsv.y = 0;
        hsv.z = 1f;
        _grayscaleHSV = hsv;
        _myMaterial.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > ColorDelay)
        {
            Vector3 hsv = Vector3.zero;
            hsv = Vector3.Lerp(_grayscaleHSV, _originalHSV, _time - ColorDelay);
            _myMaterial.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }
    }
}
