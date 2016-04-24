using UnityEngine;
using System.Collections;

public class ClickDetect:MonoBehaviour {
    public LayerMask clickDetectMask;

    private Camera _cam;

    public void Awake() {
        _cam = Camera.main;
    }

    public void Update() {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0)) {
            ClickScreenPos(Input.mousePosition);
        }
#endif
        foreach(Touch t in Input.touches) {
            if(t.phase == TouchPhase.Began) {
                ClickScreenPos(t.position);
            }
        }
    }

    private void ClickScreenPos(Vector3 screenPos) {
        Ray ray = _cam.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if(hit.collider != null) {
            hit.collider.SendMessage("Click");
        }
    }
}
