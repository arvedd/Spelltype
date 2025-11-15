using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDispay : MonoBehaviour
{
    public SpellData spellData;
    public UnityEngine.UI.Image cardImage;
    public UnityEngine.UI.Image elementImage;
    public UnityEngine.UI.Image cardBadge;
    public UnityEngine.UI.Image cardAP;
    public TMP_Text spellText;
    public TMP_Text damageText;
    public TMP_Text APText;
    public UnityEngine.UI.Image[] typeImages;

    void Start()
    {
        UpdateCard();
    }

    public void UpdateCard()
    {
        spellText.text = spellData.spellName;
        damageText.text = spellData.spellDamage.ToString();
        APText.text = spellData.spellCost.ToString();
        elementImage.sprite = spellData.elementImage;
        cardImage.sprite = spellData.cardImage;
        cardBadge.sprite = spellData.cardBadge;
        cardAP.sprite = spellData.cardAP;
    }

}
