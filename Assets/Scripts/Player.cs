using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {
    new void Start()
    {
        base.Start();
        isPlayer = true;
    }
}