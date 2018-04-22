using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBottomToLowestChild : MonoBehaviour {
    RectTransform rt;
    void Start() {
        Set();
    }

    public void Set() {
        rt = GetComponent<RectTransform>();
        RectTransform containerRt = transform.parent.GetComponentInParent<RectTransform>();
        float bottom = 0f;
        float childHeight = 0f;
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform crt in children) {
            if (crt.offsetMax.y < bottom) {
                bottom = crt.offsetMax.y;
                childHeight = crt.rect.height;
            }
        }
        bottom += containerRt.rect.height - childHeight;

        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

}
