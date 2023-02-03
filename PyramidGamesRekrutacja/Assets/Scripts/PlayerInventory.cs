using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<int> inventory;

    public void AddItemtoInventory(int itemIndex){ inventory.Add(itemIndex); }
    
    public List<int> GetInventory(){ return inventory; }

    public void SetNewEmptyInventory(){ inventory = new List<int>(); }
}
