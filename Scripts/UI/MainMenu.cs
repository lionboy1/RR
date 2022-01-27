using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewRRGame()
    {
        //Fresh loading of the first scene. 
        SceneManager.LoadScene(1);
    }

    public void ExitRRGame()
    {
        Application.Quit();
    }
}
