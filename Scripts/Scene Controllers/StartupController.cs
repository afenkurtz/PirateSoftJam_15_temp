using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    [SerializeField] GameObject completeText;

    [SerializeField] Button[] sceneButtons;

    private void OnEnable()
    {
        Messenger<int, int>.AddListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.AddListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnDisable()
    {
        Messenger<int, int>.RemoveListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.RemoveListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnManagersProgress(int numReady, int numModules)
    {
        float progress = (float)numReady / numModules;
        progressBar.value = progress;
    }

    private void OnManagersStarted()
    {
        foreach (Button button in sceneButtons) {
            button.interactable = true;
        }

        completeText.SetActive(true);
    }


    /*
     * ------------------------------Load Scene Buttons------------------------------
     */
    public void LoadWorldMapScene()
    {
        Managers.Level.LoadSceneOnly(LevelNames.WorldMap);
    }

    public void LoadArea1Scene()
    {
        Managers.Level.LoadSceneAndPlayer(LevelNames.Area1);
    }
}
