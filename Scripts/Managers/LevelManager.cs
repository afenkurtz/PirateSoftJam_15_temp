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

    [SerializeField] GameObject crossfadePrefab;
    [SerializeField] float transitionTime = 1f;

    

    private GameObject currentPlayerInstance;

    private GameObject currentCrossfadeInstance;
    private GameObject nextSceneCrossfadeInstance;

    /*
     * ------------------------------Public Methods------------------------------
     */

    public void LoadSceneOnly(LevelNames sceneName)
    {
        StartCoroutine(LoadAsyncSceneOnly(sceneName));
    }


    public void LoadSceneAndPlayer(LevelNames sceneName)
    {
        StartCoroutine(LoadAsyncSceneAndPlayer(sceneName));
    }

    public void LoadScene_Area1_OutsideApartment()
    {
        StartCoroutine(LoadAsyncSceneAndPlayer(LevelNames.Area1));
    }


    public void TeleportPlayer(Transform target)
    {
        PlayerController controller = currentPlayerInstance.GetComponentInChildren<PlayerController>();
        controller.TeleportPlayerTo(target);
    }

    /*
     * ------------------------------Private Methods------------------------------
     */

    IEnumerator LoadAsyncSceneOnly(LevelNames m_Scene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        LoadCrossfader();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene.ToString(), LoadSceneMode.Additive);

        LoadNextCrossfader(m_Scene);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);

        yield return new WaitForSeconds(transitionTime);

        DestroyNextCrossfader();
    }


    IEnumerator LoadAsyncSceneAndPlayer(LevelNames m_Scene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        LoadCrossfader();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene.ToString(), LoadSceneMode.Additive);

        LoadNextCrossfader(m_Scene);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        currentPlayerInstance = Instantiate(playerAndCameraPrefab);

        // Move the GameObject to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(currentPlayerInstance, SceneManager.GetSceneByName(m_Scene.ToString()));

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);

        yield return new WaitForSeconds(transitionTime);

        DestroyNextCrossfader();
    }


    private void LoadCrossfader()
    {
        currentCrossfadeInstance = Instantiate(crossfadePrefab);
        currentCrossfadeInstance.SetActive(true);
        Animator currentAnimator = currentCrossfadeInstance.GetComponentInChildren<Animator>();
        currentAnimator.SetTrigger("Start");
    }


    private void LoadNextCrossfader(LevelNames m_Scene)
    {
        nextSceneCrossfadeInstance = Instantiate(crossfadePrefab);
        nextSceneCrossfadeInstance.SetActive(true);
        SceneManager.MoveGameObjectToScene(nextSceneCrossfadeInstance, SceneManager.GetSceneByName(m_Scene.ToString()));
    }


    private void DestroyNextCrossfader()
    {
        Destroy(nextSceneCrossfadeInstance);
    }
}

public enum LevelNames
{
    // Be sure to add scenes to the build settings
    WorldMap,
    Area1,
    Area1_Apartment_IntroOnly,
    Area1_Apartment,
    Area1_MayorsHouse
};
