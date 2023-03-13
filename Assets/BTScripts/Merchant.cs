using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    Inventory inventory;
    int potentialValue = 0;
    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }
    public void SellItem(Inventory client, Item item)
    {
        if(item.GetValue() <= inventory.GetMoney())
        {
            TradeItem(client, item);
        }
    }
    public void TradeItem(Inventory client, Item item)
    {
        inventory.BuyItem(item);
        client.SellItem(item);
    }
    private int GetInventoryValue()
    {
        foreach(Item item in inventory.GetInventoryList())
        {
            potentialValue += item.GetValue();
        }
        return potentialValue;
    }
}
