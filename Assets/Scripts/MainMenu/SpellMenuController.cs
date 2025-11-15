using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SpellMenuController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text inputDisplay;
    public AudioSource acceptSound;
    private string typedBuffer = "";
    private string[] validSpells = { "start", "credit", "exit" };

    void Update()
    {
        InputString();
    }

    void InputString()
    {
        if (Input.anyKeyDown)
        {
            string keysPressed = Input.inputString.ToLower();

            if (keysPressed == "\b" && typedBuffer.Length > 0)
                typedBuffer = typedBuffer.Substring(0, typedBuffer.Length - 1);

            else if (keysPressed == "\n" || keysPressed == "\r")
                CheckInput(typedBuffer);

            else if (keysPressed.Length == 1 && char.IsLetter(keysPressed[0]))
                typedBuffer += keysPressed;

            if (inputDisplay != null)
                inputDisplay.text = typedBuffer;
        }
    }


    void CheckInput(string spell)
    {
        spell = spell.ToLower().Trim();
        if (System.Array.Exists(validSpells, s => s == spell))
        {
            switch (spell)
            {
                case "start":
                    PlaySound();
                    StartCoroutine(LoadSceneAfterDelay("MapSelection", 1f));
                    Debug.Log("start");
                    break;
                case "credit":
                    PlaySound();
                    StartCoroutine(LoadSceneAfterDelay("Credit", 1f));
                    break;
                case "exit":
                    PlaySound();
                    Application.Quit();
                    break;
            }
            typedBuffer = "";
        }

        inputDisplay.text = "";
        typedBuffer = "";
    }

    void PlaySound()
    {
        acceptSound.Play();
    }
    
    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
