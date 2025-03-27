using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatOnDialogueState", menuName = "ScriptableObjects/States/Cat/On Dialogue", order = 1)]
public class CatOnDialogueState : State
{
    public override void Init()
    {
        isFinished = false;

        Cat.instance.SetMovement(Vector2.zero);
        Cat.instance.canMove = false;
    }

    public override void Run()
    {
        if(!Cat.instance.dialogue.thisDialogueIsPlaying) isFinished = true;
    }
}
