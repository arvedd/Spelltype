using System.Collections;
using TMPro;
using UnityEngine;

public class ShopBook : MonoBehaviour
{
    public GameObject shopBookOpen;
    public GameObject shopBookStatic;
    public GameObject shopBookNextPage;
    public GameObject shopBookPrevPage;
    public GameObject targetItem1;
    public GameObject targetItem2;
    public GameObject targetItem3;
    public GameObject targetButtonTexts;
    public GameObject targetButtons;
    public GameObject targetTitle1;
    public GameObject goldText;
    public GameObject goldIcon;
    // public GameObject targetTitle2;

    void Awake()
    {
        shopBookStatic.SetActive(false);
    }

    void Start()
    {
        GameObject[] toDeactive = { shopBookStatic, shopBookNextPage, shopBookPrevPage,
                                    targetItem1, targetItem2, targetItem3, targetButtons,
                                    targetButtonTexts, targetTitle1, goldText, goldIcon};

        GameObject[] toActive = { shopBookOpen };

        foreach (GameObject obj in toDeactive)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (GameObject obj in toActive)
        {
            if (obj != null) obj.SetActive(true);
        }

        

        Invoke("DeactiveBook", 1f);

    }

    void DeactiveBook()
    {
        shopBookOpen.SetActive(false);
        ActivateStaticBook(true);
        targetButtons.SetActive(true);
        targetButtonTexts.SetActive(true);
        targetTitle1.SetActive(true);
        goldText.SetActive(true);
        goldIcon.SetActive(true);
    }

    public IEnumerator NextPage()
    {
        goldIcon.SetActive(true);
        goldText.SetActive(true);
        targetTitle1.SetActive(true);
        ActivateStaticBook(false);
        shopBookNextPage.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        ActivateStaticBook(true);
        shopBookNextPage.SetActive(false);
    }

    public IEnumerator PrevPage()
    {
        goldIcon.SetActive(true);
        goldText.SetActive(true);
        targetTitle1.SetActive(true);
        ActivateStaticBook(false);
        shopBookPrevPage.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        ActivateStaticBook(true);
        shopBookPrevPage.SetActive(false);
    }

    void ActivateStaticBook(bool flag)
    {
        shopBookStatic.SetActive(flag);
        targetItem1.SetActive(flag);
        targetItem2.SetActive(flag);
        targetItem3.SetActive(flag);
        // targetItem4.SetActive(flag);
        // targetItem5.SetActive(flag);
        // targetItem6.SetActive(flag);
    }
}
