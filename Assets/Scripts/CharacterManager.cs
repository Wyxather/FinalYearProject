using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnVictory;

    [SerializeField]
    UnityEvent OnGameOver;

    Character[] characters;

    float remainingTimeTillNextTurn;

    const float DURATION_TILL_NEXT_TURN = 20f;

    TextMeshProUGUI timerTextMeshProUGUI;

    void Start()
    {
        characters = GetComponentsInChildren<Character>();

        if (characters.Length != 0)
            characters[0].IsMyTurn(true);

        timerTextMeshProUGUI = GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

        ResetTimer();
    }

    void Update()
    {
        characters = GetComponentsInChildren<Character>();

        if (characters.Length == 1 && characters[0].IsPlayer())
        {
            OnVictory.Invoke();
            return;
        }

        foreach (var (character, index) in characters.Select((value, i) => (value, i)))
        {
            if (!character.IsDead())
            {
                if (character.IsMyTurn())
                {
                    UpdateTimer();

                    if (IsNextTurn() || (character.IsShooting() && !character.HasOnGoingProjectile()))
                    {
                        var nextCharacter = characters[GetNextCharacterIndex(index)];
                        ResetTimer();
                        NextTurn(character, nextCharacter);
                        FocusCameraOnCharacter(nextCharacter);
                    }
                    else
                    {
                        if (character.IsShooting() && character.HasOnGoingProjectile())
                            FocusCameraOnProjectile(character.GetProjectilePosition());
                        else
                            FocusCameraOnCharacter(character);
                    }
                }
            }
            else
            {
                if (character.IsMyTurn())
                {
                    var nextCharacter = characters[GetNextCharacterIndex(index)];
                    ResetTimer();
                    NextTurn(character, nextCharacter);
                    FocusCameraOnCharacter(nextCharacter);
                }

                Destroy(character.gameObject);

                if (character.IsPlayer())
                    OnGameOver.Invoke();
            }
        }
    }

    void FocusCameraOnCharacter(Character character)
    {
        Camera.main.transform.localPosition =
            Vector3.Lerp(Camera.main.transform.localPosition,
                         new Vector3(character.transform.localPosition.x, character.transform.localPosition.y,
                                     Camera.main.transform.localPosition.z),
                         5.0f * Time.deltaTime);
    }

    void FocusCameraOnProjectile(Vector3 position)
    {
        Camera.main.transform.localPosition = Vector3.Lerp(
            Camera.main.transform.localPosition,
            new Vector3(position.x, position.y, Camera.main.transform.localPosition.z), 5.0f * Time.deltaTime);
    }

    void UpdateTimer()
    {
        remainingTimeTillNextTurn -= Time.deltaTime;
        timerTextMeshProUGUI.text = Mathf.RoundToInt(remainingTimeTillNextTurn).ToString();
    }

    void ResetTimer()
    {
        remainingTimeTillNextTurn = DURATION_TILL_NEXT_TURN;
        timerTextMeshProUGUI.text = Mathf.RoundToInt(remainingTimeTillNextTurn).ToString();
    }

    bool IsNextTurn()
    {
        return remainingTimeTillNextTurn <= 0f;
    }

    void NextTurn(Character prevChar, Character nextChar)
    {
        prevChar.IsMyTurn(false);
        nextChar.IsMyTurn(true);
        nextChar.OnNextTurn();
    }

    int GetNextCharacterIndex(int index)
    {
        var nextCharacterIndex = index + 1;
        if (nextCharacterIndex >= characters.Length)
            nextCharacterIndex = 0;
        return nextCharacterIndex;
    }
}
