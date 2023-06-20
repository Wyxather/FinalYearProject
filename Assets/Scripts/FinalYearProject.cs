using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalYearProject : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 60), "Settings");

        if (GUI.Button(new Rect(20, 40, 80, 20), "Main Menu"))
            LoadScene_Main();
    }

    public static void LoadScene_Main()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public static void LoadScene_Level()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
