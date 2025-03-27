using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    private static PerspectiveManager instance;

    public float ZAxisSpeed;
    public float YStartingPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one \"PerspectiveManager\" class");
        }
        instance = this;
    }

    public static PerspectiveManager GetInstance()
    {
        return instance;
    }
}
