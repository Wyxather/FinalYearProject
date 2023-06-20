using UnityEngine;

public class Player : Character
{
    new void Start()
    {
        base.Start();
        isPlayer = true;
    }

    new void Update()
    {
        base.Update();

        if (IsDead())
            return;

        if (!IsMyTurn())
            return;

        if (IsShooting())
            return;

        UpdateCannonOnMouseClickPosition();
        UpdateInput();
    }

    void UpdateCannonOnMouseClickPosition()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var deltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            deltaPosition.Normalize();

            cannonAngle = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg;
            if (cannon.FlipX())
            {
                cannonAngle += 180f;
                if (cannonAngle >= 180f)
                    cannonAngle = Mathf.Clamp(cannonAngle, 270f, 360f);
                else
                    cannonAngle = Mathf.Clamp(cannonAngle, 0f, 90f);
                cannon.transform.rotation = Quaternion.Euler(0f, 0f, cannonAngle + 16f);
            }
            else
            {
                cannonAngle = Mathf.Clamp(cannonAngle, -90f, 90f);
                cannon.transform.rotation = Quaternion.Euler(0f, 0f, cannonAngle - 16f);
            }
        }
    }

    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            power.value = 0.0f;

        if (Input.GetKey(KeyCode.Space))
        {
            power.value += Time.deltaTime * 10.0f;
            power.value = Mathf.Clamp(power.value, 0f, power.max);
        }

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
}
