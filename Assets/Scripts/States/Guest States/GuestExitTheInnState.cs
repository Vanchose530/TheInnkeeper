using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestExitTheInnState", menuName = "ScriptableObjects/States/Guest/Exit the inn", order = 1)]
public class GuestExitTheInnState : GuestState
{
    private bool doingLastSteps;
    Transform entrancePosition;

    public override void Init()
    {
        isFinished = false;
        doingLastSteps = false;
        entrancePosition = Entrance.instance.transform;

        guest.MoveTo(entrancePosition);
        guest.onEndOfPath += DoLastSteps;

    }
    public override void Run()
    {
        guest.PathExecution();

        if (doingLastSteps) guest.MoveTo(entrancePosition, false);
    }

    public override void Exit()
    {
        guest.onEndOfPath -= DoLastSteps;
    }

    public void DoLastSteps()
    {
        guest.MoveTo(entrancePosition, false);
        doingLastSteps = true;
    }
}
