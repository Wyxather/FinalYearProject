using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public class StatusValue
    {
        public float value;

        public float max;

        Image bar;

        public StatusValue(float value)
        {
            this.value = value;
            this.max = value;
        }

        public StatusValue(float value, float max)
        {
            this.value = value;
            this.max = max;
        }

        public void SetImage(Image image)
        {
            this.bar = image;
        }

        public void UpdateImageBar()
        {
            this.bar.fillAmount = value / max;
        }
    }

    [SerializeField]
    GameObject projectileObject;

    Rigidbody2D projectileRigidBody2D;

    protected SpriteRenderer spriteRenderer;

    protected Cannon cannon;

    protected float cannonAngle = 0f;

    protected bool isPlayer;

    bool isMyTurn;

    bool isShooting;

    protected StatusValue health = new StatusValue(100f);

    protected StatusValue stamina = new StatusValue(1f);

    protected StatusValue power = new StatusValue(0f, 20f);

    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        cannon = GetComponentInChildren<Cannon>();

        var canvas = GetComponentInChildren<Canvas>();
        foreach (var image in canvas.GetComponentsInChildren<Image>())
        {
            switch (image.name)
            {
            case "HealthAmount":
                health.SetImage(image);
                break;
            case "StaminaAmount":
                stamina.SetImage(image);
                break;
            case "PowerAmount":
                power.SetImage(image);
                break;
            default:
                break;
            }
        }
    }

    protected void Update()
    {
        UpdateImageBar();
    }

    public bool IsDead()
    {
        return health.value <= 0.0f;
    }

    public void IsMyTurn(bool value)
    {
        isMyTurn = value;
    }

    public bool IsMyTurn()
    {
        return isMyTurn;
    }

    public bool IsShooting()
    {
        return isShooting && projectileRigidBody2D != null;
    }

    public bool HasFinishShooting()
    {
        return isShooting && projectileRigidBody2D == null;
    }

    protected bool IsExhausted()
    {
        return stamina.value <= 0.0f;
    }

    public void OnNextTurn()
    {
        stamina.value = stamina.max;
        isShooting = false;
    }

    public void Damage(float value)
    {
        health.value -= value;
    }

    protected void Shoot()
    {
        var cannonTransformRotation = cannon.transform.rotation;
        var projectileSpawnAngle = cannonAngle;
        if (!cannon.FlipX())
            projectileSpawnAngle += 180f;
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, projectileSpawnAngle);
        var projectile = Instantiate(projectileObject, cannon.transform.position - cannon.transform.right,
                                     cannon.transform.rotation);
        projectileRigidBody2D = projectile.GetComponent<Rigidbody2D>();
        projectileRigidBody2D.AddForce(-cannon.transform.right * power.value, ForceMode2D.Impulse);
        cannon.transform.rotation = cannonTransformRotation;
        isShooting = true;
    }

    void UpdateImageBar()
    {
        health.UpdateImageBar();
        stamina.UpdateImageBar();
        power.UpdateImageBar();
    }

    public Vector3 GetProjectilePosition()
    {
        return projectileRigidBody2D.transform.localPosition;
    }
}
