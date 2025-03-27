using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatStayState", menuName = "ScriptableObjects/States/Cat/Stay", order = 1)]
public class CatStayState : State
{
    [Header("In State Time")]
    [SerializeField] private float minInStateTime;
    [SerializeField] private float maxInStateTime;
    private float inStateTime;

    public override void Init()
    {
        isFinished = false;

        inStateTime = Random.Range(minInStateTime, maxInStateTime);

        Cat.instance.rb.velocity = Vector2.zero;
        Cat.instance.SetMovement(Vector2.zero);
        Cat.instance.canMove = false;
    }

    public override void Run()
    {
        if (inStateTime <= 0 || Cat.instance.rb.velocity.sqrMagnitude > 0.25) isFinished = true;

        inStateTime -= Time.deltaTime;
    }
}
