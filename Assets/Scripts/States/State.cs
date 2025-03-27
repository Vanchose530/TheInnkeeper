using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool isFinished { get; protected set; }

    // !!! в наследнике реализовать поле класса для работы состояния с конкретным объектом !!!

    public virtual void Init() { isFinished = false; }

    public virtual void Exit() { }

    public abstract void Run();
}
