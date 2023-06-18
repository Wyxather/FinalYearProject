using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float damage;

    CircleCollider2D circleCollider2D;

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        DamageEntity(collider2D.GetComponent<Character>());
        DestroyTerrain(collider2D.GetComponent<Ground>());
        Destroy(gameObject);
    }

    void DamageEntity(Character character)
    {
        if (character != null)
            character.DecreaseHealth(damage);
    }

    void DestroyTerrain(Ground ground)
    {
        if (ground != null)
            ground.Explode(circleCollider2D);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetRadius(float radius)
    {
        circleCollider2D.radius = radius;
    }
}