using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct ShopFunctionValue
{
    public string key, value;
}
public enum ItemType
{
    ChangePlayerSpeed,
    ChangeEnemyFrequency,
    ChangePlayerBulletFrequency
}
[System.Serializable]
public struct ShopItem
{
    public string id;
    public string name;
    public ItemType type;
    [SerializeField]
    public List<ShopFunctionValue> functionValues;
}

[CreateAssetMenu(fileName = "ShopItems", menuName = "Shop/Items", order = 1)]
public class ShopItems : ScriptableObject
{
    public List<ShopItem> shopItems;
}
