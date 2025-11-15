using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; set; }

    [Header("UI References")]
    public Transform cardParent;
    public GameObject cardPrefab;
    public TMP_Text goldText;

    [Header("Map Data")]
    public List<CardMapData> availableMaps;
    public List<CardMapData> mapChoices = new List<CardMapData>();

    [Header("Spacing")]
    public float cardSpacing = 350f;

    [Header("Player's Level")]
    public PlayerLevel playerLevel;

    [Header("Others")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private Typer typer;
    private int maxCard = 3;
    private MapType desiredType;
    [SerializeField] private int level;


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

        if (cardParent == null)
        {
            GameObject pos = GameObject.Find("CardContainer");
            if (pos != null)
            {
                cardParent = pos.transform;
            }
        }

        if (cardPrefab == null)
        {
            cardPrefab = Resources.Load<GameObject>("Prefabs/MapPrefab");
        }
    }
    
    void Start()
    {
        goldText.text = GoldManager.GetGold().ToString();
        level = playerLevel.GetLevel();
        GenerateMapChoices();
    }

    void GenerateMapChoices()
    {
        if (availableMaps == null || availableMaps.Count == 0)
        {
            Debug.Log("kosong");
            return;
        }

        mapChoices.Clear();

        // if (level % 10 == 0)
        // {
        //     desiredType = MapType.Boss;
        // }
        // else if (level % 5 == 0)
        // {
        //     desiredType = MapType.Elite;
        // }
        // else
        // {
        //     desiredType = MapType.Mob;
        // }

        if (level == 5)
        {
            desiredType = MapType.Boss;
        }
        else if (level == 3 || level == 4)
        {
            desiredType = MapType.Elite;
        }
        else
        {
            desiredType = MapType.Mob;
        }

        List<CardMapData> mainMaps = availableMaps.FindAll(m => m.mapType == desiredType);
        if (mainMaps.Count == 0)
        {
            Debug.LogWarning($"Tidak ada map bertipe {desiredType} di availableMaps!");
            return;
        }

        CardMapData mainMap = mainMaps[Random.Range(0, mainMaps.Count)];
        mapChoices.Add(mainMap);

        MapType[] extraTypes = { MapType.Rest, MapType.Shop, MapType.Event };
        List<MapType> availableExtraTypes = new List<MapType>(extraTypes);

        for (int i = 0; i < 2; i++)
        {
            if (availableExtraTypes.Count == 0)
                break;

            MapType chosenType = availableExtraTypes[Random.Range(0, availableExtraTypes.Count)];
            availableExtraTypes.Remove(chosenType);

            List<CardMapData> filtered = availableMaps.FindAll(m => m.mapType == chosenType);
            if (filtered.Count > 0)
            {
                CardMapData chosenMap = filtered[Random.Range(0, filtered.Count)];
                mapChoices.Add(chosenMap);
            }   
        }

        ShuffleList(mapChoices);

        for (int i = 0; i < mapChoices.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardParent);
            MapDisplay cd = cardObj.GetComponent<MapDisplay>();
            if (cd != null)
            {
                cd.mapData = mapChoices[i];
                cd.UpdateMap();
            }

            cardObj.transform.localPosition = new Vector3((i - 1) * cardSpacing, 0, 0);
        }

        typer.infoText.text = $"Level {level} - Type the map name to choose your path";
        typer.inputDisplay.text = "";   
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    
    public void GoToNextMap(string buffer)
    {
        if (buffer == "mob" || buffer == "elite")
        {
            SceneManager.LoadScene($"MAP {level} (IMP)");
        }

        if (buffer == "boss")
        {
            SceneManager.LoadScene("Boss");
        }

        if (buffer == "shop")
        {
            SceneManager.LoadScene("Shop");
        }
        
        if (buffer == "rest")
        {
            SceneManager.LoadScene("Rest");
        }
    }
}
