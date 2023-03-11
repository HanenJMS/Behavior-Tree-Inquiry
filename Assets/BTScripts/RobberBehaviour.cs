using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    NavMeshAgent agent;
    Inventory inventory;
    [SerializeField] List<GameObject> itemsList = new List<GameObject>();
    [SerializeField] Stack<GameObject> itemsToSteal = new Stack<GameObject>();
    [SerializeField] GameObject diamond, van, backDoor, frontDoor, currentObjective;
    ActionState state = ActionState.Idle;
    Status treeStatus = Status.Running;
    Status CurrentAction = Status.Success;
    [SerializeField] private int minMoney = 500;

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
        inventory = GetComponent<Inventory>();


    }

    private void Start()
    {
        foreach(GameObject item in itemsList)
        {
            itemsToSteal.Push(item);
        }
        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Selector openDoor = new Selector("Open Door");
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Door", GoBackToDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Leaf goToFrontDoor = new Leaf("GoToFrontDoor", GoToFrontDoor);
        Leaf stealWholeMuseum = new Leaf("Steal the whole museum", StealWholeMuseum);
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);


        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
        //steal.AddChild(stealWholeMuseum);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToDoor);
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

    private Status SellItems()
    {
        inventory.SellAllItems();
        return Status.Success;
    }

    private void Update()
    {
        if(treeStatus != Status.Success)
        {
            treeStatus = tree.Process();
        }
    }
    private Status HasMoney()
    {
        if(inventory.GetMoney() >= minMoney)
        {
            return Status.Failure;
        }
        return Status.Success;
    }
    private Status GoToDoor(GameObject door)
    {
        Lock doorLock = door.GetComponent<Lock>();
        Status performAction = GoToLocation(door.transform.position);
        if(performAction == Status.Success)
        {
            if (doorLock.CanOpen())
            {
                doorLock.Open();
                return Status.Success;
            }
            return Status.Failure;
        }
        return performAction;
    }
    Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }
    private Status GoBackToDoor()
    {
        return GoToDoor(backDoor);
    }

    Status GoToItem(GameObject item)
    {
        if (!item.TryGetComponent<Item>(out Item artWork)) 
        { 
            return Status.Failure; 
        }
        Status PerformAction = GoToLocation(item.transform.position);
        if(PerformAction.Equals(Status.Success))
        {
            inventory.AddItem(artWork);
        }
        return PerformAction;
    }
    Status StealWholeMuseum()
    {
        Status isRunning = Status.Running;
        if (itemsToSteal.Count == 0)
        {
            return Status.Success;
        }
        if (CurrentAction.Equals(Status.Success))
        {
            currentObjective = itemsToSteal.Pop();
            CurrentAction = GoToItem(currentObjective);
        }
        return isRunning;
    }
    Status GoToDiamond()
    {
        return GoToItem(diamond);
    }

    Status GoToVan()
    {
        Status actionSuccessful = GoToLocation(van.transform.position);
        if(actionSuccessful.Equals(Status.Success))
        {
            return SellItems();
        }
        return actionSuccessful;
    }
    Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if(state == ActionState.Idle)
        {
            agent.SetDestination(destination);
            state = ActionState.Working;
        }
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= 3f)
        {
            state = ActionState.Idle;
            return Status.Failure;
        }
        else if(distanceToTarget < 3f)
        {
            state = ActionState.Idle;
            return Status.Success;
        }
        return Status.Running;
    }
}
