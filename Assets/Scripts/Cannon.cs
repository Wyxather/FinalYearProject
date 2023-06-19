using UnityEngine;

public class Cannon : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipX(bool value)
    {
        spriteRenderer.flipX = value;
    }

    public bool FlipX()
    {
        return spriteRenderer.flipX;
    }
}