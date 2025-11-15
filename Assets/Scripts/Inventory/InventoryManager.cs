using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory UI Elements")]
    public List<InventoryItemDisplay> inventorySlots;  
    public GameObject itemPointer;                    
    public List<GameObject> pointerSpot;              
    public int selectedIndex = 0;

    private bool needsRefresh = true;

    void OnEnable()
    {
        Inventory.OnInventoryChanged += RefreshInventoryUI;
        GameStateManager.OnStateChanged += OnStateChanged;
        needsRefresh = true;
        
        StartCoroutine(RefreshOnEnable());
    }

    void OnDisable()
    {
        Inventory.OnInventoryChanged -= RefreshInventoryUI;
        GameStateManager.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(GameState oldState, GameState newState)
    {
        if (GameStateManager.instance.IsInventoryOpen())
        {
            RefreshInventoryUI();
        }
    }

    IEnumerator RefreshOnEnable()
    {
        yield return null;
        yield return null;
        
        int maxWait = 100;
        int waited = 0;
        while (Inventory.instance == null && waited < maxWait)
        {
            yield return null;
            waited++;
        }
        
        if (Inventory.instance == null)
        {
            yield break;
        }
        
        RefreshInventoryUI();
    }

    void Start()
    {
        StartCoroutine(InitialRefresh());
    }

    IEnumerator InitialRefresh()
    {
        yield return null;
        
        while (Inventory.instance == null)
        {
            yield return null;
        }
        
        RefreshInventoryUI();
    }

    void Update()
    {
        if (!GameStateManager.instance.IsInventoryOpen())
        {
            return;
        }

        if (needsRefresh && Inventory.instance != null)
        {
            RefreshInventoryUI();
            needsRefresh = false;
        }

        HandlePointer();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UseSelectedItem();
        }
    }

    public void RefreshInventoryUI()
    {
        if (Inventory.instance == null)
        {
            needsRefresh = true;
            return;
        }

        var items = Inventory.instance.GetAllItems();

        if (selectedIndex >= items.Count && items.Count > 0)
        {
            selectedIndex = items.Count - 1;
        }
        else if (items.Count == 0)
        {
            selectedIndex = 0;
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] == null)
            {
                continue;
            }

            if (i < items.Count && items[i].stackSize > 0)
            {
                inventorySlots[i].ShowItem(items[i]);
            }
            else
            {
                inventorySlots[i].Hide();
            }
        }

        UpdatePointerPosition();
    }

    void UpdatePointerPosition()
    {
        if (itemPointer != null && pointerSpot.Count > selectedIndex && selectedIndex >= 0)
        {
            itemPointer.transform.position = pointerSpot[selectedIndex].transform.position;
        }
    }

    void UseSelectedItem()
    {
        if (!GameStateManager.instance.CanUseItems())
        {
            return;
        }

        if (Inventory.instance == null || Inventory.instance.inventory.Count == 0)
        {
            return;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, Inventory.instance.inventory.Count - 1);

        if (selectedIndex >= 0 && selectedIndex < Inventory.instance.inventory.Count)
        {
            InventoryItem selectedItem = Inventory.instance.inventory[selectedIndex];

            if (selectedItem.itemData.restoreHealth > 0)
            {
                Player player = FindAnyObjectByType<Player>();
                if (player != null)
                {
                    player.UseItem(selectedItem.itemData);

                    Inventory.instance.Remove(selectedItem.itemData);
                    
                    if (Inventory.instance.inventory.Count == 0)
                    {
                        selectedIndex = 0;
                    }
                    else if (selectedIndex >= Inventory.instance.inventory.Count)
                    {
                        selectedIndex = Inventory.instance.inventory.Count - 1;
                    }
                }
                else
                {
                    Debug.Log("Player not found!");
                }
            }
        }
    }

    void HandlePointer()
    {
        int maxIndex = Inventory.instance != null && Inventory.instance.inventory.Count > 0 
            ? Inventory.instance.inventory.Count - 1 
            : 0;

        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdatePointerPosition();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = Mathf.Min(maxIndex, selectedIndex + 1);
            UpdatePointerPosition();
        }
    }
}