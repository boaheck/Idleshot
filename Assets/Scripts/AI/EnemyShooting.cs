using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

	[SerializeField]int shotAmt = 5;
	[SerializeField]float timeToShoot = 1;
	[SerializeField]float projAreaSize = 0.5f;

	public GameObject shooter;

	[HideInInspector]public bool shooting;
	float interval;
	EnemyAI ai;
	EnemyProjectileSpawn[] shotPoints;

	void Start () {
		ai = transform.parent.gameObject.GetComponent<EnemyAI> ();
		shotPoints = new EnemyProjectileSpawn[shotAmt];
		interval = timeToShoot/shotAmt;
		for (int i = 0; i < shotAmt; i++) {
			Vector3 pos = Random.insideUnitSphere * projAreaSize;
			GameObject newShoot = Instantiate (shooter, transform.position + pos, transform.rotation, transform) as GameObject;
			shotPoints [i] = newShoot.GetComponent<EnemyProjectileSpawn> ();
			shotPoints [i].setTimer (interval * i);
			shotPoints [i].shooter = this;
			shotPoints [i].timeToShoot = timeToShoot;
		}
	}

	void Update () {
		shooting = ai.hasTarget;
	}
}
