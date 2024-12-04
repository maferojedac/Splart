// Debug script to ensure RYBColor quality

using UnityEngine;

public class ColorTransformation : MonoBehaviour
{
    public Color input;

    private SpriteRenderer sprite;

    public float Red;
    public float Yellow;
    public float Blue;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // sprite.color = new RYBColor(input).toRGB();
        sprite.color = new RYBColor(Red, Yellow, Blue, 1).toRGB();
    }
}
 