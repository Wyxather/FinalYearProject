using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public bool isPlayer = false;
    public bool isInAction = false;

    void Start() {
        Debug.Log("Entity " + this + " started.");
    }

    void Update() {
        if (!isInAction) {
            return;
        }
    }
}
