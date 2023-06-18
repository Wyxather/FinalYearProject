using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalYearProject : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 90), "Scenes");

        if (GUI.Button(new Rect(20, 40, 80, 20), "Main"))
            SceneManager.LoadScene("Main", LoadSceneMode.Single);

        if (GUI.Button(new Rect(20, 70, 80, 20), "Level"))
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
