using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        Name = "Tree";
    }
    struct NodeLevel
    {
        public int level;
        public Node node;
    }
    public void PrintTree()
    {
        string treePrint = "";
        NodeLevel currentNode = new NodeLevel { level = 0, node = this };
        treePrint = RecursiveSolution(currentNode, treePrint);
        treePrint = StackSolution(currentNode, treePrint);

        Debug.Log(treePrint);
    }
    private string RecursiveSolution(NodeLevel currentNode, string treePrint)
    {
        if(currentNode.node == null) return treePrint;
        string level = new string('-', currentNode.level);
        treePrint += $"{level}{currentNode.node.Name}\n";
        foreach(Node child in currentNode.node.children)
        {
            NodeLevel nextNode = new NodeLevel { level = currentNode.level + 1, node = child };
            treePrint = RecursiveSolution(nextNode, treePrint);
        }
        return treePrint;
    }

    private string StackSolution(NodeLevel currentNode, string treePrint)
    {
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        //NodeLevel level = new NodeLevel { level = 0, node = currentNode.node };

        nodeStack.Push(currentNode);

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            string nextLevelName = new string('-', nextNode.level);
            treePrint += $"{nextLevelName} {nextNode.node.Name}\n";
            for (int childCounter = nextNode.node.children.Count - 1; childCounter >= 0; childCounter--)
            {
                NodeLevel nextLevel = new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[childCounter] };
                nodeStack.Push(nextLevel);
            }
        }

        return treePrint;
    }
}
