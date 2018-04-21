using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    void Start()
    {

    }

    void FixedUpdate()
    {
        float distance = Vector3.Magnitude(transform.position - Camera.main.transform.position);
        Debug.Log(distance);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        Vector3 target = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        Debug.DrawLine(transform.position, target, Color.black, 0.1f);

        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
    }
}
