using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterTime : MonoBehaviour {

	public float time = 3f;
	void Start () {
		StartCoroutine(Kill());
	}
	
	IEnumerator Kill(){
		yield return new WaitForSeconds(time);
		GameObject.Destroy(gameObject);
	}
}
