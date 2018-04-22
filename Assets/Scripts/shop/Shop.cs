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
    private string[] boughtItems;
    public Transform shopItemUIContainer;
    public GameObject shopItemUIPrefab;
    void Start() {
        canvasContainer = GetComponentInChildren<Canvas>().transform.parent;
        int i = 0;
        foreach (ShopItem item in items.shopItems) {
            GameObject shopItemUI = GameObject.Instantiate(shopItemUIPrefab, Vector3.zero, shopItemUIContainer.rotation, shopItemUIContainer);

            Button button = shopItemUI.GetComponent<Button>();
            button.onClick.AddListener(() => {
                BuyUpgrade(item.type, item.functionValues);
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

            i++;
        }
        GetComponentInChildren<SetBottomToLowestChild>().Set();
    }

    void BuyUpgrade(ItemType type, List<ShopFunctionValue> functionValues) {
        Debug.Log(type);
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