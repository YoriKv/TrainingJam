using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Coin:MonoBehaviour {
    private tk2dSprite _sp;

    private Sequence _spinSeq;
    private Sequence _upDownSeq;
    private Tween _fadeTween;

    public void Awake() {
        _sp = GetComponent<tk2dSprite>();
        // Spin
        _spinSeq = DOTween.Sequence().SetLoops(-1);
        _spinSeq.Append(transform.DOScale(new Vector3(0f, 1f), 0.15f));
        _spinSeq.Append(transform.DOScale(Vector3.one, 0.15f));
        _spinSeq.Pause();

        // Fly up and down
        _upDownSeq = DOTween.Sequence().SetAutoKill(false);
        _upDownSeq.Append(transform.DOBlendableMoveBy(Vector3.up, 0.2f).SetEase(Ease.OutSine).SetRelative(true));
        _upDownSeq.Append(transform.DOBlendableMoveBy(Vector3.down, 0.2f).SetEase(Ease.InSine).SetRelative(true));
        _upDownSeq.Pause();

        // Fade
        _fadeTween = _sp.DOFade(0f, 0.2f).SetAutoKill(false).OnComplete(Finish);
        _fadeTween.Complete();
    }

    public void Play(Vector3 pos) {
        transform.position = pos;
        // Spin
        _spinSeq.Play();

        // Fly up down
        _upDownSeq.Restart();

        // Move over
        transform.DOBlendableMoveBy(Random.insideUnitCircle.normalized * Random.Range(0.5f, 1f), 0.4f).SetEase(Ease.Linear);

        // Sound
        GM.SM.PlayEffectOnce(GM.SM.coinSnds);

        _sp.color = Color.white;
        StartCoroutine(FadeAway());
    }

    public void Finish() {
        _spinSeq.Pause();
        _sp.color = Color.clear;
    }

    private IEnumerator FadeAway() {
        yield return new WaitForSeconds(1f);
        _fadeTween.Restart();
    }
}
