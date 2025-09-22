using TMPro;
using UnityEngine;

public class Typer : MonoBehaviour
{
    public TextMeshProUGUI wordOuput = null;
    private string remainWord = string.Empty;
    private string currentWord = "muffins";

    private void Start()
    {
        SetCurrentWord();
    }

    private void SetCurrentWord()
    {
        SetRemainingWord(currentWord);
    }

    private void SetRemainingWord(string newString)
    {
        remainWord = newString;
        wordOuput.text = remainWord;
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string keysPressed = Input.inputString;
            string wordPressed = keysPressed.ToLower();

            if (wordPressed.Length == 1)
            {

                EnterLetter(wordPressed);
            }
        }
    }

    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            RemoveLetter();

            if (IsWordComplete())
            {
                SetCurrentWord();
            }
        }
    }

    private bool IsCorrectLetter(string letter)
    {
        return remainWord.IndexOf(letter) == 0;
    }

    private void RemoveLetter()
    {
        string newString = remainWord.Remove(0, 1);
        SetRemainingWord(newString);
    }

    private bool IsWordComplete()
    {
        return remainWord.Length == 0;
    }
}
