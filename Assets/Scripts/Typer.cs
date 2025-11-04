using TMPro;
using UnityEngine;

public class Typer : MonoBehaviour
{
    public TextMeshProUGUI inputDisplay;
    public TextMeshProUGUI infoText;
    private string typedBuffer = "";

    void Awake()
    {
        if (infoText == null)
        {
            infoText = GameObject.Find("InfoText")?.GetComponent<TextMeshProUGUI>();
        }
            
        if (inputDisplay == null)
        {
            inputDisplay = GameObject.Find("InputDisplay")?.GetComponent<TextMeshProUGUI>();
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
                CheckMapInput();

            else if (keysPressed.Length == 1 && char.IsLetter(keysPressed[0]))
                typedBuffer += keysPressed;

            if (inputDisplay != null)
                inputDisplay.text = typedBuffer;
        }
    }
    
    void CheckMapInput()
    {
        foreach (var map in MapManager.Instance.mapChoices)
        {
            string mapName = map.mapName.Trim().ToLower();
            if (typedBuffer == mapName)
            {
                // StartCoroutine(ShowFeedback($"Entering {map.mapName}...", correctColor));
                // Invoke(nameof(GoToNextMap), 1.5f);
                Debug.Log($"Entering {map.mapName}");
                typedBuffer = "";
                return;
            }
        }
        typedBuffer = "";
    }
}
