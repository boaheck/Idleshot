using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour {

	ScoreSystem scores;
	public GameObject uiElement;

	void Start(){
		scores = FindObjectOfType<ScoreSystem> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			scores.addParts ();
			uiElement.SetActive (true);
			Destroy (gameObject);
		}
	}
}
