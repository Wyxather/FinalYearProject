using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalYearProject : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 60), "Settings");

        if (GUI.Button(new Rect(20, 40, 80, 20), "Main Menu"))
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public static void LoadLevel_001()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
