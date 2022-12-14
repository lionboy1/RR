using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    //Reference Text Mesh Pro
    public TMPro.TMP_Text loadingText;
    public void LoadRRLevel(int sceneIndex)
    {
        //Loads while unity does other work.  Also allows feedback during the loading operation
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while(!operation.isDone)
        {
            //During the loading phase, unity goes from 0-0.9,
            //then for the activation, it goes from 0.9-1

            //Therefore, clamp the value and converts the operation to a 
            //seamless 0-1.
            float progress =  Mathf.Clamp01(operation.progress/.9f);
            slider.value = progress;
            loadingText.text = progress * 100f + "%";
            //Wait until the next frame before ocntinnuing
            yield return null;
        }
    }

    public void ExitRRGame()
    {
        Application.Quit();
    }
   
}
