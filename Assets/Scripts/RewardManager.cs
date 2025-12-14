using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("UI")]
    public GameObject rewardPopup;
    public TMP_InputField inputField;
    public TMP_Text infoText;
    public TMP_Text CoinText;
    public Image CoinImage;

    [Header("Player Reference")]
    public GameObject playerInstance;
    public PlayerLevel playerLevel;
    public SpellTyper spellTyper;     

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Card Spawn")]
    public Transform cardParent;
    public GameObject cardPrefab;

    [Header("Reward Settings")]
    public int numberOfCards = 3;

    [Header("Spell Database (ISI MANUAL)")]
    public List<SpellData> spellDatabase;
    private List<SpellData> rewardCards = new List<SpellData>();

    [Header("Scene Settings")]
    public string nextSceneName = "MapSelection";

    private bool isTyping = false;


    // ---------------------------------------------------------
    // Awake
    // ---------------------------------------------------------
    void Awake()
    {
        Instance = this;

        if (rewardPopup != null)
            rewardPopup.SetActive(false);

        if (playerInstance != null)
        {
            playerLevel = playerInstance.GetComponentInChildren<PlayerLevel>();

            if (playerLevel == null)
                Debug.LogError("PlayerLevel component not found!");
        }
    }


    // ---------------------------------------------------------
    // Start
    // ---------------------------------------------------------


    // ---------------------------------------------------------
    // Start Reward
    // ---------------------------------------------------------
    public void StartRewardSequence()
    {
        rewardPopup.SetActive(true);

        var st = FindSpellTyper();
        if (st != null)
            st.gameObject.SetActive(false);

        GenerateCards();
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();

        isTyping = true;
        ShowNextCard();
    }



    // ---------------------------------------------------------
    // Generate Reward Cards
        // ---------------------------------------------------------
    private SpellTyper FindSpellTyper()
    {
        if (spellTyper == null)
            spellTyper = FindAnyObjectByType<SpellTyper>();

        return spellTyper;
    }
    void GenerateCards()
    {
        rewardCards.Clear();

        if (spellDatabase == null || spellDatabase.Count == 0)
        {
            Debug.LogError("spellDatabase kosong!");
            return;
        }

        
        rewardCards.AddRange(GetRandomSpells(numberOfCards));

        
        foreach (Transform t in cardParent)
            Destroy(t.gameObject);

        
        float spacing = 350f;
        int total = rewardCards.Count;
        float startX = -(spacing * (total - 1) / 2f);

        for (int i = 0; i < total; i++)
        {
            SpellData spell = rewardCards[i];

            GameObject cardObj = Instantiate(cardPrefab, cardParent);
            CardDispay cardDisplay = cardObj.GetComponent<CardDispay>();

            if (cardDisplay != null)
            {
                cardDisplay.spellData = spell;
                cardDisplay.UpdateCard();
            }

            float posX = startX + (i * spacing);
            cardObj.transform.localPosition = new Vector3(posX, 0, 0);
        }
    }



    // ---------------------------------------------------------
    // Pick Random Spells (yang belum dimiliki)
    // ---------------------------------------------------------
    List<SpellData> GetRandomSpells(int count)
    {
        List<SpellData> available = spellDatabase.FindAll(
            s => !SpellBook.Instance.unlockedSpells.Contains(s)
        );

        List<SpellData> selected = new List<SpellData>();

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int r = Random.Range(0, available.Count);
            selected.Add(available[r]);
            available.RemoveAt(r);
        }

        return selected;
    }


    // ---------------------------------------------------------
    // Update
    // ---------------------------------------------------------
    void Update()
    {
        if (!isTyping) return;

        if (Input.GetKeyDown(KeyCode.Return))
            ValidateWord();
    }


    // ---------------------------------------------------------
    // Prompt Text
    // ---------------------------------------------------------
    void ShowNextCard()
    {
        infoText.text = "Type One Spell that You Want to Claim:";
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }


    // ---------------------------------------------------------
    // Validate Input
    // ---------------------------------------------------------
    void ValidateWord()
    {
        string typed = inputField.text.Trim();
        SpellData claimedSpell = null;

        foreach (var spell in rewardCards)
        {
            if (typed.Equals(spell.spellName, System.StringComparison.OrdinalIgnoreCase))
            {
                claimedSpell = spell;
                break;
            }
        }

        if (claimedSpell != null)
        {
            SpellBook.Instance.UnlockSpell(claimedSpell);

            Debug.Log($"Spell claimed: {claimedSpell.spellName}");

            infoText.text =
                $"You have obtained the spell: <b>{claimedSpell.spellName}</b>!";

            StartCoroutine(EndRewardDelay());
        }
        else
        {
            infoText.text = $"Wrong! '{typed}' does not match any available spell.";
            inputField.text = "";
            inputField.Select();
        }
    }


    // ---------------------------------------------------------
    // End Reward
    // ---------------------------------------------------------
    void EndReward()
    {
        isTyping = false;
        rewardPopup.SetActive(false);

        FindSpellTyper();

        var st = FindSpellTyper();
        if (st != null)
            st.gameObject.SetActive(false);

        GetGoldReward();

        if (playerLevel.currentLevel > 5)
            StartCoroutine(ChangeSceneAfterBattle("Ending"));
        else
            StartCoroutine(ChangeSceneAfterBattle("MapSelection"));
    }


    // ---------------------------------------------------------
    // Gold Reward
    // ---------------------------------------------------------
    void GetGoldReward()
    {
        int goldReward = enemyType switch
        {
            EnemyType.Normal => Random.Range(70, 91),
            EnemyType.Elite => Random.Range(91, 121),
            EnemyType.Boss => 150,
            _ => 0
        };
        CoinText.text = "" + goldReward;
        GoldManager.AddGold(goldReward);

        Debug.Log("Gold Reward: " + goldReward);
        Debug.Log("Total Gold: " + GoldManager.GetGold());
    }


    // ---------------------------------------------------------
    // Scene Change
    // ---------------------------------------------------------
    public IEnumerator ChangeSceneAfterBattle(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator StartRewardWithDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartRewardSequence();
    }

    IEnumerator EndRewardDelay()
    {
        yield return new WaitForSeconds(1.5f);
        EndReward();
    }
}
