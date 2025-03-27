using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public InputEvents inputEvents;
    public InnkeeperInventoryEvents innkeeperInventoryEvents;

    private void Awake()
    {
        if (instance != null) Debug.LogWarning("Found more than one Game Event Manager script in scene");

        instance = this;

        questEvents = new QuestEvents();
        inputEvents = new InputEvents();
        innkeeperInventoryEvents = new InnkeeperInventoryEvents();
    }
}
