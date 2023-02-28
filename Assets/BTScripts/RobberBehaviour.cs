using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    NavMeshAgent agent;
    [SerializeField] GameObject diamond, van, door;
    ActionState state = ActionState.Idle;
    Status treeStatus = Status.Running;
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
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDoor = new Leaf("Go To Door", GoToDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        steal.AddChild(goToDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        //tree.Process();
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



    private void Update()
    {
        if(treeStatus == Status.Running)
        {
            treeStatus = tree.Process();
        }
    }
    private Status GoToDoor()
    {
        return GoToLocation(door.transform.position);
    }
    Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }
    Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }
    Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if(state == ActionState.Idle)
        {
            agent.SetDestination(destination);
            state = ActionState.Working;
        }
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= 2f)
        {
            state = ActionState.Idle;
            return Status.Failure;
        }
        else if(distanceToTarget < 2f)
        {
            state = ActionState.Idle;
            return Status.Success;
        }
        return Status.Running;
    }
}
