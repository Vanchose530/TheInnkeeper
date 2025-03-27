using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InnkeeperInventoryEvents
{
    public event Action<int> onGoldGained;
    public void GoldGained(int gold)
    {
        if (onGoldGained != null)
        {
            onGoldGained(gold);
        }
    }

    public event Action<int> onSoulPowerGained;
    public void SoulPowerGained(int soulPower)
    {
        if (onSoulPowerGained != null)
        {
            onSoulPowerGained(soulPower);
        }
    }
}
