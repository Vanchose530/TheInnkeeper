using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestWalkToSeatingPositionState", menuName = "ScriptableObjects/States/Guest/Walk to seating position", order = 1)]
public class GuestWalkToSeatingPositionState : GuestState
{
    private bool doingLastSteps;
    public override void Init()
    {
        isFinished = false;
        doingLastSteps = false;

        guest.ChoseRandomSeatingPosition();
        guest.MoveTo(guest.chosenSeatingPosition.transform);

        guest.onEndOfPath += DoLastSteps;
    }

    public override void Run()
    {
        guest.PathExecution();

        if (doingLastSteps) guest.MoveTo(guest.chosenSeatingPosition.transform, false);
    }

    public override void Exit()
    {
        guest.onEndOfPath -= DoLastSteps;
    }

    public void DoLastSteps()
    {
        guest.MoveTo(guest.chosenSeatingPosition.transform, false);
        doingLastSteps = true;
    }

    public void FinishState()
    {
        isFinished = true;
        guest.wantToSeat = false;
        guest.seat = true;
        guest.SetMovement(Vector2.zero);
    }
}
