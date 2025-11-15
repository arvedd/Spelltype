using UnityEngine;

public class InventoryUIBattle : MonoBehaviour
{
    [Header("Inventory")]
    public GameObject targetInventory;
    public GameObject inventoryBG;
    public GameObject targetItem1;
    public GameObject targetItem2;
    public GameObject targetItem3;
    public GameObject targetItem4;

    void Start()
    {
        GameObject[] toDeactive = { targetInventory, targetItem1,
                                    targetItem2, targetItem3,
                                    targetItem4, inventoryBG };

        foreach (GameObject obj in toDeactive)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    public void ActivateInventory()
    {
        inventoryBG.SetActive(true);
        targetInventory.SetActive(true);
        targetItem1.SetActive(true);
        targetItem2.SetActive(true);
        targetItem3.SetActive(true);
        targetItem4.SetActive(true);
    }

    public void DeactivateInventory()
    {
        inventoryBG.SetActive(false);
        targetInventory.SetActive(false);
        targetItem1.SetActive(false);
        targetItem2.SetActive(false);
        targetItem3.SetActive(false);
        targetItem4.SetActive(false);
    }
}
