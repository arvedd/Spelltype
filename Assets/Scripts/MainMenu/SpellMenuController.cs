using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpellMenuController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TMP_Text feedbackText;

    private string[] validSpells = { "start", "settings", "exit" };

    void Start()
    {
        inputField.ActivateInputField();
        feedbackText.text = "";
        inputField.onSubmit.AddListener(OnSubmit);
    }


    void OnSubmit(string spell)
    {
        spell = spell.ToLower().Trim();
        if (System.Array.Exists(validSpells, s => s == spell))
        {
            feedbackText.text = $"> {spell.ToUpper()}...";
            switch (spell)
            {
                case "start":
                    StartCoroutine(LoadSceneAfterDelay("LevelSelection", 1f));
                    break;
                case "settings":
                    StartCoroutine(LoadSceneAfterDelay("SettingsScene", 1f));
                    break;
                case "exit":
                    Application.Quit();
                    break;
            }
        }
        else
        {
            feedbackText.text = "> The spell fizzles...";
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
