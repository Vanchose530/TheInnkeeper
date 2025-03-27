using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using System;

public class Guest : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D boxCollider { get; private set; }

    public DialogueTrigger dialogue { get; private set; }

    Seeker seeker;
    Path path;
    int currentWaypoint;

    public event Action onEndOfPath;

    [Header("Movement")]
    public float speed;
    public float nextWaypointDistance = 3f;
    Transform target;
    public bool reachedEndOfPath { get; private set; }

    [Header("States")]
    [SerializeField] GuestState startState;
    [SerializeField] GuestRandomMovementState randomMovementState;
    public GuestWalkToSeatingPositionState walkToSeatingPositionState;
    [SerializeField] GuestSeatState seatState;
    [SerializeField] GuestExitTheInnState exitTheInnState;

    private GuestState currentState;

    [HideInInspector] public bool canMove;
    [HideInInspector] public bool wantToSeat;
    [HideInInspector] public bool seat;
    [HideInInspector] public bool serviced;
    [HideInInspector] public bool wantToExit;
    public SeatingPosition chosenSeatingPosition { get; private set; }

    public PerspectiveRegulator perspectiveRegulator { get; private set; }
    public Vector2 movement { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        seeker = GetComponent<Seeker>();
        perspectiveRegulator = GetComponent<PerspectiveRegulator>();

        dialogue = this.transform.GetComponentInChildren<DialogueTrigger>();

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);

        SetState(startState);

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        reachedEndOfPath = true;

        wantToSeat = false;
        seat = false;
        serviced = false;
        dialogue.active = false;
        wantToExit = false;
    }

    private void Update()
    {
        currentState.Run();

        CheckStates();

        rb.velocity = movement;

        Animate();
    }

    private void SetState(GuestState state)
    {
        try { currentState.Exit(); }
        catch (NullReferenceException) { Debug.Log("For the " + gameObject.name + " game object, the state is set for the first time"); }
        finally
        {
            currentState = state;
            currentState.SetGuest(this);
            currentState.Init();
        }
    }

    private void CheckStates()
    {
        if (currentState.isFinished)
        {
            if (wantToSeat) SetState(walkToSeatingPositionState);
            else if (seat) SetState(seatState);
            else if (wantToExit) SetState(exitTheInnState);
            else SetState(randomMovementState);
        }
    }


    public void SetMovement(Vector2 _movement)
    {
        movement = _movement;
    }

    public void MoveTo(Transform position, bool aStar = true)
    {
        if(aStar) target = position;
        else
        {
            Vector2 direction = (position.position - transform.position).normalized;
            SetMovement(direction * speed);
        }
    }

    public void ExitTheInn()
    {
        FrontDoor.instance.OpenDoor();
        Destroy(this.gameObject);
    }

    void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (target != null) seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public void PathExecution()
    {
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            EndOfPath();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = Vector2.zero;

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        SetMovement(direction * speed);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void EndOfPath()
    {
        reachedEndOfPath = true;
        Debug.Log("Reached end of path");
        target = null;
        path = null;
        currentWaypoint = 0;
        SetMovement(Vector2.zero);

        if (onEndOfPath != null) onEndOfPath();
    }

    public void ChoseRandomSeatingPosition()
    {
        int i = UnityEngine.Random.Range(0 , InnManager.instance.availableSeatingPositions.Count);

        chosenSeatingPosition = InnManager.instance.availableSeatingPositions[i];
        chosenSeatingPosition.Chose();
    }

    public void ResetSeatingPosition()
    {
        chosenSeatingPosition = null;
    }

    private void Animate()
    {
        if (rb.velocity.sqrMagnitude != 0)
        {
            animator.SetFloat("Horizontal", rb.velocity.x);
            animator.SetFloat("Vertical", rb.velocity.y);
        }

        if (canMove) animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        else animator.SetFloat("Speed", 0f);

        animator.SetBool("Seat", seat);
        if (seat)
        {
            switch (chosenSeatingPosition.direction)
            {
                case CardinalDirections.South:
                    animator.SetFloat("Horizontal", 0);
                    animator.SetFloat("Vertical", -1);
                    break;
                case CardinalDirections.North:
                    animator.SetFloat("Horizontal", 0);
                    animator.SetFloat("Vertical", 1);
                    break;
                case CardinalDirections.East:
                    animator.SetFloat("Horizontal", 1);
                    animator.SetFloat("Vertical", 0);
                    break;
                case CardinalDirections.West:
                    animator.SetFloat("Horizontal", -1);
                    animator.SetFloat("Vertical", 0);
                    break;
            
            }
        }
    }
}
