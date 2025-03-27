using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Entrance : MonoBehaviour
{
    public static Entrance instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find more than one Entrance in scene");
        }
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Guest")
        {
            Guest guest = collision.GetComponent<Guest>();
            if(guest.wantToExit)
            {
                guest.ExitTheInn();
            }
        }
    }
}
