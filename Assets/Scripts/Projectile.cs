using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField]float time = 5;
	[SerializeField]float velocity = 10;
	public float damage = 1;

	void Start () {
		Invoke ("Die", time);
		GetComponent<Rigidbody> ().velocity = transform.forward * velocity;
	}

	void Die () {
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Level") {
			Destroy (gameObject);
		}
	}


}
