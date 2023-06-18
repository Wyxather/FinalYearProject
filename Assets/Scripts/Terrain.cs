using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public Texture2D baseTexture;
    Texture2D displayTexture;
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        displayTexture = Instantiate(baseTexture);
        displayTexture.alphaIsTransparency = true;
        if (displayTexture.format != TextureFormat.ARGB32)
            Debug.LogWarning("Texture format must be ARGB32!");
        if (displayTexture.wrapMode != TextureWrapMode.Clamp)
            Debug.LogWarning("Texture wrap mode must be clamp!");
        UpdateTexture();
        CreateCollider();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var projectile = collision.GetComponent<Projectile>();
        if (collision.GetComponent<Projectile>() == null)
            return;

        var collider = collision.GetComponent<CircleCollider2D>();
        if (collider == null)
            return;
            
        Explode(collider);
        Destroy(collision.gameObject, .02f);
    }

    void UpdateTexture()
    {
        spriteRenderer.sprite = Sprite.Create(
            displayTexture,
            new Rect(0, 0, displayTexture.width, displayTexture.height),
            new Vector2(.5f, .5f),
            50f
        );
    }

    void CreateCollider()
    {
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
    }

    void Explode(CircleCollider2D col)
    {
        Explode(col, col.bounds.size.x);
    }

    void Explode(CircleCollider2D collider, float radius)
    {
        Vector2Int c = WorldToPixelCoordinates(collider.bounds.center);
        int r = Mathf.RoundToInt(radius * spriteRenderer.sprite.texture.width / spriteRenderer.bounds.size.x);
        int r2 = r * r;
        for (int i = 0; i <= r; i++)
        {
            int d = Mathf.RoundToInt(Mathf.Sqrt(r2 - i * i));
            for (int j = 0; j <= d; j++)
            {
                int px = c.x + i;
                int nx = c.x - i;
                int py = c.y + j;
                int ny = c.y - j;
                displayTexture.SetPixel(px, py, Color.clear);
                displayTexture.SetPixel(nx, py, Color.clear);
                displayTexture.SetPixel(px, ny, Color.clear);
                displayTexture.SetPixel(nx, ny, Color.clear);
            }
        }
        displayTexture.Apply();
        UpdateTexture();
        Destroy(polygonCollider);
        CreateCollider();
    }

    Vector2Int WorldToPixelCoordinates(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;
        var dx = (pos.x - transform.position.x);
        var dy = (pos.y - transform.position.y);
        v.x = Mathf.RoundToInt(.5f * spriteRenderer.sprite.texture.width + dx * (spriteRenderer.sprite.texture.width / spriteRenderer.bounds.size.x));
        v.y = Mathf.RoundToInt(.5f * spriteRenderer.sprite.texture.height + dy * (spriteRenderer.sprite.texture.height / spriteRenderer.bounds.size.y));
        return v;
    }
}
