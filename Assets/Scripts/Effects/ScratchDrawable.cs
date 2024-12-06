// Script that takes charge of a scratchable surface's behavior as an effect.
// This script allows you to both draw and erase on the texture.
// This one does not lock the player's controls as well.
// Created by Javier Soto

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScratchDrawable : Effect
{
    private Image SpriteMask;
    private RectTransform CanvasRectTransform;

    [Range(0, 1)] public float ScratchTreshold = 0.3f;

    public float BrushSize = 30f;
    [Range(0, 1)] public float BrushPorosity = 0.3f;

    public float SpriteQualityLoss;

    private Texture2D _maskTexture;
    private Sprite _maskSprite;
    private Rect _maskRect;

    private int _textureHeight;
    private int _textureWidth;

    private int _pixelBrushSize;

    private int _pixelCount;
    private int _pixelMax;

    void Awake()
    {
        SpriteMask = GetComponent<Image>();

        Transform overlayCanvas = GameObject.Find("Overlaid Elements").transform;
        transform.SetParent(overlayCanvas);
        CanvasRectTransform = overlayCanvas.GetComponent<RectTransform>();

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.offsetMin = new Vector2(0, 0);
        transform.localScale = Vector3.one;

        _textureWidth =  Mathf.RoundToInt(Screen.width / SpriteQualityLoss);
        _textureHeight = Mathf.RoundToInt(Screen.height / SpriteQualityLoss);

        _pixelBrushSize = Mathf.RoundToInt(BrushSize / CanvasRectTransform.rect.height * _textureHeight);

        _maskRect = new Rect(0, 0, _textureWidth, _textureHeight);

        _maskTexture = CreateNewTexture();

        _maskSprite = Sprite.Create(_maskTexture, _maskRect, new Vector2(0.5f, 0.5f), _textureHeight);

        SpriteMask.sprite = _maskSprite;
    }

    public override void Execute()
    {
        base.Execute();

        ClearTexture();
    }

    // Function to be called when the player can scratch the screen
    public void EnablePlayerInteraction()
    {
        _pixelMax = _pixelCount;

        StartCoroutine(MakeScratchableScreen());
    }

    public override void Cancel()
    {
        base.Cancel();

        ClearTexture();
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        SpriteMask.color = _color;
    }

    IEnumerator MakeScratchableScreen()
    {       

        while (_pixelCount * 1f / _pixelMax > ScratchTreshold)
        {
            if (Input.GetMouseButton(0))
                DrawCircle(Input.mousePosition, _pixelBrushSize, true);

            yield return null;
        }

        StartCoroutine(VanishScratchableScreen());
    }

    IEnumerator VanishScratchableScreen()
    {
        float timer = 0f;
        while(timer < 1f)
        {
            _color.a = Mathf.SmoothStep(1f, 0, timer);
            SpriteMask.color = _color;

            timer += Time.deltaTime;
            yield return null;
        }

        ClearTexture();

        gameObject.SetActive(false);
    }

    private Texture2D CreateNewTexture()    // Create new texture2D
    {
        Texture2D texture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);

        return texture;
    }

    private void ClearTexture()
    {
        Color32[] pixels = new Color32[_textureWidth * _textureHeight];   // Array for colors for each pixel

        _pixelCount = 0;

        // Set info to texture
        _maskTexture.SetPixels32(pixels);
        _maskTexture.Apply();
    }

    public void DrawCircle(Vector2 Position, int PixelRadius, bool Erase)
    {
        Position /= SpriteQualityLoss;

        Color drawColor;
        if (Erase)
            drawColor = Color.clear;
        else
            drawColor = Color.white;

        Color32[] pixels = _maskTexture.GetPixels32();

        for (int posY = Mathf.Max(0, (int)(Position.y - PixelRadius));
                posY < Mathf.Min(_textureHeight, (int)(Position.y + PixelRadius));
                posY++)  // Start loop on Y
        {
            for (int posX = Mathf.Max(0, (int)(Position.x - PixelRadius));
            posX < Mathf.Min(_textureWidth, (int)(Position.x + PixelRadius));
            posX++) // Loop on X
            {
                if (Vector2.Distance(Position, new Vector2(posX, posY)) < PixelRadius)
                    if (Random.value < (1f - BrushPorosity))
                    {
                        if (Erase)
                        {
                            if ((pixels[posY * _textureWidth + posX] == Color.white))
                            {
                                _pixelCount--;
                            }
                        }
                        else
                        {
                            if ((pixels[posY * _textureWidth + posX] == Color.clear))
                            {
                                _pixelCount++;
                            }
                        }
                        pixels[posY * _textureWidth + posX] = drawColor;
                    }
            }
        }

        _maskTexture.SetPixels32(pixels);

        _maskTexture.Apply();
    }
}
