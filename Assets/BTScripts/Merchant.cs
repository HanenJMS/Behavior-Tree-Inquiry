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
    private bool CanBuyItem(Item item)
    {
        return item.GetValue() <= inventory.GetGold();
    }
    //most likely initiated by other merchant
    public bool SellItem(Merchant merchant, Item item)
    {
        if(CanBuyItem(item))
        {
            //TODO allow for trade value at higher level
            int tradeValue = item.GetValue();
            Buy(item, tradeValue);
            merchant.Sell(item, tradeValue);
            return true;
        }
        return false;
    }
    public void BuyItem(Merchant merchant, Item item)
    {
        if (inventory.RequestItemAvailable(item))
        {
            int cost = item.GetValue();
            merchant.SellItem(this, item);
        }
    }
    public void Buy(Item item, int cost)
    {
        inventory.AddItem(item);
        inventory.AddGold(-cost);
    }
    private void Sell(Item item, int cost)
    {
        inventory.RemoveItem(item);
        inventory.AddGold(cost);
    }
    
    //Sell All Behaviour
    public bool SellAll(Merchant merchant)
    {
        while(inventory.GetInventoryList().Count > 0)
        {
            Item item = inventory.GetInventoryList()[0];
            if(!merchant.SellItem(this, item))
            {
                return false;
            }
        }
        return true;
    }
}
