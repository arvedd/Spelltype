using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class SpellTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private PlayerDeckManager playerDeckManager;
    [SerializeField] private Transform castAttackPoint;
    [SerializeField] private Transform castHealPoint;
    [SerializeField] private HandManager handManager;
    private string typedBuffer = "";
    public Player player;
    public event Action OnPlayerFinished;

    void Awake()
    {
        if (inputDisplay == null)
            inputDisplay = GameObject.Find("Word")?.GetComponent<TextMeshProUGUI>();

        if (handManager == null)
            handManager = GameObject.Find("HandManager")?.GetComponent<HandManager>();

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

            if (keysPressed == "\b" && typedBuffer.Length > 0)
                typedBuffer = typedBuffer.Substring(0, typedBuffer.Length - 1);

            else if (keysPressed == "\n" || keysPressed == "\r")
                TryCastSpell();

            else if (keysPressed.Length == 1 && char.IsLetter(keysPressed[0]))
                typedBuffer += keysPressed;

            if (inputDisplay != null)
                inputDisplay.text = typedBuffer;
        }
    }

    private void TryCastSpell()
    {
        if (typedBuffer == "end")
        {
            OnPlayerFinished?.Invoke();
            typedBuffer = "";
            if (inputDisplay) inputDisplay.text = "";
            return;
        }

        SpellData spell = playerDeckManager.findspell(typedBuffer);

        if (spell == null)
        {
            Debug.Log("No such spell exists.");
            inputDisplay.color = Color.red;
            StartCoroutine(ResetInputColor());
            return;
        }

        if (player.currentAP < spell.spellCost)
        {
            Debug.Log("Not enough AP.");
            return;
        }

        if (!handManager.HasCardInHand(typedBuffer))
        {
            Debug.Log("You don't have that spell card in your hand!");
            return;
        }

        player.UseAP(spell.spellCost);

        switch(spell.spellCategory)
        {
            case SpellCategory.Attack:
                CastAttackSpell(spell);
                break;
            case SpellCategory.Heal:
                CastHealSpell(spell);
                break;
        }


        handManager.OnPlayerTyped(typedBuffer);

        typedBuffer = "";
        if (inputDisplay) inputDisplay.text = "";
    }

    private void CastAttackSpell(SpellData spell)
    {
        GameObject obj = Instantiate(spell.spellPrefab, castAttackPoint.position, Quaternion.identity);

        Attack atk = obj.GetComponent<Attack>();
        if (atk != null)
        {
            player.AttackAnim();
            Vector2 dir = Vector2.right;
            atk.Initialize(spell.spellDamage, spell.spellSpeed, dir, Caster.Player);
        }
    }
    
    private void CastHealSpell(SpellData spell)
    {
        GameObject healEffect = Instantiate(spell.spellPrefab, castHealPoint.position, Quaternion.identity);
        Animator anim = healEffect.GetComponent<Animator>();

        player.HealAnim();
        anim.Play("CurialHeal");
        
        player.Heal(spell.spellHeal);
        Debug.Log($"Player healed by {spell.spellHeal}!");
        
    }

    private IEnumerator ResetInputColor()
    {
        yield return new WaitForSeconds(0.5f);
        inputDisplay.color = Color.white;
        typedBuffer = "";
        if (inputDisplay) inputDisplay.text = "";
        
    }
}
