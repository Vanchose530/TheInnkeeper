using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestSeatState", menuName = "ScriptableObjects/States/Guest/Seat", order = 1)]
public class GuestSeatState : GuestState
{
    [Header("Time after service")]
    [SerializeField] private float minTimeAfterService;
    [SerializeField] private float maxTimeAfterService;
    private float timeAfterService;

    public override void Init()
    {
        isFinished = false;

        timeAfterService = Random.Range(minTimeAfterService, maxTimeAfterService);

        guest.perspectiveRegulator.active = false;
        guest.boxCollider.enabled = false;
        guest.dialogue.active = true;

        guest.transform.position = guest.chosenSeatingPosition.transform.position;
        guest.SetMovement(Vector2.zero);
    }
    public override void Run()
    {
        if(guest.serviced) CountTimeAfterService();
        if (timeAfterService <= 0 && !guest.dialogue.thisDialogueIsPlaying)
        {
            isFinished = true;
            guest.seat = false;
            guest.wantToExit = true;
        }
    }

    public override void Exit()
    {
        guest.perspectiveRegulator.active = true;
        guest.boxCollider.enabled = true;

        guest.dialogue.active = false;

        guest.transform.position = guest.chosenSeatingPosition.exitPosition;
        guest.chosenSeatingPosition.StandUp();
        guest.ResetSeatingPosition();
    }

    private void CountTimeAfterService()
    {
        timeAfterService -= Time.deltaTime;
    }
}
