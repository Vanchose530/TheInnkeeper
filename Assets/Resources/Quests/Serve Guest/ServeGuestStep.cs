using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeGuestStep : QuestStep
{
    public List<Dish> wantDishes;

    public bool canServe { get; private set; }

    [HideInInspector] public bool isOnDialogue = false;

    void Start()
    {
        
    }

    void Update()
    {
        canServe = CheckWantDishes() && isOnDialogue;

        if (canServe)
        {
            GameEventsManager.instance.questEvents.onServeGuest += ServeGuest;
        }
        else
        {
            GameEventsManager.instance.questEvents.onServeGuest -= ServeGuest;
        }
    }

    public bool CheckWantDishes() 
    {
        List<Dish> innkeeperInventoryCopy = new List<Dish>();

        foreach(var dish in InnkeeperInventory.instance.dishes)
        {
            innkeeperInventoryCopy.Add(dish);
        }

        foreach(var dish in wantDishes)
        {
            if (innkeeperInventoryCopy.Contains(dish))
            {
                innkeeperInventoryCopy.Remove(dish); 
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void ServeGuest()
    {
        for (int i = 0; i < wantDishes.Count; i++)
        {
            if (InnkeeperInventory.instance.dishes.Contains(wantDishes[i]))
            {
                InnkeeperInventory.instance.dishes.Remove(wantDishes[i]);
            }
        }

        FinishQuestStep(finishQuest : true);
        GameEventsManager.instance.questEvents.onServeGuest -= ServeGuest;
    }
}
