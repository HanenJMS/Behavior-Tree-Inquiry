using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lock : MonoBehaviour
{
    [SerializeField] bool isLocked = false;
    public bool CanOpen()
    {
        if (!isLocked) return true;
        return false;
    }
    public void Open()
    {
        if (CanOpen())
        {
            gameObject.SetActive(false);
        }
    }
}
