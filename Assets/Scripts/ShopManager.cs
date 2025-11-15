using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; set; }

    [Header("Item Data & UI")]
    public List<ShopData> allItems;
    public List<ShopItemDisplay> itemSlots;
    public List<GameObject> pointerSpot;
    public GameObject itemPointer;
    public int totalItems;  
    public TMP_Text goldText;

    [Header("References")]
    public ShopBuy shopBuy;
    public ShopBook shopBook;
                        

    [Header("Pagination Settings")]
    public int itemsPerPage = 2;
    public int currentPage = 0;
    public int selectedIndex = 0;

    [Header("Audio")]
    public AudioSource pointerAudio;
    public AudioSource flipBookAudio;
    public AudioSource buyAudio;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        goldText.text = GoldManager.GetGold().ToString();
        totalItems = itemSlots.Count;
        shopBuy.shopDatas = allItems;
        LoadPage(0);
        UpdatePointerPosition();
    }

    void Update()
    {
        // Jangan izinkan input apa pun saat pause
        if (PauseManager.IsPaused) return;

        goldText.text = GoldManager.GetGold().ToString();

        MoveToNextMap();
        ChangeItemSelection();
        ChangePage();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            int realIndex = currentPage * itemsPerPage + selectedIndex;
            shopBuy.BuyItem(realIndex);
        }
    }


    void ChangeItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PointerSound();
            selectedIndex = selectedIndex - 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            PointerSound();
            selectedIndex = selectedIndex + 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PointerSound();
            selectedIndex = selectedIndex + 2;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PointerSound();
            selectedIndex = selectedIndex - 2;
        } 

        selectedIndex = Mathf.Clamp(selectedIndex, 0, pointerSpot.Count - 1);

        UpdatePointerPosition();
    }

    void ChangePage()
    {
        bool pageChanged = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ((currentPage + 1) * itemsPerPage < allItems.Count)
            {
                FlipBookSound();
                currentPage++;
                LoadPage(currentPage);
                if (shopBook != null)
                    StartCoroutine(shopBook.NextPage());
                pageChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPage > 0)
            {
                FlipBookSound();
                currentPage--;
                LoadPage(currentPage);
                if (shopBook != null)
                    StartCoroutine(shopBook.PrevPage());
                pageChanged = true;
            }
        }
        
        if (pageChanged)
        {
            selectedIndex = 0;
            UpdatePointerPosition();
        }
    }


    void LoadPage(int pageIndex)
    {
        int startIndex = pageIndex * itemsPerPage;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            int dataIndex = startIndex + i;

            if (dataIndex < allItems.Count)
                itemSlots[i].ShowItem(allItems[dataIndex]);
            else
                itemSlots[i].Hide();
        }
    }

    void UpdatePointerPosition()
    {
        if (selectedIndex >= 0 && selectedIndex < itemSlots.Count)
        {
            itemPointer.transform.position = pointerSpot[selectedIndex].transform.position;
        }
    }

    void MoveToNextMap()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SceneManager.LoadScene("MapSelection");
            return;
        }
        
    }


    void PointerSound()
    {
        pointerAudio.Play();
    }

    void FlipBookSound()
    {
        flipBookAudio.Play();
    }

    public void BuySound()
    {
        buyAudio.Play();
    }
}
