using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnevmoslonQuestStep : QuestStep
{
    private void Update()
    {
        if (Gramophone.instance.isMusicPlaying) FinishQuestStep();
    }
}
