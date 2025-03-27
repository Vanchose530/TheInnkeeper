using System;

public class InputEvents 
{
    public event Action onInteractPressed;

    public void InteractPressed()
    {
        if (onInteractPressed != null && !PauseMenuManager.instance.isOnPause) onInteractPressed();
    }

    public event Action onInteractCanceled;

    public void InteractCanceled()
    {
        if (onInteractCanceled != null && !PauseMenuManager.instance.isOnPause) onInteractCanceled();
    }

    public event Action onOpenInventoryPressed;

    public void OpenInventoryPressed()
    {
        if (onOpenInventoryPressed != null && !PauseMenuManager.instance.isOnPause) onOpenInventoryPressed();
    }

    public event Action onPauseButtonPressed;

    public void PauseButtonPressed()
    {
        if (onPauseButtonPressed != null) onPauseButtonPressed();
    }
}
