using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static int GetGold()
    {
        return PlayerPrefs.GetInt("PlayerGold", 0); 
    }

    public static void AddGold(int amount)
    {
        int current = GetGold();
        current += amount;
        PlayerPrefs.SetInt("PlayerGold", current);
        PlayerPrefs.Save();
    }

    public static void SetGold(int amount)
    {
        PlayerPrefs.SetInt("PlayerGold", amount);
        PlayerPrefs.Save();
    }

    
}
