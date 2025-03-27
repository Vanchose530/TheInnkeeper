using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    public DoorOpenStatus doorOpenStatus { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor(DoorOpenStatus status)
    {
        doorOpenStatus = status;

        switch (status)
        {
            case DoorOpenStatus.NotOpen:
                animator.Play("NotOpen");
                break;
            case DoorOpenStatus.OpenInside:
                animator.Play("OpenInside");
                break;
            case DoorOpenStatus.OpenOutside:
                animator.Play("OpenOutside");
                break;
        }
            
    }
}
