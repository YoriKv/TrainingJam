using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Cannonball:MonoBehaviour {
    private tk2dSprite _sp;
    private ParticleSystem _ps;

    private Sequence _upDownSeq;
    private Treasure _target;

    public void Awake() {
        _sp = GetComponent<tk2dSprite>();
        _ps = GetComponentInChildren<ParticleSystem>();
    }

    public void Update() {
        transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * 10f, Vector3.back);
    }

    public void Launch(Treasure target) {
        _target = target;
        Vector3 targetPos = target.transform.position + (Vector3)Random.insideUnitCircle * 0.2f;
        float flyTime = 4f;
        // Fly up and down
        _upDownSeq = DOTween.Sequence();
        _upDownSeq.Append(transform.DOBlendableMoveBy(Vector3.up * 1.5f, flyTime / 2f).SetEase(Ease.OutSine).SetRelative(true));
        _upDownSeq.Append(transform.DOBlendableMoveBy(Vector3.down * 1.5f, flyTime / 2f).SetEase(Ease.InSine).SetRelative(true));
        _upDownSeq.Play();

        // Move over
        Vector3 offset = targetPos - transform.position;
        transform.DOBlendableMoveBy(offset, flyTime).SetEase(Ease.Linear).OnComplete(Finish);
    }

    public void Click() {
        _target = null;
        Finish();
    }

    private bool _finished;

    private void Finish() {
        if(!_finished) {
            // Remove gold from treasure
            if(_target != null && _target.value > 0) {
                _target.value -= Mathf.Min(10, _target.value);
                _target.CheckValue();
            }
            // Die
            this.DOKill();
            _finished = true;
            StartCoroutine(FinishRoutine());
        }
    }

    public IEnumerator FinishRoutine() {
        // Hit
        _sp.color = Color.clear;
        _ps.Play();
        yield return new WaitForSeconds(0.5f);
        // Clear
        Destroy(gameObject);
    }
}
