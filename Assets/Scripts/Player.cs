using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    public Image healthBar;
    float health;
    public float maxHealth = 100.0f;
    public Image staminaBar;
    float stamina;
    public float maxStamina = 1.0f;


    void Start() {
        health = maxHealth;
        stamina = maxStamina;

        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach(var image in GetComponentInChildren<Canvas>().GetComponentsInChildren<Image>()) {
            if (image.name == "HealthAmount") {
                healthBar = image;
            } else if (image.name == "StaminAmount") {
                staminaBar = image;
            }
        }
    }

    void Update() {
        if (IsDead()) {
            return;
        }

        UpdateInput();
        UpdateUI();
    }

    bool IsDead() {
        return health <= 0.0f;
    }

    bool IsExhausted() {
        return stamina <= 0.0f;
    }

    void UpdateInput() {
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
}