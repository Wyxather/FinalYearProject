using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
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

        public void SetImage(Image image)
        {
            this.bar = image;
        }

        public void UpdateBar()
        {
            this.bar.fillAmount = value / max;
        }
    }

    public class PowerBar
    {
        public bool active;
        public float value;
    }

    public Rigidbody2D projectile;

    Cannon cannon;

    SpriteRenderer spriteRenderer;

    public bool isPlayer;
    public bool isBot;
    public bool isInAction;
    public StatusValue health = new StatusValue(100.0f);
    public StatusValue stamina = new StatusValue(1.0f);
    public PowerBar power = new PowerBar();
    float angle = 0f;

    public void Start()
    {
        cannon = GetComponentInChildren<Cannon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        var canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            foreach (var image in canvas.GetComponentsInChildren<Image>())
            {
                if (image.name == "HealthAmount")
                    health.SetImage(image);
                else if (image.name == "StaminaAmount")
                    stamina.SetImage(image);
            }
        }
    }

    void Update()
    {
        UpdateUI();

        if (IsDead())
            return;
        if (!IsInAction())
            return;

        UpdateCannon();
        UpdateInput();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>() == null)
            return;
        var collider = collision.GetComponent<CircleCollider2D>();
        if (collider == null)
            return;
        health.value -= 30.0f;
        Destroy(collider.gameObject, .1f);
    }

    bool IsInAction()
    {
        return isInAction;
    }

    bool IsDead()
    {
        return health.value <= 0.0f;
    }

    bool IsExhausted()
    {
        return stamina.value <= 0.0f;
    }

    void Shoot()
    {
        var backup = cannon.transform.rotation;
        var ang = angle;
        if (!cannon.FlipX())
            ang += 180f;
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, ang);
        var p = Instantiate(projectile,
                            cannon.transform.position - cannon.transform.right,
                            cannon.transform.rotation);
        p.AddForce(-cannon.transform.right * power.value, ForceMode2D.Impulse);
        cannon.transform.rotation = backup;
    }

    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            power.value = 0.0f;
        if (Input.GetKey(KeyCode.Space))
            power.value += Time.deltaTime * 10.0f;
        if (Input.GetKeyUp(KeyCode.Space))
            Shoot();
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            if (!IsExhausted())
            {
                Vector3 deltaPos = Vector3.right * horizontal * Time.deltaTime * 1.0f;
                transform.position += deltaPos;
                stamina.value -= Mathf.Abs(deltaPos.x);
            }
            spriteRenderer.flipX = horizontal < 0;
            cannon.FlipX(spriteRenderer.flipX);
        }
    }

    void UpdateUI()
    {
        health.UpdateBar();
        stamina.UpdateBar();
    }

    void UpdateCannon()
    {
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        delta.Normalize();
        angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        if (cannon.FlipX())
            angle += 180f;
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}