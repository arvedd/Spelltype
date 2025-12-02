using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InventorySaveData
{
    public List<string> itemNames = new List<string>();
    public List<int> itemAmounts = new List<int>();
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ShopData, InventoryItem> itemDictionary = new Dictionary<ShopData, InventoryItem>();
    public List<ShopData> allShopItems;
    
    public static event Action OnInventoryChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadInventory(allShopItems);
    }

    public void Add(ShopData itemData)
{
    if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
    {
        item.AddToStack();
    }
    else
    {
        InventoryItem newItem = new InventoryItem(itemData);
        newItem.AddToStack();
        inventory.Add(newItem);
        itemDictionary.Add(itemData, newItem);
    }
    
    SaveInventory();
    OnInventoryChanged?.Invoke();
}

    public void SaveInventory()
{
    CleanupEmptyItems();
    
    InventorySaveData saveData = new InventorySaveData();

    foreach (var item in inventory)
    {
        if (item.stackSize > 0)
        {
            saveData.itemNames.Add(item.itemData.itemName);
            saveData.itemAmounts.Add(item.stackSize);
        }
    }

    string json = JsonUtility.ToJson(saveData);
    PlayerPrefs.SetString("InventoryData", json);
    PlayerPrefs.Save();

    OnInventoryChanged?.Invoke();
}

    public void LoadInventory(List<ShopData> allShopItems)
    {

        if (Ending.isResetting) 
        {
            inventory.Clear();
            itemDictionary.Clear();
            return;
        }

        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            InventorySaveData loadedData = JsonUtility.FromJson<InventorySaveData>(json);

            inventory.Clear();
            itemDictionary.Clear();

            for (int i = 0; i < loadedData.itemNames.Count; i++)
            {
                string itemName = loadedData.itemNames[i];
                int amount = loadedData.itemAmounts[i];

                if (amount <= 0) continue;

                ShopData foundItem = allShopItems.Find(x => x.itemName == itemName);
                if (foundItem != null)
                {
                    InventoryItem newItem = new InventoryItem(foundItem);
                    newItem.stackSize = amount;
                    inventory.Add(newItem);
                    itemDictionary.Add(foundItem, newItem);
                }
            }
            
        }
        
        OnInventoryChanged?.Invoke();
    }

    private void CleanupEmptyItems()
    {
        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            if (inventory[i].stackSize <= 0)
            {
                if (itemDictionary.ContainsKey(inventory[i].itemData))
                {
                    itemDictionary.Remove(inventory[i].itemData);
                }
                inventory.RemoveAt(i);
            }
        }
    }

    public List<InventoryItem> GetAllItems()
    {
        CleanupEmptyItems();
        return inventory;
    }

    public void Remove(ShopData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if (item.stackSize <= 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
            
            SaveInventory();
            OnInventoryChanged?.Invoke();
        }
    }

    public void ResetInventory()
    {
        inventory.Clear();
        itemDictionary.Clear();
        PlayerPrefs.DeleteKey("InventoryData");
        PlayerPrefs.Save();
    }
}