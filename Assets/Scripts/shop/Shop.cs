using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Shop : MonoBehaviour {

    Transform canvasContainer;
    float offScale = 0.05f;
    float onScale = 1f;
    float moveStep = 0.1f;
    Coroutine showShop, hideShop;
    bool inShopRadius = false;
    bool shopOpen = false;
	public GameObject[] doors;
	public GameObject[] extraSpawns;
	int curDoor = 0;

    public ShopItems items;

    public AudioClip startupClip, selectClip;
    private List<string> boughtItems = new List<string>();
    public Transform shopItemUIContainer;
    public GameObject shopItemUIPrefab;
    private Dictionary<string, GameObject> shopItemUIDict = new Dictionary<string, GameObject>();
    private AudioSource audioSource;
    ScoreSystem scoreSystem;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        canvasContainer = GetComponentInChildren<Canvas>().transform.parent;
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        int i = 0;
        foreach (ShopItem item in items.shopItems) {
            GameObject shopItemUI = GameObject.Instantiate(shopItemUIPrefab, shopItemUIContainer.position, shopItemUIContainer.rotation, shopItemUIContainer);
            shopItemUI.name = item.id;
            GameObject child = shopItemUI.transform.GetChild(0).gameObject;
            Button button = child.GetComponent<Button>();
            button.onClick.AddListener(() => {
                BuyUpgrade(item);
            });

            EnableButtonOnScore ebos = shopItemUI.AddComponent<EnableButtonOnScore>();
            ebos.requiredShells = item.shellCost;
            ebos.requiredJelly = item.jellyCost;
			ebos.requiredParts = item.partsCost;
            ebos.depends = item.depends;

            string costString = "";
            if(item.shellCost > 0){
                costString += item.shellCost+" Shells";
            }
            if(item.jellyCost > 0){
                costString += item.jellyCost+" Jelly";
            }
			if(item.partsCost > 0){
				costString += item.partsCost+" Ship Parts";
			}

            Transform siTransform = child.transform;
            siTransform.Find("Image").GetComponent<Image>().sprite = item.thumbnail;
            siTransform.Find("Name").GetComponent<Text>().text = item.name;
            siTransform.Find("Description").GetComponent<Text>().text = item.description;
            siTransform.Find("Cost").GetComponent<Text>().text = costString;

            shopItemUIDict.Add(item.id, shopItemUI);
            i++;
        }
        ScrollRect sr = GetComponentInChildren<ScrollRect>();
        sr.verticalNormalizedPosition = 1f;
    }

    public bool BoughtItem(string id) {
        if (string.IsNullOrEmpty(id)) {
            return true;
        } else {
            return boughtItems.Contains(id);
        }
    }

    void FixedUpdate() {
        if (inShopRadius && Input.GetButtonDown("Submit")) {
            if (shopOpen) {
                StopShowShop();
            } else {
                StartShowShop();
            }
        }
		if (inShopRadius && Input.GetButtonDown ("Cancel")) {
			if (shopOpen) {
				StopShowShop ();
			}
		}
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            inShopRadius = true;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            inShopRadius = false;
            StopShowShop();
        }
    }

    void StartShowShop() {
        audioSource.PlayOneShot(startupClip);
        if (showShop != null) {
            StopCoroutine(showShop);
        }
        if (hideShop != null) {
            StopCoroutine(hideShop);
        }
        showShop = StartCoroutine(ShowShop());
    }

    void StopShowShop() {
        if (hideShop != null) {
            StopCoroutine(hideShop);
        }
        if (showShop != null) {
            StopCoroutine(showShop);
        }
        hideShop = StartCoroutine(HideShop());
    }

    IEnumerator ShowShop() {
        shopOpen = true;
        while (canvasContainer.localScale.x < onScale) {
            canvasContainer.localScale = canvasContainer.localScale + (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
        canvasContainer.localEulerAngles = Vector3.one*onScale;
    }
    IEnumerator HideShop() {
        shopOpen = false;
        while (canvasContainer.localScale.x > offScale) {
            canvasContainer.localScale = canvasContainer.localScale - (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
        canvasContainer.localEulerAngles = Vector3.one*offScale;
    }

    void BuyUpgrade(ShopItem item) {
        ItemType type = item.type;
        List<ShopFunctionValue> functionValues = item.functionValues;
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        foreach (ShopFunctionValue val in functionValues) {
            parameters.Add(val.key, val.value);
        }

        switch (type) {
            case ItemType.AutoFire: {
                    PlayerShooting ps = GameObject.FindObjectOfType<PlayerShooting>();
                    ps.auto = true;
                    break;
                }
            case ItemType.ChangePlayerSpeed: {
                    PlayerMovement pm = GameObject.FindObjectOfType<PlayerMovement>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        pm.speed *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        pm.speed += val;
                    }
                    break;
                }
            case ItemType.ChangePlayerBulletFrequency: {
                    PlayerShooting ps = GameObject.FindObjectOfType<PlayerShooting>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        ps.rate *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        ps.rate += val;
                    }
                    break;
                }
            case ItemType.ChangePlayerBulletSpread: {
                    PlayerShooting ps = GameObject.FindObjectOfType<PlayerShooting>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
					ps.spread -= ps.spread*val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        ps.spread += val;
                    }
                    break;
                }
            case ItemType.ChangePlayerBulletDamage: {
                    PlayerShooting ps = GameObject.FindObjectOfType<PlayerShooting>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        ps.strength *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        ps.strength += val;
                    }
                    break;
                }
            case ItemType.ChangePlayerHealth: {
                    PlayerHealth ph = GameObject.FindObjectOfType<PlayerHealth>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        ph.AddMaxHealthPerc(val);
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        ph.AddMaxHealth(val);
                    }
                    break;
                }
            case ItemType.RefillPlayerHealth: {
                    PlayerHealth ph = GameObject.FindObjectOfType<PlayerHealth>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        ph.Heal(ph.GetHealth() * val);
                    } else if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        ph.Heal(val);
                    } else {
                        ph.FullHeal();
                    }
                    break;
                }

            case ItemType.BuyTurret: {
                    GameObject.Instantiate(items.turretPrefab, transform.position + (Vector3.down * transform.position.y), transform.rotation, transform);
                    break;
                }
            case ItemType.ChangeTurretSearchSpeed: {
                    TurretAI tai = GetComponentInChildren<TurretAI>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        tai.searchSpeed *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        tai.searchSpeed += val;
                    }
                    break;
                }
            case ItemType.ChangeTurretBulletFrequency: {
                    TurretAI tai = GetComponentInChildren<TurretAI>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        tai.rate *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        tai.rate += val;
                    }
                    break;
                }
            case ItemType.ChangeTurretBulletSpread: {
                    TurretAI tai = GetComponentInChildren<TurretAI>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
					tai.spread -= tai.spread*val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        tai.spread += val;
                    }
                    break;
                }
            case ItemType.ChangeTurretBulletDamage: {
                    TurretAI tai = GetComponentInChildren<TurretAI>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        tai.strength *= val;
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        tai.strength += val;
                    }
                    
                    break;
                }
            case ItemType.ChangeTurretHealth: {
                    TurretHealth th = GetComponentInChildren<TurretHealth>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        th.AddMaxHealthPerc(val);
                    }
                    if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        th.AddMaxHealth(val);
                    }
                    break;
                }
            case ItemType.RefillTurretHealth: {
                    TurretHealth th = GetComponentInChildren<TurretHealth>();
                    if (parameters.ContainsKey("percent")) {
                        string p = parameters["percent"];
                        float val = float.Parse(p);
                        th.Heal(th.GetHealth()*val);
                        
                    } else if (parameters.ContainsKey("value")) {
                        string p = parameters["value"];
                        float val = float.Parse(p);
                        th.Heal(val);
                    } else {
                        th.FullHeal();
                    }
                    break;
                }
			case ItemType.OpenDoor:{
				Destroy (doors [curDoor]);
				extraSpawns [curDoor].SetActive (true);
				curDoor++;
				break;
			}
			case ItemType.FixShip:{
				SceneManager.LoadScene ("Ending");
				break;
			}
            default: {
                    break;
                }
        }
        boughtItems.Add(item.id);
        GameObject UIItem = shopItemUIDict[item.id];
        if (item.consumable) {
            GameObject.DestroyImmediate(UIItem);
        }
        audioSource.PlayOneShot(selectClip);
        scoreSystem.AddShells(-item.shellCost);
        scoreSystem.AddJelly(-item.jellyCost);

    }

}