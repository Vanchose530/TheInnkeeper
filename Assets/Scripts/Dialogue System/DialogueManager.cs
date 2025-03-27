using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    private Animator layoutAnimator;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    private string followingDialogueQuestId;
    private QuestState? followingDialogueQuestState = null;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private ServeGuestStep serveGuestStep;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one \"DialogueManager\" class");
        }
        instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (var choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && InputManager.instance.GetInteractPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        displayNameText.text = "???";
        // !!! установить базовое значение для арта !!!
        layoutAnimator.Play("text_name_right");

        ContinueStory();
    }

    public void EnterDialogueMode(TextAsset inkJSON, string questId, QuestState questState)
    {
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // обрабатываем наличие квеста в диалоге
        followingDialogueQuestId = questId;
        followingDialogueQuestState = questState;

        currentStory.BindExternalFunction("start_quest",
            () => GameEventsManager.instance.questEvents.StartQuest(followingDialogueQuestId));
        currentStory.BindExternalFunction("finish_quest",
            () => GameEventsManager.instance.questEvents.FinishQuest(followingDialogueQuestId));

        currentStory.variablesState["quest_state"] = (int) followingDialogueQuestState;

        SetServeGuestStep(questId); // проверяем является ли квест квестом на обслуживание и если является обрабатываем
        if (serveGuestStep != null)
        {
            serveGuestStep.isOnDialogue = true;
            currentStory.variablesState["can_serve"] = (serveGuestStep.isOnDialogue && serveGuestStep.CheckWantDishes()); // по задумке передаётся serveGuestStep.canServe но что то идёт не так
            Debug.Log("Can Serve: " + serveGuestStep.canServe + ". IsOnDialogue: " + serveGuestStep.isOnDialogue + ". CheckWantDishes(): " + serveGuestStep.CheckWantDishes());
            currentStory.BindExternalFunction("serve_guest",
            () => GameEventsManager.instance.questEvents.ServeGuest());
        }

            displayNameText.text = "???";
        // !!! установить базовое значение для арта !!!
        layoutAnimator.Play("text_name_right");

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (followingDialogueQuestId != null || followingDialogueQuestState != null)
        {
            followingDialogueQuestId = null;
            followingDialogueQuestState = null;
            currentStory.UnbindExternalFunction("start_quest");
            currentStory.UnbindExternalFunction("finish_quest");

            if (serveGuestStep != null)
            {
                serveGuestStep.isOnDialogue = false;
                currentStory.UnbindExternalFunction("start_quest");

                serveGuestStep = null;
            }
        }

        Innkeeper.instance.canMove = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            // TODO - обработать случай когда последняя строка это external функция
            
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            HandleTags(currentStory.currentTags);
            
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        canContinueToNextLine = false;

        continueIcon.SetActive(false);
        HideChoices();

        foreach(char letter in line.ToCharArray())
        {
            if (InputManager.instance.GetInteractPressed())
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    void HandleTags(List<string> currentTag)
    {
        foreach(string tag in currentTag)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriatly parsed " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;

                case PORTRAIT_TAG: // !!! доделать когда будут арты !!!
                    break;

                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag name in but is not currently being handled " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choises were given than the UI can support. Number of choises given: " + currentChoices.Count);
        }

        int index = 0;

        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoise(int choiseIndex)
    {
        currentStory.ChooseChoiceIndex(choiseIndex);
    }

    private void SetServeGuestStep(string questId)
    {
        Quest quest = QuestManager.instance.GetQuestById(questId);

        try
        {
            serveGuestStep = quest.GetCurrentQuestStepPrefab().GetComponent<ServeGuestStep>();
        }
        catch
        {
            serveGuestStep = null;
        }
        
    }
}
