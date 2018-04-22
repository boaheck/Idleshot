using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour {

	[SerializeField]Vector3 shotOffset = Vector3.zero;
	[SerializeField]float rate = 0.2f;
	[SerializeField]bool auto = false;

	float timer;
	ScoreSystem scores;

	public GameObject projectile;

	void Start () {
		timer = 0;
		scores = GameObject.Find ("Scores").GetComponent<ScoreSystem> ();
	}

    void Update() {
        bool onUI = EventSystem.current.IsPointerOverGameObject();
        if (!onUI) {

            if (auto) {
                if (Input.GetButtonDown("Fire1")) {
                    Fire();
                    timer = rate;
                } else if (Input.GetButton("Fire1")) {
                    if (timer <= 0) {
                        Fire();
                        timer = rate;
                    } else {
                        timer -= Time.deltaTime;
                    }
                }
            } else {
                if (timer <= 0) {
                    if (Input.GetButtonDown("Fire1")) {
                        Fire();
                        timer = rate;
                    }
                } else {
                    timer -= Time.deltaTime;
                }
            }
        }

    }

	void Fire(){
		Instantiate (projectile, transform.position + (transform.rotation * shotOffset), transform.rotation);
		scores.AddShells ();
	}
}
