using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	int shells = 0;
	public Text shellDisplay;

	public int GetShells(){
		return shells;
	}

	public void AddShells(){
		AddShells (1);
	}

	public void AddShells(int amt){
		shells += amt;
		if (shells < 0) {
			shells = 0;
		}
		updateDisplay ();
	}

	public void ClearShells(){
		shells = 0;
		updateDisplay ();
	}

	void updateDisplay(){
		shellDisplay.text = "Shells: " + shells;
	}

}
