using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    void Start()
    {
        base.Start();
        isBot = true;
    }
}