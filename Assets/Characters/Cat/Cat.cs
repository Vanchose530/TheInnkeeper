using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    static public Cat instance;

    private Animator animator;
    public Rigidbody2D rb { get; private set; }

    public DialogueTrigger dialogue { get; private set; }

    [Header("Movement")]
    public float speed;

    [Header("States")]
    [SerializeField] State startState;
    [SerializeField] CatStayState stayState;
    [SerializeField] CatRandomMovementState randomMoveState;
    [SerializeField] CatOnDialogueState onDialogueState;

    private State currentState;

    [HideInInspector] public bool canMove;
    private Vector2 movement;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Find more than one Cat script in scene");
        }
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dialogue = this.transform.GetComponentInChildren<DialogueTrigger>();

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);

        SetState(startState);
    }

    private void Update()
    {
        currentState.Run();
        Animate();

        rb.velocity = movement;

        if(dialogue.thisDialogueIsPlaying)
        {
            SetState(onDialogueState);
        }

        if(currentState.isFinished)
        {
            switch(currentState)
            {
                case CatRandomMovementState:
                    SetState(stayState);
                    break;
                case CatStayState:
                    SetState(randomMoveState);
                    break;
                case CatOnDialogueState:
                    SetState(stayState);
                    break;
            }
        }
    }

    public void SetMovement(Vector2 _movement)
    {
        movement = _movement;
    }

    public void MoveTo(Vector2 position)
    {
        // TODO - реализовать метод для перемещения кота в конкретную позицию
    }

    private void SetState(State state)
    {
        try { currentState.Exit(); }
        catch (NullReferenceException) { Debug.Log("For the " + gameObject.name + " game object, the state is set for the first time"); }
        finally
        {
            currentState = state;
            currentState.Init();
        }
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
    }
}
