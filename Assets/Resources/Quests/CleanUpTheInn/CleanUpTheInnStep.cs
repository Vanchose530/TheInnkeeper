using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpTheInnStep : QuestStep
{
    private void Update()
    {
        if (InnManager.instance.innClean) FinishQuestStep();
    }
}
