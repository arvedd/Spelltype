using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RewardTyper : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI inputDisplay;
    public Transform rewardCardParent;
    public GameObject cardPrefab;

    [Header("Reward Logic")]
    public PlayerDeckManager playerDeckManager;
    public string nextSceneName = "NextScene";

    private List<SpellData> rewardSpells = new List<SpellData>();
    private string currentInput = "";
    private Color defaultColor;
    private Color wrongColor = Color.red;
    private Color correctColor = Color.green;
    private float colorResetDelay = 1f;

    void Awake()
    {
        
        if (infoText == null)
            infoText = GameObject.Find("InfoText")?.GetComponent<TextMeshProUGUI>();
        if (inputDisplay == null)
            inputDisplay = GameObject.Find("Word")?.GetComponent<TextMeshProUGUI>();
        if (rewardCardParent == null)
        {
            GameObject parent = GameObject.Find("RewardCardParent");
            if (parent != null) rewardCardParent = parent.transform;
        }
        if (playerDeckManager == null)
            playerDeckManager = FindAnyObjectByType<PlayerDeckManager>();
        if (cardPrefab == null)
            cardPrefab = Resources.Load<GameObject>("Prefabs/CardPrefab");

        defaultColor = infoText.color;
    }

    void Start()
    {
        GenerateRewardChoices();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                ValidateSpellInput();
            }
            else if (char.IsLetter(c))
            {
                currentInput += c;
            }

            inputDisplay.text = currentInput;
        }
    }

    void GenerateRewardChoices()
    {
        if (playerDeckManager == null || playerDeckManager.allCards.Count == 0)
            return;

        rewardSpells.Clear();

        
        for (int i = 0; i < 3; i++)
        {
            SpellData spell = playerDeckManager.allCards[Random.Range(0, playerDeckManager.allCards.Count)];
            rewardSpells.Add(spell);

            GameObject cardObj = Instantiate(cardPrefab, rewardCardParent);
            CardDispay cd = cardObj.GetComponent<CardDispay>();
            if (cd != null)
            {
                cd.spellData = spell;
                cd.UpdateCard();
            }

            
            float spacing = 350f;
            cardObj.transform.localPosition = new Vector3((i - 1) * spacing, 0, 0);
        }

        infoText.text = "Type the spell name to claim your reward";
        inputDisplay.text = "";
    }

    void ValidateSpellInput()
    {
        if (string.IsNullOrEmpty(currentInput))
            return;

        string userInput = currentInput.Trim().ToLower();

        foreach (var s in rewardSpells)
        {
            string spellName = s.spellName.Trim().ToLower();
            if (userInput == spellName)
            {
                StartCoroutine(ShowFeedback($"You got {s.spellName}!", correctColor));
                playerDeckManager.AddCardToDeck(s.spellName);
                Invoke(nameof(LoadNextScene), 1.5f);
                currentInput = "";
                return;
            }
        }

        StartCoroutine(ShowFeedback("Wrong spell! Try again...", wrongColor));
        currentInput = "";
    }

    IEnumerator ShowFeedback(string message, Color color)
    {
        infoText.text = message;
        infoText.color = color;
        yield return new WaitForSeconds(colorResetDelay);
        infoText.text = "Type the spell name to claim your reward";
        infoText.color = defaultColor;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
