using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    bool mousemode = false;
	Vector3 lookPos = Vector3.zero;
	public LayerMask aimableLayers;

    void Update() {
        if (mousemode) {
            if (Mathf.Abs(Input.GetAxis("AimX")) > 0.1 || Mathf.Abs(Input.GetAxis("AimY")) > 0.1) {
                mousemode = false;
            }
        } else {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1) {
                mousemode = true;
            }
        }
    }

    void FixedUpdate() {
        if (mousemode) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit,1000f ,aimableLayers.value)) {
				lookPos = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
			}
			transform.LookAt(lookPos);
        } else {
            Vector3 lookdir = new Vector3(Input.GetAxis("AimX"), 0, Input.GetAxis("AimY"));
            if (lookdir.magnitude > 0.2) {
                transform.forward = lookdir.normalized;
            }
        }
    }
}
