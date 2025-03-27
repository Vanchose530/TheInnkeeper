using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CircleCollider2D))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Trigger Params")]
    public bool active = true; // костыль
    [SerializeField] private StartDialogueInstantlySettings startDialogueInstantlySettings;

    [Header("Dialogue Cue")]
    [SerializeField] private GameObject dialogueCue;

    [Header("Dialogue Camera")]
    [SerializeField] private CinemachineVirtualCamera dialogueVirtualCamera;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    string questId;
    QuestState currentQuestState;
    bool questPoint;

    bool playerIsNear;
    public bool thisDialogueIsPlaying { get; private set; }
    bool startDialogueInstantly;

    protected private enum StartDialogueInstantlySettings
    {
        No = 0,
        OnlyAtFirstTime = 1,
        Always = 2
    };

    private void Awake()
    {
        playerIsNear = false;
        thisDialogueIsPlaying = false;

        startDialogueInstantly = (startDialogueInstantlySettings == StartDialogueInstantlySettings.Always
            || startDialogueInstantlySettings == StartDialogueInstantlySettings.OnlyAtFirstTime);

        dialogueCue.SetActive(false);
        dialogueVirtualCamera.Priority = 0;

        questPoint = questInfoForPoint != null;

        if (questPoint)
        {
            questId = questInfoForPoint.id;
        }
    }

    private void Start()
    {
        if (questPoint)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
            QuestManager.instance.RecreateQuestMap();

        }
    }

    private void OnDisable()
    {
        if (questPoint)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        }
    }

    private void Update()
    {
        if (playerIsNear && !DialogueManager.instance.dialogueIsPlaying && active)
        {
            if(!startDialogueInstantly) dialogueCue.SetActive(true);

            if (InputManager.instance.GetInteractPressed() || startDialogueInstantly)
            {
                thisDialogueIsPlaying = true;

                startDialogueInstantly = false;

                if (!questPoint) DialogueManager.instance.EnterDialogueMode(inkJSON);
                else DialogueManager.instance.EnterDialogueMode(inkJSON, questId, currentQuestState);
            }
        }
        else
        {
            dialogueCue.SetActive(false);
        }

        if (!DialogueManager.instance.dialogueIsPlaying) thisDialogueIsPlaying = false;

        if (thisDialogueIsPlaying)
        {
            dialogueVirtualCamera.Priority = 20;
        }
        else
        {
            dialogueVirtualCamera.Priority = 0;
        }

        if(currentQuestState == QuestState.FINISHED && !thisDialogueIsPlaying) { GetComponentInParent<Guest>().serviced = true; }
    }
    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsNear = false;

            if(startDialogueInstantlySettings == StartDialogueInstantlySettings.Always) startDialogueInstantly = true;
        }
    }
}
