using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float transformRotationZOffset;

    [SerializeField]
    float damage;

    Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
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
        return this.damage;
    }
}