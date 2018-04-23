using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOnScore : MonoBehaviour {

    public int requiredShells;
    public int requiredJelly;
    public string depends;
    Button button;
    ScoreSystem scoreSystem;
    Shop shop;
    RectTransform rectTransform;
    Rect initialRect;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        initialRect = rectTransform.rect;
        button = transform.GetChild(0).GetComponent<Button>();
        shop = GetComponentInParent<Shop>();
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        CheckValid();
    }

    void FixedUpdate() {
        CheckValid();
    }

    public void CheckValid() {
        int currentScore = scoreSystem.GetShells();
        int currentJelly = scoreSystem.GetJelly();
        bool hasDependency = shop.BoughtItem(depends);
        button.interactable = requiredShells <= currentScore && requiredJelly <= currentJelly && hasDependency;
        GameObject child= transform.GetChild(0).gameObject;
        if(!hasDependency && child.active){
            child.SetActive(false);
            rectTransform.sizeDelta = new Vector2(0,-30);
        }
        if(hasDependency && !child.active){
            child.SetActive(true);
            rectTransform.sizeDelta = new Vector2(initialRect.width,initialRect.height);
        }
    }
}
