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

    void Start() {
        button = GetComponent<Button>();
        shop = GetComponentInParent<Shop>();
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        CheckButtonValid();
    }

    void FixedUpdate() {
        CheckButtonValid();
    }

    public void CheckButtonValid() {
        int currentScore = scoreSystem.GetShells();
        int currentJelly = scoreSystem.GetJelly();
        bool hasDependency = shop.BoughtItem(depends);
        button.interactable = requiredShells <= currentScore && requiredJelly < currentJelly && hasDependency;
    }
}
