using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class Selector : Node
{
    public Selector(string name)
    {
        Name = name;
    }
    public override Status Process()
    {
        if (CurrentChild >= children.Count)
        {
            CurrentChild = 0;
            return Status.Failure;
        }
        Status childStatus = children[CurrentChild].Process();
        if (childStatus == Status.Running) return childStatus;
        if (childStatus == Status.Success)
        {
            CurrentChild = 0;
            return childStatus;
        }
        CurrentChild++;
        return Status.Running;
    }
}
