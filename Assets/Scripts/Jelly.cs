using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour {

	[SerializeField] float time = 10;
	[SerializeField] float startForce = 30;

	void Start () {
		Invoke ("Die", time);
		Vector3 force = Random.insideUnitSphere;
		force = new Vector3 (force.x, Mathf.Abs (force.y), force.z);
		GetComponent<Rigidbody> ().AddForce (force * startForce);
		GetComponentInChildren<Animator> ().playbackTime = Random.value;
	}

	void Die(){
		Destroy (gameObject);
	}
}
