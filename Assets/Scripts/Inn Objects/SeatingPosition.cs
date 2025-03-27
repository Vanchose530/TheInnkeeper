using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SeatingPosition : MonoBehaviour
{
    [SerializeField] public CardinalDirections direction;
    [SerializeField] public CardinalDirections exitDirection;
    public float exitDistance = 0.4f;
    public Vector2 exitPosition { get; private set; }

    public bool occupied { get; private set; }

    public bool chosenByGuest { get; private set; }

    private void OnEnable()
    {
        try
        {
            InnManager.instance.AddSeatingPosition(this);
        }
        catch (System.NullReferenceException)
        {
            StartCoroutine(AddSeatingPosition());
        }
    }

    private void OnDisable()
    {
        InnManager.instance.RemoveSeatingPosition(this);
    }

    private void Start()
    {
        occupied = false;
        SetExitPosition();
        
        StartCoroutine(AddSeatingPosition());
    }

    private IEnumerator AddSeatingPosition() // костыль чтобы исправить NullReferenceException при вызове OnEnable
    {
        yield return new WaitForEndOfFrame();

        if(!InnManager.instance.allSeatingPositions.Contains(this)) InnManager.instance.AddSeatingPosition(this);
    }

    private void SetExitPosition()
    {
        Vector2 exitPointDirection = Vector2.zero;

        switch (exitDirection)
        {
            case CardinalDirections.North:
                exitPointDirection = new Vector2(0, 1 * exitDistance);
                break;
            case CardinalDirections.South:
                exitPointDirection = new Vector2(0, -1 * exitDistance);
                break;
            case CardinalDirections.West:
                exitPointDirection = new Vector2(-1 * exitDistance, 0);
                break;
            case CardinalDirections.East:
                exitPointDirection = new Vector2(1 * exitDistance, 0);
                break;
        }

        exitPosition = (Vector2)transform.position + exitPointDirection;
    }

    private void OnDrawGizmos()
    {
        Vector2 directionVector = Vector2.zero;
        Gizmos.color = Color.green;

        switch(direction)
        {
            case CardinalDirections.North:
                directionVector = new Vector2(0, 1);
                break;
            case CardinalDirections.South:
                directionVector = new Vector2(0, -1);
                break;
            case CardinalDirections.West:
                directionVector = new Vector2(-1, 0);
                break;
            case CardinalDirections.East:
                directionVector = new Vector2(1, 0);
                break;
        }

        Gizmos.DrawWireSphere(transform.position, 0.05f);
        Gizmos.DrawRay(transform.position, directionVector * 0.08f);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 exitPointDirection = Vector2.zero;
        Gizmos.color = Color.white;

        switch (exitDirection)
        {
            case CardinalDirections.North:
                exitPointDirection = new Vector2(0, 1 * exitDistance);
                break;
            case CardinalDirections.South:
                exitPointDirection = new Vector2(0, -1 * exitDistance);
                break;
            case CardinalDirections.West:
                exitPointDirection = new Vector2(-1 * exitDistance, 0);
                break;
            case CardinalDirections.East:
                exitPointDirection = new Vector2(1 * exitDistance, 0);
                break;
        }

        Gizmos.DrawWireSphere((Vector2)transform.position + exitPointDirection, 0.02f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Guest")
        {
            Guest guest = collision.gameObject.GetComponent<Guest>();
            if(guest.chosenSeatingPosition == this)
            {
                guest.walkToSeatingPositionState.FinishState();
            }
        }
    }

    public void Chose() { chosenByGuest = true; }

    public void Seat() { occupied = true; }

    public void StandUp() { chosenByGuest = false; occupied = false; }
}
