using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Entity[] entities;
    float tick = 10.0f;
    int entityIndexInAction = 0;

    void Start()
    {
        entities = GetComponentsInChildren<Entity>();
        if (entities.Length != 0)
            entities[0].isInAction = true;
    }

    void Update()
    {
        entities = GetComponentsInChildren<Entity>();

        if (entities.Length == 0)
            return;

        tick -= Time.deltaTime;
        if (tick <= 0.0f)
        {
            tick = 10.0f;

            entityIndexInAction += 1;
            if (entityIndexInAction >= entities.Length)
                entityIndexInAction = 0;
            for (int index = 0; index < entities.Length; ++index)
                entities[index].isInAction = entityIndexInAction == index;

            entities[entityIndexInAction].RestoreStamina();
        }
        else
        {
            if (entities[entityIndexInAction].IsDead())
            {
                tick = 0f;
            }
        }

        Camera.main.transform.localPosition = Vector3.Lerp(
            Camera.main.transform.localPosition,
            new Vector3(
                entities[entityIndexInAction].transform.localPosition.x,
                entities[entityIndexInAction].transform.localPosition.y,
                Camera.main.transform.localPosition.z
            ),
            5.0f * Time.deltaTime
        );

        if (entities[entityIndexInAction].IsDead())
            Destroy(entities[entityIndexInAction].gameObject);
    }
}