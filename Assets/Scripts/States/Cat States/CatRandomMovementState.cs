using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatRandomMovementState", menuName = "ScriptableObjects/States/Cat/Random Movement", order = 1)]
public class CatRandomMovementState : State
{
    [Header("In State Time")]
    [SerializeField] private float minInStateTime;
    [SerializeField] private float maxInStateTime;
    private float inStateTime;

    [Header("Movement")]
    [SerializeField] private float minTimeToWalkInOneTurn;
    [SerializeField] private float maxTimeToWalkInOneTurn;
    private float timeToWalkInOneTurn;

    public override void Init()
    {
        isFinished = false;

        Cat.instance.canMove = true;

        inStateTime = Random.Range(minInStateTime, maxInStateTime);
        ResetTimeToWalkInOneTurn();

        SetRandomWalkTurn();
    }

    public override void Run()
    {
        CountTime();
        if(inStateTime <= 0) isFinished = true;
        if (timeToWalkInOneTurn <= 0 || Cat.instance.rb.velocity.sqrMagnitude < (Cat.instance.speed * Cat.instance.speed)) SetRandomWalkTurn();
    }

    private void CountTime()
    {
        inStateTime -= Time.deltaTime;
        timeToWalkInOneTurn -= Time.deltaTime;
    }

    private void ResetTimeToWalkInOneTurn()
    {
        timeToWalkInOneTurn = Random.Range(minTimeToWalkInOneTurn, maxTimeToWalkInOneTurn);
    }

    private void SetRandomWalkTurn()
    {
        ResetTimeToWalkInOneTurn();
        Vector2 movement = Random.insideUnitCircle.normalized * Cat.instance.speed;
        Cat.instance.SetMovement(movement);
    }
}
