using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    /*
     * ------------------------------Interface------------------------------
     */
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("PlayerManager starting...");

        Status = ManagerStatus.Started;
    }

    /*
     * ------------------------------PlayerManager------------------------------
     */
    
}
