using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ShopData itemData;
    public int stackSize;

    public InventoryItem(ShopData item)
    {
        itemData = item;
        stackSize = 0;
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
        if (stackSize < 0) stackSize = 0;
    }
}