// Script for screen blots

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlot : Effect
{
    [SerializeField] float appearSpeed = 2f;
    [SerializeField] float vanishSpeed = 2f;
    [SerializeField] float alphaValue = 0.7f;
    [SerializeField] float margins = 100f;

    private Canvas overlayCanvas;
    private Image imageRenderer;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private float _timer;

    void Awake()
    {
        imageRenderer = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        overlayCanvas = GameObject.Find("Overlaid Elements").GetComponent<Canvas>();
        transform.SetParent(overlayCanvas.transform);

        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public override void SetColor(Color color)
    {
        color.a = alphaValue;
        imageRenderer.color = color;
    }

    public override void Execute()
    {
        base.Execute();
        rectTransform.anchoredPosition = new Vector2(
            Random.Range(- (parentRectTransform.rect.width / 2) + margins, (parentRectTransform.rect.width / 2) - margins), 
            Random.Range(- (parentRectTransform.rect.height / 2) + margins, (parentRectTransform.rect.height / 2) - margins));

        StartCoroutine(SpawnSplat());
    }

    public override void Cancel()
    {
        StartCoroutine(RemoveSplat());
    }

    IEnumerator SpawnSplat()
    {
        _timer = 0f;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime * appearSpeed;

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _timer);

            yield return null;
        }
    }

    IEnumerator RemoveSplat()
    {
        _timer = 0f;

        while(_timer < 1f)
        {
            _timer += Time.deltaTime * vanishSpeed;

            Color mycolor = imageRenderer.color;
            mycolor.a = 1 - _timer;
            imageRenderer.color = mycolor;

            yield return null;
        }

        gameObject.SetActive(false);
    }


}
