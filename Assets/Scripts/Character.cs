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

    protected float waitForXSeconds;

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
        return isShooting;
    }

    public bool HasOnGoingProjectile()
    {
        return projectileRigidBody2D != null;
    }

    protected bool IsExhausted()
    {
        return stamina.value <= 0.0f;
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }

    public void OnNextTurn()
    {
        stamina.value = stamina.max;
        isShooting = false;
        waitForXSeconds = 1f;
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
        Debug.Log($"Player Angle: {projectileSpawnAngle}");
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, projectileSpawnAngle);
        projectileRigidBody2D = Instantiate(projectileObject, cannon.transform.position - cannon.transform.right, cannon.transform.rotation).GetComponent<Rigidbody2D>();
        projectileRigidBody2D.AddForce(-cannon.transform.right * power.value, ForceMode2D.Impulse);
        cannon.transform.rotation = cannonTransformRotation;
        isShooting = true;
    }

    protected void Shoot(Vector2 targetPosition, float timeToReach, float initialVelocityMultiplier)
    {
        //https://stackoverflow.com/questions/42792320/moving-a-2d-physics-body-on-a-parabolic-path-with-initial-impulse-in-unity

        // Calculate the initial position of the projectile
        // Adjusted for the muzzle position
        float cannonRightMultiplier;
        if (!cannon.FlipX())
            cannonRightMultiplier = -1f;
        else
            cannonRightMultiplier = 1f;
        Vector2 initialPosition = cannon.transform.position - cannon.transform.right * cannonRightMultiplier;

        // Calculate the direction towards the player
        Vector2 direction = targetPosition - initialPosition;

        // Spawn projectile with specified initial speed
        projectileRigidBody2D = Instantiate(projectileObject, initialPosition, Quaternion.identity).GetComponent<Rigidbody2D>();
        projectileRigidBody2D.velocity = new Vector2(
            direction.x / timeToReach,
            (direction.y - .5f * Physics2D.gravity.y * projectileRigidBody2D.gravityScale * timeToReach * timeToReach) / timeToReach
        ) * initialVelocityMultiplier;

        isShooting = true;

        // Calculate the angle between the enemy and the player
        float angle = Mathf.Atan2(projectileRigidBody2D.velocity.y, projectileRigidBody2D.velocity.x) * Mathf.Rad2Deg;

        // Rotate cannon and sprite
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            cannon.FlipX(true);
            cannon.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f + 16f);
        }
        else
        {
            spriteRenderer.flipX = false;
            cannon.FlipX(false);
            cannon.transform.rotation = Quaternion.Euler(0f, 0f, angle - 16f);
        }

        Debug.Log($"Cannon FlipX: {cannon.FlipX()}, Angle: {angle}, Cannon Rot: {cannon.transform.rotation}");
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

    public void LoadPrefab_FryingPan_Projectile()
    {
        projectileObject = Resources.Load("Prefabs/FryingPan_Projectile") as GameObject;
    }

    public void LoadPrefab_FriedRice_Projectile()
    {
        projectileObject = Resources.Load("Prefabs/FriedRice_Projectile") as GameObject;
    }

    public void LoadPrefab_MixedRice_Projectile()
    {
        projectileObject = Resources.Load("Prefabs/MixedRice_Projectile") as GameObject;
    }

    public void LoadPrefab_Soto_Projectile()
    {
        projectileObject = Resources.Load("Prefabs/Soto_Projectile") as GameObject;
    }

}
