using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IGameManager
{
    /*
     * ------------------------------Interface------------------------------
     */
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("LevelManager starting...");

        Status = ManagerStatus.Started;
    }

    /*
     * ------------------------------LevelManager------------------------------
     */
    [SerializeField] GameObject playerAndCameraPrefab;

    public void LoadSceneOnly(LevelNames sceneName)
    {
        StartCoroutine(LoadAsyncSceneOnly(sceneName));
    }

    public void LoadSceneAndPlayer(LevelNames sceneName)
    {
        StartCoroutine(LoadAsyncSceneAndPlayer(sceneName));
    }

    IEnumerator LoadAsyncSceneOnly(LevelNames m_Scene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene.ToString(), LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    IEnumerator LoadAsyncSceneAndPlayer(LevelNames m_Scene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene.ToString(), LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(Instantiate(playerAndCameraPrefab), SceneManager.GetSceneByName(m_Scene.ToString()));

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}

public enum LevelNames
{
    // Be sure to add scenes to the build settings
    WorldMap,
    Area1,
    Area1_Apartment,
    Area1_MayorsHouse
};
