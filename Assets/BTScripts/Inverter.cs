using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    public Inverter(string name)
    {
        Name = name;
    }
    public override Status Process()
    {
        Status currentStatus = children[CurrentChild].Process();
        if(currentStatus.Equals(Status.Success))
        {
            return Status.Failure;
        }
        if(currentStatus.Equals(Status.Failure))
        {
            return Status.Success;
        }
        return currentStatus;
    }
}
