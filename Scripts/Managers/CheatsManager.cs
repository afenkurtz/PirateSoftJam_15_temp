using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsManager : MonoBehaviour, IGameManager
{
    /*
     * ------------------------------Interface------------------------------
     */
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("CheatsManager starting...");

        Status = ManagerStatus.Started;
    }

    /*
     * ------------------------------CheatsManager------------------------------
     */
    
}