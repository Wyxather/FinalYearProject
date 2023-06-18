using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float transformRotationZOffset;

    [SerializeField]
    GameObject explosion;

    Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
            Debug.LogError("Projectile is missing RigidBody2D component.");
    }

    void Update()
    {
        RotateTransform();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void RotateTransform()
    {
        float angle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + transformRotationZOffset, Vector3.forward);
    }
}