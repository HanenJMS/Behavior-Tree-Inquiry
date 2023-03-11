using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] int money = 0;
    public void AddItem(Item item)
    {
        items.Add(item.PickUpItem());
    }
    public int GetMoney()
    {
        return money;
    }
    public void SetMoney(int transaction)
    {
        money += transaction;
    }
    public void SellAllItems()
    {
        foreach (Item item in items)
        {
            SetMoney(item.GetValue());
        }
        items.Clear();
    }
}
