using UnityEngine;

public class ShopInventoryController : MonoBehaviour
{
    [Header("References")]
    public GameObject shopManager;
    public GameObject inventoryManager; 
    public GameObject shopUI;
    public GameObject inventoryUI;

    [Header("Transition Settings")]
    public float transitionSpeed = 5f;  
    private bool isInventoryOpen = false; 

    private CanvasGroup shopCanvas;
    private CanvasGroup inventoryCanvas;

    void Start()
    {
        if (shopUI != null)
        {
            shopCanvas = shopUI.GetComponent<CanvasGroup>();
            if (shopCanvas == null) shopCanvas = shopUI.AddComponent<CanvasGroup>();
        }

        if (inventoryUI != null)
        {
            inventoryCanvas = inventoryUI.GetComponent<CanvasGroup>();
            if (inventoryCanvas == null) inventoryCanvas = inventoryUI.AddComponent<CanvasGroup>();
        }

        SetActiveUI(shopActive: true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }

        if (shopCanvas != null && inventoryCanvas != null)
        {
            float targetShopAlpha = isInventoryOpen ? 0 : 1;
            float targetInventoryAlpha = isInventoryOpen ? 1 : 0;

            shopCanvas.alpha = Mathf.Lerp(shopCanvas.alpha, targetShopAlpha, Time.deltaTime * transitionSpeed);
            inventoryCanvas.alpha = Mathf.Lerp(inventoryCanvas.alpha, targetInventoryAlpha, Time.deltaTime * transitionSpeed);
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        SetActiveUI(shopActive: !isInventoryOpen);
    }

    void SetActiveUI(bool shopActive)
    {
        if (shopManager != null) shopManager.SetActive(shopActive);
        if (inventoryManager != null) inventoryManager.SetActive(!shopActive);

        if (shopUI != null) shopUI.SetActive(shopActive);
        if (inventoryUI != null) inventoryUI.SetActive(!shopActive);
    }
}
