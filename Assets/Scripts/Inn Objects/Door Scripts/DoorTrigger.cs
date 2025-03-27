using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private DoorOpenStatus setDoorOpenStatus;

    [SerializeField] private Door door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && door.doorOpenStatus == DoorOpenStatus.NotOpen)
        {
            door.OpenDoor(setDoorOpenStatus);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            door.OpenDoor(DoorOpenStatus.NotOpen);
        }
    }
}
