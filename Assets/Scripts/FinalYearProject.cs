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

    public static void LoadScene_Level2()
    {
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }

    public static void LoadScene_Level3()
    {
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }

    public static void LoadScene_FriedRice()
    {
        SceneManager.LoadScene("FriedRice", LoadSceneMode.Single);
    }

    public static void LoadScene_MixedRice()
    {
        SceneManager.LoadScene("MixedRice", LoadSceneMode.Single);
    }

    public static void LoadScene_Soto()
    {
        SceneManager.LoadScene("Soto", LoadSceneMode.Single);
    }
}
