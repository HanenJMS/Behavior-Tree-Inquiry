using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BehaviourTreeAgent
{
    [SerializeField] Dictionary<Item, int> itemsList = new Dictionary<Item, int>();
    [SerializeField] Stack<Item> itemsToSteal = new Stack<Item>();
    [SerializeField] GameObject backDoor, frontDoor;

    [SerializeField] private int minMoney = 500;

    new void Start()
    {
        base.Start();
        CreateTree();
    }
    public override void CreateTree()
    {
        foreach (Item item in FindObjectsOfType<Item>())
        {
            itemsList.Add(item, item.GetValue());
        }
        foreach(KeyValuePair<Item, int> items in itemsList.OrderBy(key => key.Value))
        {
            itemsToSteal.Push(items.Key);
        }
        Sequence steal = new Sequence("Steal Something");
        Selector openDoor = new Selector("Open Door");
        Selector selectItemToSteal = new Selector("Choosing what to steal");
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Door", GoBackToDoor);
        Leaf goTocurrentObjective = new Leaf("Go To Current Objective", GoToObjective);
        Leaf goToVan = new Leaf("Go To Van", GoToMerchant);
        Leaf goToFrontDoor = new Leaf("GoToFrontDoor", GoToFrontDoor);
        Inverter invertMoney = new Inverter("Has Money");
        invertMoney.AddChild(hasGotMoney);
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);


        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        //steal.AddChild(stealWholeMuseum);
        steal.AddChild(goTocurrentObjective);
        steal.AddChild(merchantSellingBehaviour);
        //steal.AddChild(goToDoor);
        //steal.AddChild(goToVan);
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
    private Status HasMoney()
    {
        if (inventory.GetMoney() < minMoney)
        {
            currentObjective = itemsToSteal.Pop();
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

    Status GoToItem(Item item)
    {
        if (item.Equals(null)) 
        { 
            return Status.Failure; 
        }
        Status PerformAction = GoToLocation(item.transform.position);
        if(PerformAction.Equals(Status.Success))
        {
            inventory.AddItem(item);
        }
        return PerformAction;
    }
    Status GoToObjective()
    {
        return GoToItem(currentObjective);
    }


}
