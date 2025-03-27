using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    public static FrontDoor instance { get; private set; }

    private Animator animator;

    public float openTime;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find more than one Front Door in scene");
        }
        instance = this;

        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    public IEnumerator OpenDoorCoroutine()
    {
        animator.SetBool("Open", true);
        yield return new WaitForSeconds(openTime);
        animator.SetBool("Open", false);
    }
}
