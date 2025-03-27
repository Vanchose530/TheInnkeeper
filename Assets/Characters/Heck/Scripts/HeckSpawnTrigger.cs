using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeckSpawnTrigger : MonoBehaviour
{
    [Header("Dialogue Camera")]
    [SerializeField] private CinemachineVirtualCamera dialogueVirtualCamera;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    bool isPlayerInRange;
    bool dialogueWasStarted;

    void Start()
    {
        isPlayerInRange = false;
        dialogueWasStarted = false;

        dialogueVirtualCamera.Priority = 0;
    }

    void Update()
    {
        if (isPlayerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            HeckScript.GetInstance().Appear();
        }

        if (!DialogueManager.instance.dialogueIsPlaying && dialogueWasStarted)
        {
            dialogueVirtualCamera.Priority = 0;
            HeckScript.GetInstance().activeFollowing = true;
            Destroy(this.gameObject);
            return;
        }

        if (HeckScript.GetInstance().appeared && !DialogueManager.instance.dialogueIsPlaying)
        {
            dialogueVirtualCamera.Priority = 20;
            DialogueManager.instance.EnterDialogueMode(inkJSON);
            dialogueWasStarted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
}
