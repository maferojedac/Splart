using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LevelObject : MonoBehaviour
{

    [SerializeField] private Direction ExitDirection;
    [SerializeField] private bool Decolor = true;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalHSV;
    private Vector3 _grayscaleHSV;

    private Vector3 _exitPosition;
    private Vector3 _atLevelPosition;

    private bool isPainted;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _atLevelPosition = transform.position;
        switch (ExitDirection)
        {
            case Direction.Left: _exitPosition = _atLevelPosition - new Vector3(200f, 0, 0); break;
            case Direction.Right: _exitPosition = _atLevelPosition + new Vector3(200f, 0, 0); break;
            case Direction.Up: _exitPosition = _atLevelPosition + new Vector3(0, 250f, 0); break;
            case Direction.Down: _exitPosition = _atLevelPosition - new Vector3(0, 250f, 0); break;
            default: _exitPosition = transform.position; break;
        }
        transform.position = _exitPosition;

        // Save both Grayscale and Colored HSVs
        Vector3 TempHSV = Vector3.zero;
        Color.RGBToHSV(_spriteRenderer.color, out TempHSV.x, out TempHSV.y, out TempHSV.z);
        _originalHSV = TempHSV;
        TempHSV.y = 0;
        _grayscaleHSV = TempHSV;
        _spriteRenderer.color = Color.HSVToRGB(TempHSV.x, TempHSV.y, TempHSV.z);
    }

    void OnEnable()
    {
        if (Decolor)
        {
            isPainted = false;
            _spriteRenderer.color = Color.HSVToRGB(_grayscaleHSV.x, _grayscaleHSV.y, _grayscaleHSV.z);
        }
    }

    public void SlideIn()
    {
        StartCoroutine(SlideInCoroutine());
    }

    public void SlideOut()
    {
        StartCoroutine(SlideOutCoroutine());
    }

    public void Paint()
    {
        isPainted = true;
        StartCoroutine(PaintCoroutine());
    }

    public void SetMaterial(Material material)
    {
        _spriteRenderer.material = material;
    }

    public void Dharken(float ExitTime)
    {
        StartCoroutine(DharkenCoroutine(ExitTime));
    }

    IEnumerator PaintCoroutine()
    {
        float timer = 0;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            Vector3 LerpedHSV = LerpedHSV = Vector3.Lerp(_grayscaleHSV, _originalHSV, timer);

            _spriteRenderer.color = Color.HSVToRGB(LerpedHSV.x, LerpedHSV.y, LerpedHSV.z);
            yield return null;
        }
    }

    IEnumerator SlideOutCoroutine()
    {
        transform.position = _atLevelPosition;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 3;
            transform.position = Vector3.Slerp(_atLevelPosition, _exitPosition, timer);
            yield return null;
        }
    }

    IEnumerator SlideInCoroutine()
    {
        transform.position = _exitPosition;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 3;
            transform.position = Vector3.Slerp(_exitPosition, _atLevelPosition, timer);
            yield return null;
        }
    }

    IEnumerator DharkenCoroutine(float ExitTime)
    {
        Vector3 ReturnColor;
        if (isPainted)
            ReturnColor = _originalHSV;
        else
            ReturnColor = _grayscaleHSV;

        Vector3 BlackHSV = new Vector3(ReturnColor.x, 0, 0);

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime / ExitTime;

            Vector3 LerpedHSV = Vector3.Lerp(BlackHSV, ReturnColor, timer);
            _spriteRenderer.color = Color.HSVToRGB(LerpedHSV.x, LerpedHSV.y, LerpedHSV.z);

            yield return null;
        }

    }
}
