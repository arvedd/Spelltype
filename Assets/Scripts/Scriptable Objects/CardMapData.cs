using UnityEngine;

public enum MapType {Shop, Mob, Elite, Boss, Rest, Event}

[CreateAssetMenu(fileName = "CardMapData", menuName = "ScriptableObjects/CardMapData")]
public class CardMapData : ScriptableObject
{
    public string mapName;
    public Sprite mapSprite;
    public MapType mapType;
}
