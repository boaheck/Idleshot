using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShopFunctionValue {
    public string key, value;
}
public enum ItemType {
    AutoFire,
    ChangePlayerSpeed,
    ChangePlayerBulletFrequency,
    ChangePlayerBulletSpread,
    ChangePlayerBulletDamage,
    ChangePlayerHealth,
    RefillPlayerHealth,

    BuyTurret,
    ChangeTurretSearchSpeed,
    ChangeTurretBulletFrequency,
    ChangeTurretBulletSpread,
    ChangeTurretBulletDamage,
    ChangeTurretHealth,
    RefillTurretHealth,

	OpenDoor,
	FixShip,
    
}
[System.Serializable]
public struct ShopItem {
    public string id;
    public string name;
    public string description;
    public string depends;
    public Sprite thumbnail;
    public int shellCost;
    public int jellyCost;
	public int partsCost;
    public ItemType type;
    public bool consumable;
    [SerializeField]
    public List<ShopFunctionValue> functionValues;
}

[CreateAssetMenu(fileName = "ShopItems", menuName = "Shop/Items", order = 1)]
public class ShopItems : ScriptableObject {
    public GameObject turretPrefab;
    public List<ShopItem> shopItems;

}
