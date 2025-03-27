using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineOrbitalTransposer;

public class Innkeeper : MonoBehaviour
{
    public static Innkeeper instance { get; private set; }

    Rigidbody2D rb;
    Animator animator;

    [Header("Innkeeper UI")]
    public Slider slider;

    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    float currentSpeed;
    Vector2 movement;
    [HideInInspector] public bool canMove = true;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one \"InnkeeperScript\" class");
        }
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = moveSpeed;

        animator.SetFloat("Vertical", -1f);

        UpdateSlider(0, false);
        GameEventsManager.instance.inputEvents.onInteractCanceled += () => { UpdateSlider(0, false); };
    }

    void Update()
    {
        if(DialogueManager.instance.dialogueIsPlaying) canMove = false;

        movement = InputManager.instance.moveDirection;

        SetRunningState();
        Move();
        Animate();
    }

    private void Move()
    {
        if(canMove) rb.velocity = (movement * currentSpeed);
        else rb.velocity = Vector2.zero;
    }

    void SetRunningState()
    {
        if (InputManager.instance.runPressed) currentSpeed = runSpeed;
        else currentSpeed = moveSpeed;
    }

    void Animate()
    {
        if (PauseMenuManager.instance.isOnPause) return;

        if(movement.sqrMagnitude != 0 && !DialogueManager.instance.dialogueIsPlaying && canMove)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        if(canMove) animator.SetFloat("Speed", movement.sqrMagnitude);
        else animator.SetFloat("Speed", 0f);
    }

    public void UpdateSlider(float value, bool active = true)
    {
        if (active)
        {
            slider.gameObject.SetActive(true);
            slider.value = value;
        }
        else
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }
    }
}
