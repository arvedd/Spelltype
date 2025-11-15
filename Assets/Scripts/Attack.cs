using System;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using TMPro;

public enum Caster { Player, Enemy }

public class Attack : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector2 direction;
    public GameObject Explosion;
    public SpellData spellData;
    public Caster caster;

    [Header("Counter System")]
    [SerializeField] private TextMeshPro counterTextInstance;
    private string activeCounterWord;
    private int counterProgress = 0;
    public float TextOffest = 0.6f;
    private SpellTyper spellTyper;

    public static event Action<Attack> OnAttackDestroyed;

    public void Initialize(int dmg, float spd, Vector2 dir, Caster cstr)
    {
        damage = dmg;
        speed = spd;
        direction = dir.normalized;
        caster = cstr;
    }

    void Start()
    {
        if (caster == Caster.Enemy && spellData != null && spellData.counterWords.Length > 0)
        {
            activeCounterWord = spellData.counterWords[UnityEngine.Random.Range(0, spellData.counterWords.Length)];
            counterTextInstance.text = activeCounterWord;
            // counterTextInstance.alignment = TextAlignmentOptions.Center;
            counterTextInstance.gameObject.SetActive(true);
        }
        else if (counterTextInstance != null)
        {
            counterTextInstance.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Keep text slightly above spell
        if (counterTextInstance != null)
            counterTextInstance.transform.localPosition = new Vector3(0f, TextOffest, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (caster == Caster.Player)
        {
            SpellTyper st = FindAnyObjectByType<SpellTyper>();
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (Explosion != null)
                {
                    AudioManager.Instance.PlaySpellSFX(spellData, true);
                    Instantiate(Explosion, transform.position, transform.rotation);
                }
                enemy.SpellDamage(spellData);
                Destroy(gameObject);

                if (st != null)
                    st.SetAttackFlag(false);
            }
        }
        else if (caster == Caster.Enemy)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (Explosion != null)
                {
                    AudioManager.Instance.PlaySpellSFX(spellData, true);
                    Instantiate(Explosion, transform.position, transform.rotation);
                }
                player.TakeDamage(damage);
                OnAttackDestroyed?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }

    public bool TryCounterProgressive(char typedChar)
    {
        if (caster != Caster.Enemy || string.IsNullOrEmpty(activeCounterWord))
            return false;

        // prevent index overflow
        if (counterProgress >= activeCounterWord.Length)
            return false;

        char expected = activeCounterWord[counterProgress];
        if (char.ToLower(typedChar) == char.ToLower(expected))
        {
            counterProgress++;
            HighlightLetter(counterProgress);

            // fully matched word
            if (counterProgress >= activeCounterWord.Length)
            {
                Debug.Log($"Countered spell {activeCounterWord}!");

                if (Explosion != null)
                    Instantiate(Explosion, transform.position, transform.rotation);

                OnAttackDestroyed?.Invoke(this); //notify BattleSystem before Destroy

                Destroy(gameObject);
                return true;
            }

        }

        return false;
    }

    private void HighlightLetter(int progress)
    {
        if (counterTextInstance == null || string.IsNullOrEmpty(activeCounterWord))
            return;

        // color the correctly typed portion green
        string colored = "<color=#00FF00>" + activeCounterWord.Substring(0, progress) + "</color>";

        // leave the rest white
        if (progress < activeCounterWord.Length)
            colored += activeCounterWord.Substring(progress);

        counterTextInstance.text = colored;
    }
}
