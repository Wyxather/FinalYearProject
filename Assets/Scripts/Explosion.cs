using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float damage;

    CircleCollider2D circleCollider2D;

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        if (circleCollider2D == null)
            Debug.LogError("Explosion is missing CircleCollider2D component.");
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        DamageCharacter(collider2D.GetComponent<Character>());
        DestroyGround(collider2D.GetComponent<Ground>());
        Destroy(gameObject);
    }

    void DamageCharacter(Character character)
    {
        if (character != null)
            character.Damage(damage);
    }

    void DestroyGround(Ground ground)
    {
        if (ground != null)
            ground.Explode(circleCollider2D);
    }
}
