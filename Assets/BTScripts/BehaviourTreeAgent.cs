using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTreeAgent : MonoBehaviour
{

    public Item currentObjective, currentItem;
    public BehaviourTree tree;
    public Sequence merchantSellingBehaviour;
    public NavMeshAgent agent;
    public Inventory inventory;
    public ActionState state = ActionState.Idle;
    public Status treeStatus = Status.Running;
    public Status CurrentAction = Status.Success;
    [SerializeField] Merchant client;
    WaitForSeconds waitForSeconds;
    public void Awake()
    {
        Initialization();
    }

    public bool Initialization()
    {
        return IsInitialized();
    }

    public bool IsInitialized()
    {
        Initialize();
        if (agent == null) return false;
        if (inventory == null) return false;
        return true;
    }

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();


    }

    public void Start()
    {
        tree = new BehaviourTree();
        merchantSellingBehaviour = new Sequence("Sell to merchant");
        Leaf findAMerchant = new Leaf("Looking for a merchant", FindAMerchant);
        Leaf goToMerchant = new Leaf("Go to merchant", GoToMerchant);
        merchantSellingBehaviour.AddChild(findAMerchant);
        merchantSellingBehaviour.AddChild(goToMerchant);
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine("Behave");
    }

    public virtual void CreateTree()
    {
        Leaf leaf = new Leaf("nothing", null);
        tree.AddChild(leaf);
    }

    IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }
    public Status GoToMerchant()
    {
        Status actionSuccessful = GoToLocation(client.transform.position);
        if (actionSuccessful.Equals(Status.Success))
        {
            Item item = inventory.ItemToSell();
            client.SellItem(inventory, item);
            return Status.Success;
        }
        return actionSuccessful;
    }

    private Status FindAMerchant()
    {
        if (inventory.GetInventoryList().Count == 0)
        {
            return Status.Failure;
        }
        foreach (Merchant client in FindObjectsOfType<Merchant>())
        {
            if (client != this.GetComponent<Merchant>())
            {
                this.client = client;
                return Status.Success;
            }
        }
        return Status.Failure;
    }
    public Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.Idle)
        {
            agent.SetDestination(destination);
            state = ActionState.Working;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 3f)
        {
            state = ActionState.Idle;
            return Status.Failure;
        }
        else if (distanceToTarget < 3f)
        {
            state = ActionState.Idle;
            return Status.Success;
        }
        return Status.Running;
    }
}
