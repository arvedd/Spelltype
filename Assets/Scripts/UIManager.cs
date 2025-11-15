using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI attackPoin;
    public TextMeshProUGUI playerHP;
    public TMP_Text goldText;
    public GameObject turnText;
    public GameObject cards;
    public GameObject bgInput;
    public GameObject spellTyperObject;
    public GameObject[] apIcons;
    public GameObject[] hpIcons;
    public Player player;
    public SpellTyper spellTyper;

    void Start()
    {
        spellTyper = FindAnyObjectByType<SpellTyper>();
        if (spellTyper != null)
        {
            spellTyperObject = spellTyper.gameObject;
        }

        UpdateAPIcons();
        UpdateHPIcons();
    }

    void Update()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
            return;
        }

        goldText.text = GoldManager.GetGold().ToString();
        UpdateAttackPoin();
        UpdateHP();
    }

    void UpdateAttackPoin()
    {
        attackPoin.text = player.currentAP.ToString();
        UpdateAPIcons();
    }

    void UpdateHP()
    {
        playerHP.text = player.currentHP.ToString();
        UpdateHPIcons();
    }

    void UpdateAPIcons()
    {
        foreach (var icon in apIcons)
        {
            if (icon != null) icon.SetActive(false);
        }

        int index = Mathf.Clamp(5 - player.currentAP, 0, apIcons.Length - 1);
        if (apIcons[index] != null)
            apIcons[index].SetActive(true);
    }

    void UpdateHPIcons()
    {
        foreach (var icon in hpIcons)
        {
            if (icon != null) icon.SetActive(false);
        }

        int index = Mathf.Clamp((100 - player.currentHP) / 10, 0, hpIcons.Length - 1);

        if (hpIcons[index] != null)
            hpIcons[index].SetActive(true);
    }

    public void DisableBattleUI()
    {
        turnText.SetActive(false);
        cards.SetActive(false);
        bgInput.SetActive(false);
        spellTyperObject.SetActive(false);
        Debug.Log("Panggil DisableUI di UIManager");
    }

    public void EnableBattleUI()
    {
        turnText.SetActive(true);
        cards.SetActive(true);
        bgInput.SetActive(true);
        spellTyperObject.SetActive(true);
        Debug.Log("Panggil EnableUI di UIManager");
    }
}
