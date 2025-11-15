using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Spell Audio Sources")]
    public AudioSource spellCastSource;
    public AudioSource spellImpactSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySpellSFX(SpellData spell, bool isImpact)
    {
        if (spell == null) return;

        if (isImpact)
        {
            if (spell.impactSFX != null)
            {
                spellImpactSource.clip = spell.impactSFX;
                spellImpactSource.Play();
            }
        }
        else
        {
            if (spell.castSFX != null)
            {
                spellCastSource.clip = spell.castSFX;
                spellCastSource.Play();
            }
        }
    }
}
