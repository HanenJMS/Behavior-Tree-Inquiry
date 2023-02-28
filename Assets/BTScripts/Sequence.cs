using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name)
    {
        Name = name;
    }
    public override Status Process()
    {
        if(CurrentChild >= children.Count)
        {
            CurrentChild = 0;
            return Status.Success;
        }
        Status childStatus = children[CurrentChild].Process();
        if (childStatus == Status.Running) return childStatus;
        if (childStatus == Status.Failure) return childStatus;
        if (childStatus == Status.Success) CurrentChild++;
        return Status.Running;
    }
}
