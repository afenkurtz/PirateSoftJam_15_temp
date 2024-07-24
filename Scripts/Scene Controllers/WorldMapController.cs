using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    public void LoadWorldMap()
    {
        StartCoroutine(LoadYourAsyncScene("WorldMap"));
    }
    
    public void LoadArea1Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area1"));
    }

    public void LoadArea2Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area2"));
    }

    public void LoadArea3Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area3"));
    }

    public void LoadArea4Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area4"));
    }

    public void LoadArea5Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area5"));
    }

    public void LoadArea6Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area6"));
    }

    public void LoadArea7Scene()
    {
        StartCoroutine(LoadYourAsyncScene("Area7"));
    }


    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
