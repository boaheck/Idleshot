using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

	int shells = 0;
	int jelly = 0;
	int parts = 0;
	public Text shellDisplay;
	public Text jellyDisplay;

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
		jellyDisplay.text = "Jelly: " + jelly;
	}

	public int GetJelly(){
		return jelly;
	}

	public void AddJelly(){
		AddJelly (1);
	}

	public void AddJelly(int amt){
		jelly += amt;
		if (jelly < 0) {
			jelly = 0;
		}
		updateDisplay ();
	}

	public void ClearJelly(){
		jelly = 0;
		updateDisplay ();
	}

	public void addParts(){
		parts++;
	}

	public int getParts(){
		return parts;
	}

}
