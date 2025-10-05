using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using System;

public class SpellTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private SpellBook spellBook;
    [SerializeField] private Transform castPoint;
    [SerializeField] private HandManager handManager;
    private string typedBuffer = "";
    public Player player;
    public event Action OnPlayerFinished;

    void Awake()
    {
        inputDisplay = GameObject.Find("Word").GetComponent<TextMeshProUGUI>();
        handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

    void Update()
    {
        CheckInput();
    }

    public void CheckInput()
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
        SpellData spell = spellBook.GetSpell(typedBuffer);

        if (spell.spellCost <= player.currentAP)
        {
            if (spellBook.HasSpell(typedBuffer))
            {
                
                player.UseAP(spell.spellCost);
                GameObject obj = Instantiate(spell.spellPrefab, castPoint.position, Quaternion.identity);

                Attack atk = obj.GetComponent<Attack>();
                if (atk != null)
                {
                    Vector2 dir = Vector2.right;
                    atk.Initialize(spell.spellDamage, spell.spellSpeed, dir);
                }
                handManager.OnPlayerTyped(typedBuffer);
            }
            else
            {
                
            }
        }
        else
        {
            if (player.currentAP <= 0)
            {
                OnPlayerFinished?.Invoke();
            }
            Debug.Log("Not Enough AP");
        }
    }
}
