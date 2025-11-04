// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class CardMapDisplayManager : MonoBehaviour
// {
//     [Header("UI References")]
//     public TextMeshProUGUI infoText;
//     public TextMeshProUGUI inputDisplay;
//     public Transform cardParent;
//     public GameObject cardPrefab;

//     [Header("Map Data")]
//     public List<CardMapData> availableMaps;
//     public List<CardMapData> mapChoices = new List<CardMapData>();

//     private string currentInput = "";
//     private Color defaultColor;
//     private Color wrongColor = Color.red;
//     private Color correctColor = Color.green;
//     private float colorResetDelay = 1f;

//     public float spacing = 350f;

//     void Awake()
//     {
//         if (infoText == null)
//             infoText = GameObject.Find("InfoText")?.GetComponent<TextMeshProUGUI>();
//         if (inputDisplay == null)
//             inputDisplay = GameObject.Find("Word")?.GetComponent<TextMeshProUGUI>();
//         if (cardParent == null)
//         {
//             GameObject parent = GameObject.Find("CardContainer");
//             if (parent != null) cardParent = parent.transform;
//         }
//         if (cardPrefab == null)
//             cardPrefab = Resources.Load<GameObject>("Prefabs/MapCardPrefab");

//         defaultColor = infoText.color;
//     }

//     void Start()
//     {
//         GenerateMapChoices();
//     }

//     void Update()
//     {
//         foreach (char c in Input.inputString)
//         {
//             if (c == '\b')
//             {
//                 if (currentInput.Length > 0)
//                     currentInput = currentInput.Substring(0, currentInput.Length - 1);
//             }
//             else if (c == '\n' || c == '\r')
//             {
//                 ValidateMapInput();
//             }
//             else if (char.IsLetter(c))
//             {
//                 currentInput += c;
//             }

//             inputDisplay.text = currentInput;
//         }
//     }

    // void GenerateMapChoices()
    // {
    //     if (availableMaps == null || availableMaps.Count == 0)
    //     {
    //         Debug.Log("kosong");
    //         return;
    //     }
            

    //     mapChoices.Clear();

    //     // pilih 3 map secara acak
    //     for (int i = 0; i < 3; i++)
    //     {
    //         CardMapData map = availableMaps[Random.Range(0, availableMaps.Count)];
    //         mapChoices.Add(map);

    //         GameObject cardObj = Instantiate(cardPrefab, cardParent);
    //         MapDisplay cd = cardObj.GetComponent<MapDisplay>();
    //         if (cd != null)
    //         {
    //             cd.mapData = map;
    //             cd.UpdateMap();
    //         }

            
    //         cardObj.transform.localPosition = new Vector3((i - 1) * spacing, 0, 0);
    //     }

    //     infoText.text = "Type the map name to choose your path";
    //     inputDisplay.text = "";
    // }

//     void ValidateMapInput()
//     {
//         if (string.IsNullOrEmpty(currentInput))
//             return;

//         string userInput = currentInput.Trim().ToLower();

//         foreach (var map in mapChoices)
//         {
//             string mapName = map.mapName.Trim().ToLower();
//             if (userInput == mapName)
//             {
//                 StartCoroutine(ShowFeedback($"Entering {map.mapName}...", correctColor));
//                 Invoke(nameof(GoToNextMap), 1.5f);
//                 currentInput = "";
//                 return;
//             }
//         }

//         StartCoroutine(ShowFeedback("Invalid map! Try again...", wrongColor));
//         currentInput = "";
//     }

//     IEnumerator ShowFeedback(string message, Color color)
//     {
//         infoText.text = message;
//         infoText.color = color;
//         yield return new WaitForSeconds(colorResetDelay);
//         infoText.text = "Type the map name to choose your path";
//         infoText.color = defaultColor;
//     }

// //     void GoToNextMap()
// //     {
// //         switch()
// //     }
// }
