using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    public Player() {
        this.isPlayer = true;
    }

    void Start() {
        Debug.Log("Player " + this + " started.");
    }

    void Update() {
        if (!isInAction) {
            return;
        }

        
    }
}
