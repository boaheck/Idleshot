using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	bool hasPlayer = false;

	void OnTriggerEnter(Collider col){
		if(col.CompareTag("Player")){
			hasPlayer = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.CompareTag("Player")){
			hasPlayer = false;
		}
	}

	public bool HasPlayer() {
		return hasPlayer;
	}
}
