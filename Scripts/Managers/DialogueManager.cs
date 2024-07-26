using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;

/*
 * Most of this code is from the following: https://github.com/shapedbyrainstudios/ink-dialogue-system
 * Because my managers are initialized at startup and persist through all scenes with DontDestroyOnLoad, 
 * I had to rework how the UI works here. I made it a prefab and got the components from there
 * So that they can be instantiated when needed.
 */
public class DialogueManager : MonoBehaviour, IGameManager
{
    /*
     * ------------------------------Interface------------------------------
     */
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("DialogueManager starting...");

        DialogueIsPlaying = false;
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        canContinueToNextLine = false;

        Status = ManagerStatus.Started;
    }

    /*
     * ------------------------------DialogueManager------------------------------
     */
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] float typingSpeed = 0.04f;

    [SerializeField] TextAsset loadGlobalsJSON;

    public bool DialogueIsPlaying { get; private set; }

    private DialogueVariables dialogueVariables;

    private TextMeshProUGUI dialogueText;
    private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    private GameObject currentDialoguePanel;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine;
    private GameObject continueIcon;
    

    private void Update()
    {
        // return if dialogue isnt playing
        if (!DialogueIsPlaying)
        {
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 
            && (Input.GetKeyDown(KeyCode.Return) 
            || Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.Space)))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;

        GetInstanceValues();

        dialogueVariables.StartListening(currentStory);

        /*
        currentStory.BindExternalFunction("updateMoney", (int player_money) =>
        {
            Debug.Log(player_money);
        });
        */

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);

        // currentStory.UnbindExternalFunction("updateMoney");

        DialogueIsPlaying = false;
        Destroy(currentDialoguePanel);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            // handle case where the last line is an external function
            if (nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            // otherwise handle tags and display line as normal
            else
            {
                // handle tags here
                displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // set the text to the full line, but set the visible characters to be 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        // hide items while text is typing
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            // if not rich text, add the next letter and wait a small time
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        // actions to take after the entire line has finished displaying
        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void GetInstanceValues()
    {
        currentDialoguePanel = Instantiate(dialoguePanel);
        dialogueText = currentDialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        choices = GameObject.FindGameObjectsWithTag("DialogueChoices");
        continueIcon = GameObject.FindGameObjectWithTag("ContinueIcon");
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. /nNumber of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // enable and initizlize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure theyre hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null: " + variableName);
        }
        return variableValue;
    }



    // this method will allow of a variable defined in globals.ink to be set using C# code
    public void SetVariableState(string variableName, Ink.Runtime.Object variableValue)
    {
        if (dialogueVariables.variables.ContainsKey(variableName))
        {
            dialogueVariables.variables.Remove(variableName);
            dialogueVariables.variables.Add(variableName, variableValue);
        }
        else
        {
            Debug.LogWarning("Tried to update variable that wasn't initialized by globals.ink: " + variableName);
        }
    }

    /* HOWTO:
     * use StringValue, IntValue, etc depending on the type
     *
    // convert the variable into a Ink.Runtime.Object value
    bool pokemonChosen = false;
    Ink.Runtime.Object obj = new Ink.Runtime.BoolValue(pokemonChosen);
    // call the DialogueManager to set the variable in the globals dictionary
    Managers.Dialogue.SetVariableState("pokemon_chosen", obj);
     *
     */
}
