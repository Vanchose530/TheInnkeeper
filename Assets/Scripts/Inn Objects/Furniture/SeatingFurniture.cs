using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SeatingFurniture : MonoBehaviour
{
    private Animator animator;

    private List<SeatingPosition> seatingPositions;

    [Header("Rising Settings")]
    public float riseTime;
    private float risePercents;
    private float riseSpeed;
    private bool riseExecuting;
    private float droppedSide;

    [Header("Start Settings")]
    public bool droppedOnStart;

    bool dropped;
    bool selected = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        risePercents = 0;

        SeatingPosition[] seatingPositionsArr = GetComponentsInChildren<SeatingPosition>();
        seatingPositions = new List<SeatingPosition>();

        foreach(SeatingPosition pos in seatingPositionsArr)
        {
            seatingPositions.Add(pos);
        }

        SetDropped(droppedOnStart);
    }

    private void Start()
    {
        InnManager.instance.CountNewFurniture();

        if (dropped) InnManager.instance.CountNewDroppedFurniture(); // поменять когда появится возможность ронять мебель непосредственно в игре
    }

    private void Update()
    {
        Rise();
        Animate();

        if(riseExecuting) UpdatePlayerSlider();
    }

    private void UpdatePlayerSlider()
    {
        Innkeeper.instance.UpdateSlider(risePercents / 100);

        // Innkeeper.instance.UpdateSlider(0, false);

    }

    private void Animate()
    {
        animator.SetBool("Dropped", dropped);
        animator.SetBool("Selected", selected);
        animator.SetFloat("Side", droppedSide);
    }

    private void Rise()
    {
        if (selected && dropped && InputManager.instance.GetInteractHolded())
        {
            riseSpeed = 100 / riseTime;

            Innkeeper.instance.canMove = false;
            riseExecuting = true;
            risePercents += Time.deltaTime * riseSpeed;

            if (risePercents >= 100)
            {
                SetDropped(false);
                InnManager.instance.CountNewUndroppedFurniture();
                riseExecuting = false;
                Innkeeper.instance.UpdateSlider(0, false);
            }
        }
        else
        {
            Innkeeper.instance.canMove = true;
            riseExecuting = false;
            risePercents = 0;
        }
    }

    public void SetDropped(bool _dropped)
    {
        if (_dropped)
        {
            droppedSide = Random.Range(0, 2);

            foreach(var pos in seatingPositions)
            {
                pos.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var pos in seatingPositions)
            {
                pos.gameObject.SetActive(true);
            }
        }

        dropped = _dropped;
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
