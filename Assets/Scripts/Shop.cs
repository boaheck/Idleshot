using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{

    Transform canvasContainer;
    float offScale = 0.2f;
    float onScale = 1f;
    float moveStep = 0.03f;
    Coroutine showShop, hideShop;
    void Start()
    {
        canvasContainer = GetComponentInChildren<Canvas>().transform.parent;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (showShop != null)
            {
                StopCoroutine(showShop);
            }
            if (hideShop != null)
            {
                StopCoroutine(hideShop);
            }
            showShop = StartCoroutine(ShowShop());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (hideShop != null)
            {
                StopCoroutine(hideShop);
            }
            if (showShop != null)
            {
                StopCoroutine(showShop);
            }
            hideShop = StartCoroutine(HideShop());
        }
    }

    IEnumerator ShowShop()
    {
        while (canvasContainer.localScale.x < onScale)
        {
            canvasContainer.localScale = canvasContainer.localScale + (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator HideShop()
    {
        while (canvasContainer.localScale.x > offScale)
        {
            canvasContainer.localScale = canvasContainer.localScale - (Vector3.one * moveStep);
            yield return new WaitForFixedUpdate();
        }
    }
}