using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOnScore : MonoBehaviour {

    public int requiredScore;
    Button button;
    ScoreSystem scoreSystem;

    void Start() {
        button = GetComponent<Button>();
        scoreSystem = GameObject.FindObjectOfType<ScoreSystem>();
        CheckButtonValid();
    }

    void FixedUpdate() {
        CheckButtonValid();
    }

    public void CheckButtonValid() {
        int currentScore = scoreSystem.GetShells();
        button.interactable = requiredScore <= currentScore;
    }
}
