using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using System;

public class SpellTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private PlayerDeckManager playerDeckManager;
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

    private void Start()
    {
        playerDeckManager = FindAnyObjectByType<PlayerDeckManager>();
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
        SpellData spell = playerDeckManager.findspell(typedBuffer);
        

        if (spell == null)
        {
            Debug.Log("No such spell exists.");
            return;
        }

        if (player.currentAP < spell.spellCost)
        {
            Debug.Log("Not enough AP.");
            if (player.currentAP <= 0)
            {
                OnPlayerFinished?.Invoke();
            }
            return;
        }

        if (!handManager.HasCardInHand(typedBuffer))
        {
            Debug.Log("You don't have that spell card in your hand!");
            return;
        }

        // Proceed to cast
        player.UseAP(spell.spellCost);
        GameObject obj = Instantiate(spell.spellPrefab, castPoint.position, Quaternion.identity);

        Attack atk = obj.GetComponent<Attack>();
        if (atk != null)
        {
            player.AttackAnim();
            Vector2 dir = Vector2.right;
            atk.Initialize(spell.spellDamage, spell.spellSpeed, dir, Caster.Player);
        }

        handManager.OnPlayerTyped(typedBuffer);
    }

}
