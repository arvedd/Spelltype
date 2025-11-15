using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthRestManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject[] hpIcons;
    public TextMeshProUGUI playerHP;

    [Header("Health Data")]
    private int savedHP;
    private int hpToRestore;

    void Start()
    {
        savedHP = PlayerPrefs.GetInt("PlayerHP", 100);

        playerHP.text = savedHP.ToString();
        UpdateHPIcons(savedHP);

        StartCoroutine(Resting());
    }

    void UpdateHPIcons(int currentHP)
    {
        foreach (var icon in hpIcons)
        {
            if (icon != null) icon.SetActive(false);
        }

        int index = Mathf.Clamp((100 - currentHP) / 10, 0, hpIcons.Length - 1);

        if (hpIcons[index] != null)
            hpIcons[index].SetActive(true);
    }

    public IEnumerator Resting()
    {
         yield return new WaitForSeconds(3.5f);

        hpToRestore = Mathf.RoundToInt(savedHP * 0.3f);
        int newHP = Mathf.Min(100, savedHP + hpToRestore);

        PlayerPrefs.SetInt("PlayerHP", newHP);
        PlayerPrefs.Save();

        playerHP.text = newHP.ToString();
        UpdateHPIcons(newHP);

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene("MapSelection");
    }
}
