using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] int money = 0;
    public void AddItem(Item item)
    {
        items.Add(item.PickUpItem());
    }
    public Item ItemToSell()
    {
        if(items.Count != 0)
        {
            Item i = items[0];
            return i;
        }
        return null;
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
    public int GetMoney()
    {
        return money;
    }
    public void AddMoney(int transaction)
    {
        money += transaction;
    }
    public void BuyItem(Item item)
    {
        AddMoney(-item.GetValue());
        AddItem(item);
    }
    public void SellItem(Item item)
    {
        money += item.GetValue();
        RemoveItem(item);
    }
    public List<Item> GetInventoryList()
    {
        return items;
    }
    public void SellAllItems()
    {
        foreach (Item item in items)
        {
            AddMoney(item.GetValue());
        }
        items.Clear();
    }
}
