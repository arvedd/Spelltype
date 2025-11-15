using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject
{
    public Sprite iconBG;
    public Sprite itemIcon;
    public string itemName;
    [TextArea(3, 10)] public string itemDesc;
    public int itemCost;
    public int itemStock;
    public float restoreHealth;
    public float restoreAP;
}
