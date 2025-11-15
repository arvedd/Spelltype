using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Ending : MonoBehaviour
{
    void Update()
    {
        EndGame();
    }

    public void EndGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Delayed());
        }
    }

    IEnumerator Delayed()
    {
        yield return new WaitForSeconds(2f);

        PlayerPrefs.SetInt("PlayerHP", 100);
        PlayerPrefs.DeleteKey("PlayerLevel");
        PlayerPrefs.DeleteKey("PlayerGold");
        PlayerPrefs.DeleteKey("InventoryData");
        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }
}
