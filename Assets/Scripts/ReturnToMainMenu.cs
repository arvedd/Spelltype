using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    void Update()
    {
        MoveToNextMap();
    }

    void MoveToNextMap()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        
    }
}
