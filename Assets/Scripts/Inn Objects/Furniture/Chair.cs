using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    private Animator animator;

    [Header("Chair Position")]
    [SerializeField] private CardinalDirections chairDirection = CardinalDirections.South;

    [Header("Start Settings")]
    public bool droppedOnStart;

    bool dropped;
    bool selected = false;

    private void Awake()
    {
        dropped = droppedOnStart;

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InnManager.instance.CountNewFurniture();

        if (dropped) InnManager.instance.CountNewDroppedFurniture();
    }

    void Update()
    {
        SetDirection(chairDirection);
        SetDropped(dropped);
        SetSelected(selected);

        if (dropped && selected && InputManager.instance.GetInteractPressed())
        {
            dropped = false;
            InnManager.instance.CountNewUndroppedFurniture();
        }
    }

    void SetDirection(CardinalDirections _chairDirection)
    {
        switch(_chairDirection)
        {
            case CardinalDirections.North:
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 1);
                break;
            case CardinalDirections.South:
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", -1);
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

    void SetDropped(bool _dropped)
    {
        animator.SetBool("Dropped", _dropped);
    }

    void SetSelected(bool _selected)
    {
        animator.SetBool("Selected", _selected);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            selected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            selected = false;
        }
    }
}
