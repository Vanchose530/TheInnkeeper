using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool isFinished { get; protected set; }

    // !!! � ���������� ����������� ���� ������ ��� ������ ��������� � ���������� �������� !!!

    public virtual void Init() { isFinished = false; }

    public virtual void Exit() { }

    public abstract void Run();
}
