using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {

	[SerializeField]Vector3 shotOffset = Vector3.zero;
	[SerializeField]float rate = 5;
	[SerializeField]float spread = 5.0f;

	public GameObject projectile;

    Transform target;
	private float sightDistance = 17f;
	float overflow = 0.0f;
	float interval;
	float timer;
	ScoreSystem scores;

	void Start () {
		Debug.Log("I am a turret!");
		timer = 0;
		scores = GameObject.Find ("Scores").GetComponent<ScoreSystem> ();
		interval = 1.0f / rate;
	}
	
	void FixedUpdate () {
		if(target == null){
			FindTarget();
		}
		else{
			ShootTarget();
		}
	}

	void FindTarget() {
		transform.Rotate(new Vector3(0,1f,0));
		Ray ray = new Ray(transform.position,transform.forward);
		Debug.DrawRay(ray.origin,ray.direction*sightDistance,Color.green);
		RaycastHit raycastHit;
		if(Physics.Raycast(ray.origin,ray.direction,out raycastHit,sightDistance)){
			if(raycastHit.collider.gameObject.GetComponentInParent<EnemyAI>() != null){
                Debug.Log("Got thing!");
				target=  raycastHit.collider.transform.parent;
			}
		}
	}

	void ShootTarget() {
		float distance = Vector3.Distance(transform.position,target.position);
		if (distance > sightDistance) {
			//Lost Sight
			target = null;
			return;
		} else {
			if (timer <= 0) {
				if (interval < Time.fixedDeltaTime) {
					float bulletTot = Time.fixedDeltaTime/interval;
					bulletTot += overflow;
					int bulletAmt = Mathf.FloorToInt (bulletTot);
					overflow = bulletTot - bulletAmt;
					for (int i = 0; i < bulletAmt; i++) {
						Fire ();
					}
				} else {
					Fire ();
				}
				timer = interval;
			} else {
				timer -= Time.fixedDeltaTime;
			}
		}
		transform.LookAt(target,Vector3.up);
		transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));

	}

	void Fire(){
		Quaternion randRot = Quaternion.Euler(Vector3.up * Random.Range(-spread,spread));
		Instantiate (projectile, transform.position + (transform.rotation * shotOffset), transform.rotation * randRot);
		scores.AddShells ();
	}

}
