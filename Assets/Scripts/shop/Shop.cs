using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{

    Transform canvasContainer;
    float offScale = 0.2f;
    float onScale = 1f;
    float moveStep = 0.03f;
    Coroutine showShop, hideShop;
    bool inShopRadius = false;
    bool shopOpen = false;

    public ShopItems items;

    public AudioClip startupClip, selectClip;
    private List<string> boughtItems = new List<string>();
    public Transform shopItemUIContainer;
    public GameObject shopItemUIPrefab;
    private Dictionary<string, GameObject> shopItemUIDict = new Dictionary<string, GameObject>();
    private AudioSource audioSource;
    ScoreSystem scoreSystem;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canvasContainer = GetComponentInChildren<Canvas>().transform.parent;
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        int i = 0;
        foreach (ShopItem item in items.shopItems)
        {
            GameObject shopItemUI = GameObject.Instantiate(shopItemUIPrefab, Vector3.zero, shopItemUIContainer.rotation, shopItemUIContainer);
            shopItemUI.name = item.id;
            Button button = shopItemUI.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                BuyUpgrade(item);
            });

            EnableButtonOnScore ebos = shopItemUI.AddComponent<EnableButtonOnScore>();
            ebos.requiredShells = item.shellCost;
            ebos.requiredJelly = item.jellyCost;
            ebos.depends = item.depends;

            Transform siTransform = shopItemUI.transform;
            siTransform.Find("Image").GetComponent<Image>().sprite = item.thumbnail;
            siTransform.Find("Name").GetComponent<Text>().text = item.name;
            siTransform.Find("Description").GetComponent<Text>().text = item.description;

            RectTransform rtc = shopItemUI.GetComponent<RectTransform>();
            float initialPos = shopItemUIContainer.GetComponent<RectTransform>().rect.yMax - rtc.rect.yMax;
            rtc.localPosition = new Vector3(0, initialPos + (-i * 120), 0);

            shopItemUIDict.Add(item.id, shopItemUI);
            i++;
        }
        GetComponentInChildren<SetBottomToLowestChild>().Set();
    }

    public bool BoughtItem(string id){
        if(string.IsNullOrEmpty(id)){
            return true;
        }
        else {
            return boughtItems.Contains(id);
        }
    }

    void FixedUpdate() {
        if(inShopRadius && Input.GetButtonDown("Submit")){
            if (shopOpen) {
                StopShowShop();
            } else {
                StartShowShop();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            inShopRadius = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            inShopRadius = false;
            StopShowShop();
        }
    }

    void StartShowShop(){
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

    IEnumerator ShowShop()
    {
        shopOpen = true;
        while (canvasContainer.localScale.x < onScale)
        {
            canvasContainer.localScale = canvasContainer.localScale + (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator HideShop()
    {
        shopOpen = false;
        while (canvasContainer.localScale.x > offScale)
        {
            canvasContainer.localScale = canvasContainer.localScale - (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }

    void BuyUpgrade(ShopItem item)
    {
        ItemType type = item.type;
        List<ShopFunctionValue> functionValues = item.functionValues;
        switch (type)
        {
            case ItemType.BuyTurret:
                {
                    GameObject.Instantiate(items.turretPrefab, transform.position + (Vector3.down * transform.position.y), transform.rotation, transform);
                    break;
                }
            default:
                {
                    break;
                }
        }
        boughtItems.Add(item.id);
        GameObject UIItem = shopItemUIDict[item.id];
        GameObject.DestroyImmediate(UIItem);
        audioSource.PlayOneShot(selectClip);
        scoreSystem.AddShells(-item.shellCost);
        Debug.Log("Change Me");
        scoreSystem.AddShells(-item.jellyCost);
        
    }

}