using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("UI")]
    public GameObject rewardPopup;
    public TMP_InputField inputField;
    public TMP_Text infoText;
    public PlayerLevel playerLevel;
    public EnemyType enemyType;

    [Header("Player Reference")] 
    public GameObject playerInstance;
    [Header("Card Spawn")]
    public Transform cardParent;
    public GameObject cardPrefab;

    [Header("Reward Settings")]
    public int numberOfCards = 3;

    [Header("Spell Database (ISI MANUAL)")]
    public List<SpellData> spellDatabase;   

    private List<SpellData> rewardCards = new List<SpellData>();
    private SpellData currentCard;
    private int currentIndex = 0;

    [Header("Scene Settings")]
    public string nextSceneName = "MapSelection";

    private bool isTyping = false;
    
    void Awake()
    {
        Instance = this;

        if (rewardPopup != null)
            rewardPopup.SetActive(false);

        if (playerInstance != null)
    {
        playerLevel = playerInstance.GetComponentInChildren<PlayerLevel>();
        if (playerLevel == null)
        {
            Debug.LogError("PlayerLevel component not found in children of playerInstance!");
        }
    }
    }

    
    public void StartRewardSequence()
    {
        Debug.Log("Reward sequence started!");

        rewardPopup.SetActive(true);
        GenerateCards();

        currentIndex = 0;
        ShowNextCard();

        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();

        isTyping = true;
    }   

    void GenerateCards()
    {
        rewardCards.Clear();

        if (spellDatabase == null || spellDatabase.Count == 0)
        {
            Debug.LogError("❌ spellDatabase kosong! Isi SpellData di RewardManager.");
            return;
        }

        rewardCards.AddRange(GetRandomSpells(numberOfCards));

        // Bersihkan parent
        foreach (Transform t in cardParent)
            Destroy(t.gameObject);

        // --- Spacing mirip RewardTyper ---
        float spacing = 350f;
        int total = rewardCards.Count;
        float startX = -(spacing * (total - 1) / 2f);

        for (int i = 0; i < total; i++)
        {
            SpellData spell = rewardCards[i];

            GameObject c = Instantiate(cardPrefab, cardParent);
            CardDispay cd = c.GetComponent<CardDispay>();

            if (cd != null)
            {
                cd.spellData = spell;
                cd.UpdateCard();
            }

            // Posisi identik dengan RewardTyper
            float posX = startX + (i * spacing);
            c.transform.localPosition = new Vector3(posX, 0, 0);
        }
    }


    List<SpellData> GetRandomSpells(int count)
        {
    
    List<SpellData> all = spellDatabase.FindAll(
        s => !SpellBook.Instance.unlockedSpells.Contains(s)
        );

    List<SpellData> pick = new List<SpellData>();

    for (int i = 0; i < count && all.Count > 0; i++)
        {
        int r = Random.Range(0, all.Count);
        pick.Add(all[r]);
        all.RemoveAt(r);   
        }

    return pick;
    }


    void Update()
    {
        if (!isTyping) return;

        if (Input.GetKeyDown(KeyCode.Return))
            ValidateWord();
    }

    void ShowNextCard()
    {
        
        string prompt = "Type the name of ONE of the displayed spells to claim it:";

        infoText.text = prompt;

        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }
    void ValidateWord()
    {
        string typed = inputField.text.Trim();
        
        SpellData claimedSpell = null;

        // Loop melalui SEMUA kartu reward yang tersedia
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
            // Klaim berhasil
            SpellBook.Instance.UnlockSpell(claimedSpell);
            Debug.Log($"🎉 Spell claimed: {claimedSpell.spellName}");

            // Tampilkan pesan reward
            infoText.text = $"🎉 You have obtained the spell: <b>{claimedSpell.spellName}</b>!";

            // Delay sebelum keluar popup
            StartCoroutine(EndRewardDelay());
        }

        else
        {
            
            infoText.text = $"❌ Wrong! The word '{typed}' does not match any available spell. Try again.";
            
            inputField.text = "";
            inputField.Select();
        }
    }

    void EndReward()
    {
        isTyping = false;
        rewardPopup.SetActive(false);

        GetGoldReward();

        if (playerLevel.currentLevel > 5)
            {
                StartCoroutine(ChangeSceneAfterBattle("Ending"));
            }
             else
            {
                StartCoroutine(ChangeSceneAfterBattle("MapSelection"));
            
            }

    }
    void GetGoldReward()
    {
        int goldReward = 0;

            switch (enemyType)
            {
                case EnemyType.Normal:
                goldReward = UnityEngine.Random.Range(70, 91);
                break;

                case EnemyType.Elite:
                goldReward = UnityEngine.Random.Range(91, 121);
                break;

                case EnemyType.Boss:
                goldReward = 150;
                break;
            }

            GoldManager.AddGold(goldReward);

            Debug.Log("Gold Reward: " + goldReward);
            Debug.Log("Total Gold: " + GoldManager.GetGold());

            return;
    }
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
