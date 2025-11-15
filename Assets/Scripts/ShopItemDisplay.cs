using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour
{
    public Image itemIconBG;
    public Image itemIcon;
    public TMP_Text itemShopName;
    public TMP_Text itemShopDesc;
    public TMP_Text itemShopCost;

    void Start()
    {
        Hide();
    }

    public void ShowItem(ShopData shopData)
    {
        itemIconBG.sprite = shopData.iconBG;
        itemIcon.sprite = shopData.itemIcon;
        itemShopName.text = shopData.itemName;
        itemShopDesc.text = shopData.itemDesc;
        itemShopCost.text = shopData.itemCost.ToString();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
