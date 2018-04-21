using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField]float time = 5;
	[SerializeField]float velocity = 10;

	void Start () {
		Invoke ("Die", time);
		GetComponent<Rigidbody> ().velocity = transform.forward * velocity;
	}

	void Die () {
		Destroy (gameObject);
	}

}
