using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorModeManager : MonoBehaviour
{
    private static CursorModeManager instance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one \"CursorModeManager\" class");
        }
        instance = this;
    }

    public static CursorModeManager GetInstance()
    {
        return instance;
    }
}
