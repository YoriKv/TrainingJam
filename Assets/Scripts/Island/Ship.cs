using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Ship:MonoBehaviour {
    private tk2dSprite _sp;
    private Sequence _bobSeq;

    public void Awake() {
        _sp = GetComponent<tk2dSprite>();
        // Bob
        _bobSeq = DOTween.Sequence().SetLoops(-1);
        _bobSeq.Append(transform.DOBlendableMoveBy(Vector3.up * 0.1f, 1f).SetEase(Ease.OutSine));
        _bobSeq.Append(transform.DOBlendableMoveBy(Vector3.down * 0.1f, 1f).SetEase(Ease.InSine));
        _bobSeq.Append(transform.DOBlendableMoveBy(Vector3.down * 0.1f, 1f).SetEase(Ease.OutSine));
        _bobSeq.Append(transform.DOBlendableMoveBy(Vector3.up * 0.1f, 1f).SetEase(Ease.InSine));
        // Move to side of screen
        Vector3 offset = transform.position;
        transform.position = Vector3.zero;
        if(SceneManager.GetActiveScene().name == "Island") {
            transform.DOBlendableMoveBy(offset, 3f).SetEase(Ease.OutQuad);
        }
    }

    public void Arrive() {
        _bobSeq.Pause();
    }

    public void Leave(float time) {
        _sp.FlipX = true;
        _bobSeq.Play();
        Vector3 offset = -transform.position;
        transform.DOBlendableMoveBy(offset, 3f).SetEase(Ease.InQuad);
    }
}
