using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    NavMeshAgent agent;
    [SerializeField] GameObject diamond, van;
    private void Awake()
    {
        Initialization();
    }

    private bool Initialization()
    {
        return IsInitialized();
    }

    private bool IsInitialized()
    {
        Initialize();
        if (agent == null) return false;
        return true;
    }

    private void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        tree = new BehaviourTree();
        Node steal = new Node("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.Process();
        //Node eat = new Node("Eat Something");
        //Node pizza = new Node("Go To Pizza Shop");
        //Node buy = new Node("Buy Pizza");

        //eat.AddChild(pizza);
        //eat.AddChild(buy);
        //tree.AddChild(eat);

        //tree.PrintTree();
        /*OUTPUT
         *Tree
         *-Steal Something
         *--Go To Diamond
         *--Go To Van
         *Tree
         *-Steal Something
         *--Go To Diamond
         *--Go To Van
         */
    }
    public Status GoToDiamond()
    {
        agent.SetDestination(diamond.transform.position);
        return Status.Success;
    }
    public Status GoToVan()
    {
        agent.SetDestination(van.transform.position);
        return Status.Success;
    }
}
