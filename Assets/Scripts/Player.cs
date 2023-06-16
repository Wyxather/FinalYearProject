using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {
    new void Start()
    {
        base.Start();
        isPlayer = true;
    }
}