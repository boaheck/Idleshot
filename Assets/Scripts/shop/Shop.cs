using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour {

    Transform canvasContainer;
    float offScale = 0.2f;
    float onScale = 1f;
    float moveStep = 0.03f;
    Coroutine showShop, hideShop;

    public ShopItems items;
    private List<string> boughtItems = new List<string>();
    public Transform shopItemUIContainer;
    public GameObject shopItemUIPrefab;
    private Dictionary<string,GameObject> shopItemUIDict = new Dictionary<string,GameObject>();

    void Start() {
        canvasContainer = GetComponentInChildren<Canvas>().transform.parent;
        int i = 0;
        foreach (ShopItem item in items.shopItems) {
            GameObject shopItemUI = GameObject.Instantiate(shopItemUIPrefab, Vector3.zero, shopItemUIContainer.rotation, shopItemUIContainer);
            shopItemUI.name = item.id;
            Button button = shopItemUI.GetComponent<Button>();
            button.onClick.AddListener(() => {
                BuyUpgrade(item);
            });

            EnableButtonOnScore ebos = shopItemUI.AddComponent<EnableButtonOnScore>();
            ebos.requiredScore = item.cost;

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

    void BuyUpgrade(ShopItem item) {
        ItemType type = item.type;
        List<ShopFunctionValue> functionValues = item.functionValues;
        Debug.Log(type);
        switch(type){
            case ItemType.BuyTurret:{
				GameObject.Instantiate(items.turretPrefab, transform.position + (Vector3.down * transform.position.y), transform.rotation, transform);
                break;
            }
            default: {
                break;
            }
        }
        boughtItems.Add(item.id);
        GameObject UIItem = shopItemUIDict[item.id];
        GameObject.DestroyImmediate(UIItem);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            if (showShop != null) {
                StopCoroutine(showShop);
            }
            if (hideShop != null) {
                StopCoroutine(hideShop);
            }
            showShop = StartCoroutine(ShowShop());
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            if (hideShop != null) {
                StopCoroutine(hideShop);
            }
            if (showShop != null) {
                StopCoroutine(showShop);
            }
            hideShop = StartCoroutine(HideShop());
        }
    }

    IEnumerator ShowShop() {
        while (canvasContainer.localScale.x < onScale) {
            canvasContainer.localScale = canvasContainer.localScale + (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator HideShop() {
        while (canvasContainer.localScale.x > offScale) {
            canvasContainer.localScale = canvasContainer.localScale - (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }
}