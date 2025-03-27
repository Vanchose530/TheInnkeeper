using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestRandomMovementState", menuName = "ScriptableObjects/States/Guest/Random Movement", order = 1)]
public class GuestRandomMovementState : GuestState
{
    [Header("In State Time")]
    [SerializeField] private float minInStateTime;
    [SerializeField] private float maxInStateTime;
    private float inStateTime;

    [Header("Movement")]
    [SerializeField] private float minTimeToWalkInOneTurn;
    [SerializeField] private float maxTimeToWalkInOneTurn;
    private float timeToWalkInOneTurn;

    /*[Header("Wall Check")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckRadius;*/

    public override void Init()
    {
        isFinished = false;

        guest.canMove = true;

        inStateTime = Random.Range(minInStateTime, maxInStateTime);
        ResetTimeToWalkInOneTurn();
    }
    public override void Run()
    {
        CountTime();
        if (inStateTime <= 0) { isFinished = true; guest.wantToSeat = true; } 
        if (timeToWalkInOneTurn <= 0 || guest.rb.velocity.sqrMagnitude < (guest.speed * guest.speed)) SetRandomWalkTurn();
        //else if (CheckWalls()) guest.SetMovement(-guest.movement);
    }

    public override void Exit()
    {
        guest.SetMovement(Vector2.zero);
    }

    private void ResetTimeToWalkInOneTurn()
    {
        timeToWalkInOneTurn = Random.Range(minTimeToWalkInOneTurn, maxTimeToWalkInOneTurn);
    }

    private void CountTime()
    {
        inStateTime -= Time.deltaTime;
        timeToWalkInOneTurn -= Time.deltaTime;
    }

    private void SetRandomWalkTurn()
    {
        ResetTimeToWalkInOneTurn();
        Vector2 movement = Random.insideUnitCircle.normalized * guest.speed;
        guest.SetMovement(movement);
    }

    /*private bool CheckWalls()
    {
        return Physics2D.OverlapCircle(guest.transform.position, wallCheckRadius, wallLayer) != null;
    }*/
}
