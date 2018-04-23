using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyPickup : MonoBehaviour {

	ScoreSystem scores;

	void Start(){
		scores = FindObjectOfType<ScoreSystem> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Jelly") {
			scores.AddJelly ();
			Destroy (other.gameObject);
		}
	}

}
