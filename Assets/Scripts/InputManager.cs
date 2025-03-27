using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public Vector2 moveDirection { get; private set; }
    public bool runPressed { get; private set; }

    private bool interactPressed;
    private bool interactHolded;

    private bool openInventoryPressed;

    private bool pauseButtonPressed;

    public static InputManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;

        moveDirection = Vector2.zero;
        interactPressed = false;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;

            GameEventsManager.instance.inputEvents.InteractPressed();
        }
        else if (context.canceled)
        {
            interactPressed = false;
            interactHolded = false;

            GameEventsManager.instance.inputEvents.InteractCanceled();
        }
    }

    public void OpenInventoryPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            openInventoryPressed = true;

            GameEventsManager.instance.inputEvents.OpenInventoryPressed();
        }
        else if (context.canceled)
        {
            openInventoryPressed = false;
        }
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pauseButtonPressed = true;

            GameEventsManager.instance.inputEvents.PauseButtonPressed();
        }
        else if (context.canceled)
        {
            pauseButtonPressed = false;
        }
    }

    public void RunPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            runPressed = true;
        }
        else if (context.canceled)
        {
            runPressed = false;
        }
    }

    public bool GetInteractHolded()
    {
        interactHolded = true;
        return interactPressed;
    }


    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetInteractPressed()
    {
        if (!interactHolded)
        {
            bool result = interactPressed;
            interactPressed = false;
            return result;
        }
        else
        {
            return false;
        }
    }

    public bool GetOpenInventoryPressed()
    {
        bool result = openInventoryPressed;
        openInventoryPressed = false;
        return result;
    }

    public bool GetPauseButtonPressed()
    {
        bool result = pauseButtonPressed;
        pauseButtonPressed = false;
        return result;
    }

}
