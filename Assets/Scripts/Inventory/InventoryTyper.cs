using TMPro;
using UnityEngine;

public class InventoryTyper : MonoBehaviour
{
    [Header("References")]
    public InventoryUI inventoryUI;
    public InventoryManager inventoryManager;
    public GameObject pointerBook;
    public GameObject pointerInv;
    private SpellTyper spellTyper;
    private bool isInventoryOpen = false;

    void Update()
    {
        HandleInventoryToggle();
    }

    void HandleInventoryToggle()
    {
        if (PauseManager.IsPaused) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isInventoryOpen)
            {
                var st = GetSpellTyper();
                if (st != null)
                    st.ClearBuffer();

                if(pointerBook) pointerBook.SetActive(false);
                pointerInv.SetActive(true);
                inventoryUI.DeactivateBook();
                inventoryUI.ActivateInventory();
                isInventoryOpen = true;
                
                if (inventoryManager != null)
                {
                    StartCoroutine(DelayedRefresh());
                }
            }
            else
            {
                if(pointerBook) pointerBook.SetActive(true);
                pointerInv.SetActive(false);
                inventoryUI.ActivateBook();
                inventoryUI.DeactivateInventory();
                isInventoryOpen = false;
            }
        }

        

        if (Input.GetKeyDown(KeyCode.Escape) && isInventoryOpen)
        {
            if(pointerBook) pointerBook.SetActive(true);
            pointerInv.SetActive(false);
            inventoryUI.ActivateBook();
            inventoryUI.DeactivateInventory();
            isInventoryOpen = false;
        }
    }

    private SpellTyper GetSpellTyper()
    {
        if (spellTyper == null)
            spellTyper = FindAnyObjectByType<SpellTyper>();

        return spellTyper;
    }

    System.Collections.IEnumerator DelayedRefresh()
    {
        yield return null;
        
        if (inventoryManager != null)
        {
            inventoryManager.RefreshInventoryUI();
        }
    }
}