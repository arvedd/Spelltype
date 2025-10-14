using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDispay : MonoBehaviour
{
    public SpellData spellData;
    public UnityEngine.UI.Image cardImage;
    public UnityEngine.UI.Image elementImage;
    public TMP_Text spellText;
    public TMP_Text damageText;
    public UnityEngine.UI.Image[] typeImages;

    void Start()
    {
        UpdateCard();
    }

    public void UpdateCard()
    {
        spellText.text = spellData.spellName;
        damageText.text = spellData.spellDamage.ToString();
        elementImage.sprite = spellData.elementImage;
    }

}
