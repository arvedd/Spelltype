using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;  
    public GameObject card;
    public GameObject input;
    public GameObject word;
    public GameObject turnText;
    public GameObject pointer;
    public AudioSource pointerSound;
    public PauseMenuTyper pauseMenuTyper;
    private SpellTyper spellTyper;
    private bool isPaused = false;
    public static bool IsPaused = false;

    void Start()
    {
        pauseMenuTyper.enabled = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private SpellTyper GetSpellTyper()
    {
        if (spellTyper == null)
            spellTyper = FindAnyObjectByType<SpellTyper>();

        return spellTyper;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        pauseMenuTyper.enabled = true;

        if (pointerSound != null)
            pointerSound.enabled = false;

        if (pointer != null)      
            pointer.SetActive(false);

        var st = GetSpellTyper();
        if (st != null)
            st.gameObject.SetActive(false);

        if (pauseMenuTyper != null)
            pauseMenuTyper.ResetBuffer();

        isPaused = true;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);

        if (card != null)      
            card.SetActive(true);

        if (input != null)     
            input.SetActive(true);

        if (word != null)      
            word.SetActive(true);

        if (turnText != null)  
            turnText.SetActive(true);

        if (pointer != null)      
            pointer.SetActive(true);    

        if (pointerSound != null)
            pointerSound.enabled = true;

        pauseMenuTyper.enabled = false;

        var st = GetSpellTyper();
        if (st != null)
            st.gameObject.SetActive(true);

        isPaused = false;
        IsPaused = false;
    }
}
