using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuestState : State
{
    protected Guest guest;

    public void SetGuest(Guest _guest)
    {
        guest = _guest;
    }
}
