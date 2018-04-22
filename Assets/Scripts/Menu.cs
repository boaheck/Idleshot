using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void Play(string level){
		SceneManager.LoadScene (level);
	}

	public void Quit(){
		Application.Quit();
	}

}
