using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel;

    void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
    }

    void Start()
    {
        Debug.Log($" Current Level: {currentLevel}");
    }

    public void LevelUp()
    {
        currentLevel++;
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.Save();

        if (currentLevel == 6)
        {
            
        }

        Debug.Log($"Level UP! New level = {currentLevel}");
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("PlayerLevel", 1);
        PlayerPrefs.Save();
    }
}
