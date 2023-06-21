using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    Texture2D serializedTexture2D;

    Texture2D texture2D;

    SpriteRenderer spriteRenderer;

    PolygonCollider2D polygonCollider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CreateTexture2D();
        CreateSprite();
        CreatePolygonCollider2D();
    }

    void CreateTexture2D()
    {
        texture2D = Instantiate(serializedTexture2D);
    }

    void CreateSprite()
    {
        spriteRenderer.sprite = Sprite.Create(
            texture2D,
            new Rect(
                0,
                0,
                texture2D.width,
                texture2D.height
            ),
            new Vector2(
                .5f,
                .5f
            ),
            60f
        );
    }

    void CreatePolygonCollider2D()
    {
        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
    }

    void DestroyPolygonCollider2D()
    {
        Destroy(polygonCollider2D);
    }

    public void Explode(CircleCollider2D circleCollider2D)
    {
        Explode(WorldToPixelCoordinates(circleCollider2D.bounds.center), circleCollider2D.radius);
    }

    void Explode(Vector2Int position, float radius)
    {
        ClearTexture(position, radius);
        CreateSprite();
        DestroyPolygonCollider2D();
        CreatePolygonCollider2D();
    }

    void ClearTexture(Vector2Int position, float radius)
    {
        var r = Mathf.RoundToInt(radius * spriteRenderer.sprite.texture.width / spriteRenderer.bounds.size.x);
        var r2 = r * r;
        for (var i = 0; i <= r; i++)
        {
            var d = Mathf.RoundToInt(Mathf.Sqrt(r2 - i * i));
            for (var j = 0; j <= d; j++)
            {
                var px = position.x + i;
                var nx = position.x - i;
                var py = position.y + j;
                var ny = position.y - j;
                texture2D.SetPixel(px, py, Color.clear);
                texture2D.SetPixel(nx, py, Color.clear);
                texture2D.SetPixel(px, ny, Color.clear);
                texture2D.SetPixel(nx, ny, Color.clear);
            }
        }
        texture2D.Apply();
    }

    Vector2Int WorldToPixelCoordinates(Vector2 positoin)
    {
        var pixelCoordinates = Vector2Int.zero;
        var dx = (positoin.x - transform.position.x);
        var dy = (positoin.y - transform.position.y);
        pixelCoordinates.x = Mathf.RoundToInt(.5f * spriteRenderer.sprite.texture.width + dx * (spriteRenderer.sprite.texture.width / spriteRenderer.bounds.size.x));
        pixelCoordinates.y = Mathf.RoundToInt(.5f * spriteRenderer.sprite.texture.height + dy * (spriteRenderer.sprite.texture.height / spriteRenderer.bounds.size.y));
        return pixelCoordinates;
    }
}
