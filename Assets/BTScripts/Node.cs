using System.Collections.Generic;
using UnityEngine;

public class Node
{
    Status status;
    public List<Node> children = new List<Node>();
    public int CurrentChild { get; set; } = 0;
    public string Name { get; set; }

    public Node()
    {

    }
    public Node(string n)
    {
        Name = n;
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
