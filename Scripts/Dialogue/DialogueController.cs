using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public void MakeChoice(int choiceIndex)
    {
        Managers.Dialogue.MakeChoice(choiceIndex);
    }
}
