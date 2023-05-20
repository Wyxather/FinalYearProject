using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private List<GameObject> entities = new List<GameObject>();
    private int currentEntityInAction = 0;
    private float remainingActionTime = 10.0f;
    private Text gameActionTimer;

    void Start() {
        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        entities.Add(
            Instantiate(
                player, 
                new Vector3(), 
                new Quaternion()
            )
        );

        GameObject entity = Resources.Load<GameObject>("Prefabs/Entity");
        for (int i = 0; i < 1; ++i) {
            entities.Add(
                Instantiate(
                    entity, 
                    new Vector3(
                        Random.Range(-5.0f, 5.0f),
                        Random.Range(1.0f, 5.0f),
                        0.0f
                    ), 
                    new Quaternion()
                )
            );
        }

        gameActionTimer = GameObject
        .FindObjectOfType<GameTimer>()
        .GetComponent<Text>();

        Debug.Log("GameManager started.");
    }

    void Update() {
        remainingActionTime -= Time.deltaTime;
        if (remainingActionTime <= 0.0f) {
            remainingActionTime = 10.0f;
            currentEntityInAction += 1;
            if (currentEntityInAction >= entities.Count) {
                currentEntityInAction = 0;
            }
        }

        gameActionTimer.text = Mathf
        .RoundToInt(remainingActionTime)
        .ToString() + " Sec";

        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position, 
            new Vector3(
                entities[currentEntityInAction].transform.position.x, 
                entities[currentEntityInAction].transform.position.y, 
                Camera.main.transform.position.z
            ),
            5.0f * Time.deltaTime
        );

        for (int i = 0; i < entities.Count; ++i) {
            Entity entity = entities[currentEntityInAction]
            .GetComponent<Entity>();

            if (i == currentEntityInAction) {
                entity.isInAction = true;
            } else {
                entity.isInAction = false;
            }
        }
    }
}