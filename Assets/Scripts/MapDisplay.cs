using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    public CardMapData mapData;
    public Image cardMapImage;
    public Image mapImage;
    public TMP_Text mapName;

    void Start()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        mapName.text = mapData.mapName;
        mapImage.sprite = mapData.mapSprite;
        
    }
}
