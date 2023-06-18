using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float transformRotationZOffset;

    [SerializeField]
    float damage;

    [SerializeField]
    float weight;

    [SerializeField]
    float explosionRadius;

    Rigidbody2D rigidBody2D;

    CircleCollider2D circleCollider2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
            Debug.LogError("Projectile is missing RigidBody2D component.");

        circleCollider2D = GetComponent<CircleCollider2D>();
        if (circleCollider2D == null)
            Debug.LogError("Projectile is missing CircleCollider2D component.");

        rigidBody2D.gravityScale = weight;
    }

    void Update()
    {
        RotateTransform();
    }

    void RotateTransform()
    {
        float angle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + transformRotationZOffset, Vector3.forward);
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetExplosionRadius()
    {
        return explosionRadius;
    }
}