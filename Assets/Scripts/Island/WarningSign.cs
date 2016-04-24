using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class WarningSign:MonoBehaviour {
    public tk2dSprite signSprite;

    public void Update() {
        if(!Island.I.warningPresent) {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() {
        Tween t = signSprite.DOFade(0f, 0.2f);
        yield return t.WaitForCompletion();
        Destroy(gameObject);
    }

    public void Click() {
        if(Island.interactable) {
            Island.I.ShowWarning();
        }
    }
}
