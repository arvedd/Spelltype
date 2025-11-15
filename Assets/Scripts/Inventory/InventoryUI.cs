using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory")]
    public GameObject targetInventory;
    public GameObject inventoryBG;
    public GameObject targetItem1;
    public GameObject targetItem2;
    public GameObject targetItem3;
    public GameObject targetItem4;

    [Header("Book")]
    public GameObject book;
    public GameObject bookItem1;
    public GameObject bookItem2;
    public GameObject bookItem3;
    public GameObject targetTitle1;
    // public GameObject targetTitle2;

    void Start()
    {
        GameObject[] toDeactive = { targetInventory, targetItem1,
                                    targetItem2, targetItem3,
                                    targetItem4, inventoryBG, book, targetTitle1 };

        foreach (GameObject obj in toDeactive)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    public void ActivateInventory()
    {
        if (inventoryBG) inventoryBG.SetActive(true);
        if (targetInventory) targetInventory.SetActive(true);
        if (targetItem1) targetItem1.SetActive(true);
        if (targetItem2) targetItem2.SetActive(true);
        if (targetItem3) targetItem3.SetActive(true);
        if (targetItem4) targetItem4.SetActive(true);
    }

    public void DeactivateInventory()
{
    if (inventoryBG) inventoryBG.SetActive(false);
    if (targetInventory) targetInventory.SetActive(false);
    if (targetItem1) targetItem1.SetActive(false);
    if (targetItem2) targetItem2.SetActive(false);
    if (targetItem3) targetItem3.SetActive(false);
    if (targetItem4) targetItem4.SetActive(false);
    
}


    public void ActivateBook()
    {
        if (book) book.SetActive(true);
        if (bookItem1) bookItem1.SetActive(true);
        if (bookItem2) bookItem2.SetActive(true);
        if (bookItem3) bookItem3.SetActive(true);
        if (targetTitle1) targetTitle1.SetActive(true);

    }


    public void DeactivateBook()
    {
        if (book) book.SetActive(false);
        if (bookItem1) bookItem1.SetActive(false);
        if (bookItem2) bookItem2.SetActive(false);
        if (bookItem3) bookItem3.SetActive(false);
        if (targetTitle1) targetTitle1.SetActive(false);
    }

}