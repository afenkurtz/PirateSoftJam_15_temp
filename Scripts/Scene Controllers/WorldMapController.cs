using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    public void LoadWorldMap()
    {
        Managers.Level.LoadSceneOnly(LevelNames.WorldMap);
    }
    
    public void LoadArea1Scene()
    {
        Managers.Level.LoadSceneAndPlayer(LevelNames.Area1);
    }

    public void LoadArea2Scene()
    {
        Debug.Log("Area not configured.");
    }

    public void LoadArea3Scene()
    {
        Debug.Log("Area not configured.");
    }

    public void LoadArea4Scene()
    {
        Debug.Log("Area not configured.");
    }

    public void LoadArea5Scene()
    {
        Debug.Log("Area not configured.");
    }

    public void LoadArea6Scene()
    {
        Debug.Log("Area not configured.");
    }

    public void LoadArea7Scene()
    {
        Debug.Log("Area not configured.");
    }
}
