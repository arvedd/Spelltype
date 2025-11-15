using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    public Image itemIconBG;
    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public TMP_Text itemAmount;

    void Start()
    {
        Hide();
    }

    public void ShowItem(InventoryItem item)
    {

        if (itemIconBG == null) Debug.LogError("itemIconBG is NULL!");
        if (itemIcon == null) Debug.LogError("itemIcon is NULL!");
        if (itemName == null) Debug.LogError("itemName is NULL!");
        if (itemDesc == null) Debug.LogError("itemDesc is NULL!");
        if (itemAmount == null) Debug.LogError("itemAmount is NULL!");

        itemIconBG.enabled = true;
        itemIcon.enabled = true;
        itemName.enabled = true;
        itemDesc.enabled = true;
        itemAmount.enabled = true;

        itemIconBG.sprite = item.itemData.iconBG;
        itemIcon.sprite = item.itemData.itemIcon;
        itemName.text = item.itemData.itemName;
        itemDesc.text = item.itemData.itemDesc;
        itemAmount.text = "x" + item.stackSize;
        
    }

    public void Hide()
    {
        itemIconBG.sprite = null;
        itemIcon.sprite = null;
        itemName.text = "";
        itemDesc.text = "";
        itemAmount.text = "";

        itemIconBG.enabled = false;
        itemIcon.enabled = false;
        itemName.enabled = false;
        itemDesc.enabled = false;
        itemAmount.enabled = false;
    }
}