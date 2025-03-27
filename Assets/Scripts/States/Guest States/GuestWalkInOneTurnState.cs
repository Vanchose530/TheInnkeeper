using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestWalkInOneTurnState", menuName = "ScriptableObjects/States/Guest/Walk in one turn", order = 1)]
public class GuestWalkInOneTurnState : GuestState
{
    public CardinalDirections turn;
    private Vector2 direction;

    [Header("In State Time")]
    [SerializeField] private float minInStateTime;
    [SerializeField] private float maxInStateTime;
    private float inStateTime;
    public override void Init()
    {
        isFinished = false;
        inStateTime = Random.Range(minInStateTime, maxInStateTime);
        guest.canMove = true;
        SetDirection();
        guest.SetMovement(direction * guest.speed);
    }
    public override void Run()
    {
        inStateTime -= Time.deltaTime;
        if(inStateTime <= 0) isFinished = true;
    }

    public override void Exit()
    {
        guest.SetMovement(Vector2.zero);
    }

    private void SetDirection()
    {
        switch (turn)
        {
            case CardinalDirections.North:
                direction = new Vector2(0, 1);
                break;
            case CardinalDirections.South:
                direction = new Vector2(0, -1);
                break;
            case CardinalDirections.West:
                direction = new Vector2(-1, 0);
                break;
            case CardinalDirections.East:
                direction = new Vector2(1, 0);
                break;
        }
    }
}
