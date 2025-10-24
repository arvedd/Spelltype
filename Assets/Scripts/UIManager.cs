using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI attackPoin;
    public Player player;
    
    void Update()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
            return;
        }
        UpdateAttackPoin();
    }

    void UpdateAttackPoin()
    {
        attackPoin.text = player.currentAP.ToString();
    }


}


