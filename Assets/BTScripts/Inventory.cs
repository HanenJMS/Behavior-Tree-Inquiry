using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] int gold = 0;
    public void PickUpItem(Item item)
    {
        items.Add(item.PickUpItem());
    }
    public void AddItem(Item item)
    {
        items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
    public bool RequestItemAvailable(Item itemType)
    {
        return(items.Contains(itemType));
    }
    public int GetGold()
    {
        return gold;
    }
    public void AddGold(int itemValue)
    {
        gold += itemValue;
    }
    public List<Item> GetInventoryList()
    {
        return items;
    }
}
