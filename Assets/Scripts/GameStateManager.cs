using UnityEngine;
using System;

public enum GameState
{
    Battle,
    Inventory,
    Shop,
    ShopInventory,
    MapSelection,
    MapInventory,
    Paused
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    
    [Header("Current State")]
    public GameState currentState = GameState.Battle;
    
    public static event Action<GameState, GameState> OnStateChanged;

    private GameState previousState;

    public UIManager uIManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        DetectInitialState();
    }

    void Update()
    {
        HandleStateInput();
    }

    void DetectInitialState()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (sceneName.Contains("MAP 1 (IMP)"))
        {
            SetState(GameState.Battle);
        }
        else if (sceneName.Contains("Shop"))
        {
            SetState(GameState.Shop);
        }
        else if (sceneName.Contains("MapSelection"))
        {
            SetState(GameState.MapSelection);
        }
    }

    void HandleStateInput()
    {
        if (PauseManager.IsPaused) return; 

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    void ToggleInventory()
    {
        switch (currentState)
        {
            case GameState.Battle:
                SetState(GameState.Inventory);
                uIManager.DisableBattleUI();
                break;

            case GameState.Inventory:
                SetState(GameState.Battle);
                uIManager.EnableBattleUI();
                break;
            
            case GameState.Shop:
                SetState(GameState.ShopInventory);
                break;
            
            case GameState.ShopInventory:
                SetState(GameState.Shop);
                break;
            
            case GameState.MapSelection:
                SetState(GameState.MapInventory);
                break;
            
            case GameState.MapInventory:
                SetState(GameState.MapSelection);
                break;
        }
    }

    void CloseInventory()
    {
        switch (currentState)
        {
            case GameState.Inventory:
                SetState(GameState.Battle);
                break;
            
            case GameState.ShopInventory:
                SetState(GameState.Shop);
                break;
            
            case GameState.MapInventory:
                SetState(GameState.MapSelection);
                break;
        }
    }

    public void SetState(GameState newState)
    {
        if (currentState == newState) return;

        previousState = currentState;
        currentState = newState;
        OnStateChanged?.Invoke(previousState, currentState);
    }

    public bool IsInventoryOpen()
    {
        return currentState == GameState.Inventory || currentState == GameState.ShopInventory || currentState == GameState.MapInventory;
    }

    public bool IsInBattle()
    {
        return currentState == GameState.Battle;
    }

    public bool IsInShop()
    {
        return currentState == GameState.Shop || currentState == GameState.ShopInventory;
    }

    public bool CanUseItems()
    {
        return currentState == GameState.Inventory;
    }

    public bool CanBuyItems()
    {
        return currentState == GameState.Shop;
    }
}