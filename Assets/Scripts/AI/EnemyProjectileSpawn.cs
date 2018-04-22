using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileSpawn : MonoBehaviour {

	[HideInInspector] public EnemyShooting shooter;
	[HideInInspector] public float timeToShoot;
	[SerializeField]float targetScale = 0.2f;
	[SerializeField]float minScale = 0.01f;

	public GameObject projectile;

	float timer;

	void Start () {
		
	}

	void Update () {
		if (shooter.shooting) {
			if (timer > 0) {
				timer -= Time.deltaTime;
			} else {
				Instantiate (projectile, transform.position, transform.rotation);
				timer += timeToShoot;
			}
		}
		transform.localScale = Vector3.one * Mathf.Lerp (minScale, targetScale, 1 - (timer / timeToShoot));
	}

	public void setTimer(float newTime){
		timer = newTime;
	}
}
