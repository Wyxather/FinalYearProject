using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Rigidbody2D projectile;

    Cannon cannon;

    SpriteRenderer spriteRenderer;

    Image healthBar;
    float health;
    public float maxHealth = 100.0f;

    Image staminaBar;
    float stamina;
    public float maxStamina = 1.0f;

    bool poweringUp = false;
    float power = 1.0f;

    void Start() {
        health = maxHealth;
        stamina = maxStamina;
        
        cannon = GetComponentInChildren<Cannon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach(var image in GetComponentInChildren<Canvas>().GetComponentsInChildren<Image>()) {
            if (image.name == "HealthAmount") {
                healthBar = image;
            } else if (image.name == "StaminaAmount") {
                staminaBar = image;
            }
        }
    }

    void Update() {
        UpdateUI();
        
        if (IsDead()) {
            return;
        }

        UpdateCannon();
        UpdateInput();
    }

    bool IsDead() {
        return health <= 0.0f;
    }

    bool IsExhausted() {
        return stamina <= 0.0f;
    }

    void Shoot() {
        var p = Instantiate(projectile, 
                            cannon.transform.position - cannon.transform.right, 
                            cannon.transform.rotation);
        p.AddForce(-cannon.transform.right * power, ForceMode2D.Impulse);
        Debug.Log(p);
    }

    void UpdateInput() {
        if (Input.GetKeyDown(KeyCode.Space))  {
            power = 0.0f;
        }
        if (Input.GetKey(KeyCode.Space)) {
            power += Time.deltaTime * 10.0f;
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            Shoot();
        }

        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal == 0) {
        } else {
            if (!IsExhausted()) {
                Vector3 deltaPos = Vector3.right * horizontal * Time.deltaTime * 1.0f;
                transform.position += deltaPos;
                stamina -= Mathf.Abs(deltaPos.x);
            }
            spriteRenderer.flipX = horizontal > 0;
        }
    }

    void UpdateUI() {
        healthBar.fillAmount = health / maxHealth;
        staminaBar.fillAmount = stamina / maxStamina;
    }

    void UpdateCannon() {
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        delta.Normalize();
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }
}