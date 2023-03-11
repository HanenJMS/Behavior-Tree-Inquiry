using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] int value = 0;
    public Item PickUpItem()
    {
        this.gameObject.SetActive(false);
        return this;
    }
    public int GetValue()
    {
        return value;
    }
}
