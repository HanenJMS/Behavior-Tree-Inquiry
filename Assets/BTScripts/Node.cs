using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Status status;
    public List<Node> children = new List<Node>();
    public int CurrentChild { get; set; } = 0;
    public string Name { get; set; }

    public Node()
    {

    }
    public Node(string name)
    {
        Name = name;
    }
    public virtual Status Process()
    {
        return children[CurrentChild].Process();
    }
    public void AddChild(Node child)
    {
        children.Add(child);
    }
    public Node GetChild()
    {
        return children[CurrentChild];
    }
}
