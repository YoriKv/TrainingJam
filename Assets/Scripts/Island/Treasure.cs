using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Treasure:MonoBehaviour {
    public tk2dSprite glow;
    public tk2dSprite chest;
    public tk2dSprite pile;

    public int value;

    private Tweener _chestPunchTween = null;
    private Tweener _pilePunchTween = null;
    private float _interactDelayTimer = -1f;

    public void Start() {
        CheckValue();

        _pilePunchTween = pile.transform.DOPunchScale(new Vector3(0.2f, 0.4f), 0.2f).SetAutoKill(false).Pause();
        _chestPunchTween = chest.transform.DOPunchScale(new Vector3(0.2f, 0.4f), 0.2f).SetAutoKill(false).Pause();
    }

    private void CheckValue() {
        // Check fade out
        if(value == 0) {
            value = -1;
            if(chest.isActiveAndEnabled) {
                glow.gameObject.SetActive(false);
                chest.DOFade(0f, 0.2f).OnComplete(Done);
            } else {
                pile.DOFade(0f, 0.2f).OnComplete(Done);
            }
            return;
        }
        // Set visual based on value
        glow.gameObject.SetActive(false);
        chest.gameObject.SetActive(false);
        pile.gameObject.SetActive(false);
        if(value < 30) {
            glow.gameObject.SetActive(false);
            chest.gameObject.SetActive(false);
            pile.gameObject.SetActive(true);
            // Fade in
            pile.DOFade(1f, 0.5f);
        } else if(value < 100) {
            glow.gameObject.SetActive(false);
            chest.gameObject.SetActive(true);
            pile.gameObject.SetActive(false);
            // Fade in
            chest.DOFade(1f, 0.5f);
        } else if(value >= 100) {
            glow.gameObject.SetActive(true);
            chest.gameObject.SetActive(true);
            pile.gameObject.SetActive(false);
            // Fade in
            glow.DOFade(1f, 0.5f);
            chest.DOFade(1f, 0.5f);
        }
    }

    private void Done() {
        Destroy(gameObject);
        if(Island.I.warningPresent) {
            FailedUI.Fail("Tried to explore the island without proper Lockout/Tagout.");
        }
    }

    public void Update() {
        if(_interactDelayTimer >= 0f) {
            _interactDelayTimer -= Time.deltaTime;
        }
    }

    public void Click() {
        if(Island.interactable && _interactDelayTimer < 0f && value > 0) {
            // Warning?
            if(Island.I.warningPresent) {
                value = 0;
            }
            // Anim
            _pilePunchTween.Restart();
            _chestPunchTween.Restart();
            // Gold
            int v = Mathf.Min(10, value);
            value -= v;
            Island.I.collectedValue += v;
            // Coin
            if(v > 0) {
                CoinPool.I.PlayNext(transform.position);
            }
            // Update graphics
            CheckValue();
            // Delay
            _interactDelayTimer = 0.05f;
        }
    }
}
