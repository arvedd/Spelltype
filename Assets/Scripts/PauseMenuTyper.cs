using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputDisplay;
    private string typedBuffer = "";

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            this.enabled = false;
        }
    }

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
                Return(typedBuffer);

            else if (keysPressed.Length == 1 && char.IsLetter(keysPressed[0]))
                typedBuffer += keysPressed;

            if (inputDisplay != null)
                inputDisplay.text = typedBuffer;
        }
    }

    public void ResetBuffer()
    {
        typedBuffer = "";
        if (inputDisplay != null)
            inputDisplay.text = "";
    }


    void Return(string typedBuffer)
    {
        if (typedBuffer == "menu" || typedBuffer == "Menu")
        {
            PauseManager.IsPaused = false;

            PauseManager pm = FindAnyObjectByType<PauseManager>();
            if (pm != null)
            pm.ResumeGame();

            SceneManager.LoadScene("MainMenu");
        }

        if (typedBuffer == "restartgame")
        {
            PauseManager.IsPaused = false;
            Ending.isResetting = true;

            PauseManager pm = FindAnyObjectByType<PauseManager>();
            if (pm != null)
            pm.ResumeGame();

            PlayerPrefs.SetInt("PlayerHP", 100);
            PlayerPrefs.DeleteKey("PlayerLevel");
            PlayerPrefs.DeleteKey("PlayerGold");
            PlayerPrefs.DeleteKey("InventoryData");
            PlayerPrefs.Save();

            SceneManager.LoadScene("MainMenu");
        }
        
    }

    IEnumerator Delayed()
    {
        yield return new WaitForSeconds(1f);
    }
}
