using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ocean:MonoBehaviour {
    public void Awake() {
        // Ocean moving
        Sequence seq = DOTween.Sequence().SetLoops(-1);
        seq.Append(transform.DOBlendableMoveBy(Vector3.left * 3f, 3f).SetEase(Ease.OutSine));
        seq.Append(transform.DOBlendableMoveBy(Vector3.right * 3f, 3f).SetEase(Ease.InSine));
        seq.Append(transform.DOBlendableMoveBy(Vector3.right * 3f, 3f).SetEase(Ease.OutSine));
        seq.Append(transform.DOBlendableMoveBy(Vector3.left * 3f, 3f).SetEase(Ease.InSine));
    }
}
