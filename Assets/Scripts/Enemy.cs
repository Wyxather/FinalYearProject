using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    GameObject target;

    new void Start()
    {
        base.Start();
        isPlayer = false;
    }

    new void Update()
    {
        base.Update();

        if (IsDead())
            return;

        if (!IsMyTurn())
            return;

        waitForXSeconds -= Time.deltaTime;
        if (waitForXSeconds > 0)
            return;

        if (IsShooting())
            return;

        Shoot(target.transform.position, Random.Range(.5f, 1.5f));
    }
}