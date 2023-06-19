using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
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
        foreach (var (character, index) in characters.Select((value, i) => (value, i)))
        {
            if (!character.IsDead())
            {
                if (character.IsMyTurn())
                {
                    UpdateTimer();

                    if (IsNextTurn())
                    {
                        var nextCharacter = characters[GetNextCharacterIndex(index)];
                        ResetTimer();
                        NextTurn(character, nextCharacter);
                        FocusCameraOnCharacter(nextCharacter);
                    }
                    else
                    {
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
