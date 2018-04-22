using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    bool mousemode = false;

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
            float distance = Vector3.Magnitude(transform.position - Camera.main.transform.position);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));

            transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        } else {
            Vector3 lookdir = new Vector3(Input.GetAxis("AimX"), 0, Input.GetAxis("AimY"));
            if (lookdir.magnitude > 0.2) {
                transform.forward = lookdir.normalized;
            }
        }
    }
}
