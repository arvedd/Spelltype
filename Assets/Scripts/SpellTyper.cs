using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

public class SpellTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private TextMeshProUGUI messageDisplay;
    [SerializeField] private SpellBook spellBook;
    [SerializeField] private Transform castPoint;
    [SerializeField] private HandManager handManager;

    private SpellData[] spell;

    private string typedBuffer = "";

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string keysPressed = Input.inputString.ToLower();

            if (keysPressed == "\b")
            {
                if (typedBuffer.Length > 0)
                    typedBuffer = typedBuffer.Substring(0, typedBuffer.Length - 1);
            }
            else if (keysPressed == "\n" || keysPressed == "\r")
            {
                TryCastSpell();
                typedBuffer = "";
            }
            else if (keysPressed.Length == 1 && char.IsLetter(keysPressed[0]))
            {
                typedBuffer += keysPressed;
            }

            inputDisplay.text = typedBuffer;
        }
    }

    private void TryCastSpell()
    {
        if (spellBook.HasSpell(typedBuffer))
        {
            SpellData spell = spellBook.GetSpell(typedBuffer);
            GameObject obj = Instantiate(spell.spellPrefab, castPoint.position, Quaternion.identity);

            Attack atk = obj.GetComponent<Attack>();
            if (atk != null)
            {
                Vector2 dir = Vector2.right;
                atk.Initialize(spell.spellDamage, spell.spellSpeed, dir);
            }
            handManager.OnPlayerTyped(typedBuffer);
            messageDisplay.text = $"Casting spell: {spell.spellName}!";
        }
        else
        {
            messageDisplay.text = "You haven't unlocked that spell yet!";
        }
    }
}
