using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthbar;

    public void UpdateHealthBar(int currentHP, int maxHp)
    {
        healthbar.maxValue = maxHp;
        healthbar.value = currentHP;
    }

}
