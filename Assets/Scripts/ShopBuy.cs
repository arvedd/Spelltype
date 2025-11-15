using System.Collections.Generic;
using UnityEngine;

public class ShopBuy : MonoBehaviour
{
    public List<ShopData> shopDatas;
    public Inventory inventory;
    public AudioSource audioSource;
    public int playerCoin;

    void Start()
    {
        if (inventory == null)
            inventory = FindAnyObjectByType<Inventory>();
        playerCoin = GoldManager.GetGold();
    }
    
    public void BuyItem(int index)
    {
        if (index < 0 || index >= shopDatas.Count)
            return;

        if (shopDatas[index].itemStock > 0)
        {
            if (playerCoin >= shopDatas[index].itemCost)
            {
                playerCoin -= shopDatas[index].itemCost;
                shopDatas[index].itemStock--;

                Inventory.instance.Add(shopDatas[index]);
                Inventory.instance.SaveInventory();

                ShopManager.Instance.BuySound();
                GoldManager.SetGold(playerCoin);
                Debug.Log($"Item : {shopDatas[index].itemName} has been bought");
            }
            else
            {
                audioSource.Play();
                Debug.Log("Not enough coin");
            }
        }
        else
        {
            Debug.Log($"Stock for item : {shopDatas[index].itemName} is 0");
        }
    }

}
