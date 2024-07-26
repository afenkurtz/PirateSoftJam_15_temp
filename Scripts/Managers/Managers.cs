using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* ------------------------------Add Managers As Required Components------------------------------
*/
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(CheatsManager))]
[RequireComponent(typeof(DialogueManager))]
public class Managers : MonoBehaviour
{
    /*
     * ------------------------------Add Static Manager Properties------------------------------
     */
    public static PlayerManager Player { get; private set; }
    public static LevelManager Level { get; private set; }
    public static CheatsManager Cheats { get; private set; }
    public static DialogueManager Dialogue { get; private set; }

    private List<IGameManager> startSequence;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        /*
         * ------------------------------Get Manager Components------------------------------
         */
        Player = GetComponent<PlayerManager>();
        Level = GetComponent<LevelManager>();
        Cheats = GetComponent<CheatsManager>();
        Dialogue = GetComponent<DialogueManager>();

        startSequence = new List<IGameManager>();
        /*
         * ------------------------------Add Managers to startSequence------------------------------
         */
        startSequence.Add(Player);
        startSequence.Add(Level);
        startSequence.Add(Cheats);
        startSequence.Add(Dialogue);

        StartCoroutine(StartupManagers());
    }



    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = startSequence.Count;
        int numReady = 0;

        // Keep looping until all managers are started
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log($"Progress: {numReady}/{numModules}");
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
            }

            // Pause for one frame before checking again
            yield return null;
        }

        Debug.Log("All managers started up.");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }
}
