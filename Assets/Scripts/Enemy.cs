using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    float initialVelocityMultiplierErrorThreshold = .25f;

    bool shouldMove;

    float moveDir;

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

        if (!hasBeginNextTurn)
        {
            shouldMove = Random.value >= .5f;
            if (shouldMove)
                moveDir = Random.value >= .5f ? 1f : -1f;
            hasBeginNextTurn = true;
        }

        if (shouldMove)
        {
            if (!IsExhausted())
            {
                Vector3 deltaPos = Vector3.right * moveDir * Time.deltaTime * 1.0f;
                transform.position += deltaPos;
                stamina.value -= Mathf.Abs(deltaPos.x) * Random.Range(1f, 2f);
                spriteRenderer.flipX = moveDir < 0;
                cannon.FlipX(spriteRenderer.flipX);
            }
            else
            {
                waitForXSeconds = 1f;
                shouldMove = false;
            }
        }
        else
        {
            var timeToReach = Random.Range(.5f, 1.5f);
            LookAt(target.transform.position, timeToReach);
            Shoot(target.transform.position, timeToReach, Random.Range(1f - initialVelocityMultiplierErrorThreshold, 1f + initialVelocityMultiplierErrorThreshold));
        }
    }
}