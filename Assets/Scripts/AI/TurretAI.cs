using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {

    Transform target;
	private float sightDistance = 17f;

	void Start () {
		Debug.Log("I am a turret!");
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
		if(distance > sightDistance){
			//Lost Sight
			target=  null;
			return;
		}
		transform.LookAt(target,Vector3.up);
		transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));

	}
}
