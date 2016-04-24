using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IslandUI:MonoBehaviour {
    public Image locationIndicator;
    public Text goldText;
    public Text timerText;
    public Text authorizedPirateText;
    public RectTransform warningPanel;
    public Image warningImage;
    public Image tagImage;

    public RectTransform exitPanel;

    public Sprite needSprite;
    public Sprite dontNeedSprite;

    public bool needsPirate;
    public bool calledPirate;

    private float _authorizedPirateTimer = 0f;

    public void Awake() {
        needsPirate = Random.value < 0.5f; // Random chance to need pirate
        warningImage.sprite = needsPirate ? needSprite : dontNeedSprite;
        warningPanel.gameObject.SetActive(false);
        exitPanel.gameObject.SetActive(false);
        tagImage.gameObject.SetActive(false);
    }

    public void Update() {
        if(_authorizedPirateTimer > 0f) {
            _authorizedPirateTimer -= Time.deltaTime;
            if(_authorizedPirateTimer <= 0f) {
                calledPirate = true;
                authorizedPirateText.text = "Traps Disarmed, Tagged Out!";
                Island.I.warningPresent = false;
                tagImage.gameObject.SetActive(true);
            } else {
                authorizedPirateText.text = Mathf.RoundToInt(_authorizedPirateTimer).ToString();
            }
        }

        locationIndicator.color = LocationManager.location == LocationManager.Location.Present ? Color.green : Color.red;
    }

    public void ShowWarning() {
        Island.interactable = false;
        authorizedPirateText.text = "Call Authorized Pirate";
        warningPanel.gameObject.SetActive(true);
    }

    public void CallPirate() {
        if(!needsPirate) {
            FailedUI.Fail("Uneccessarily called the authorized pirate.");
        } else if(!calledPirate && _authorizedPirateTimer <= 0f) {
            Island.I.paused = true;
            // Start timer
            _authorizedPirateTimer = 5f;
        }
    }

    public void HideWarning() {
        if(needsPirate == calledPirate) {
            // Didn't need pirate and exited correctly
            Island.I.warningPresent = false;
        }
        _authorizedPirateTimer = 0f;
        warningPanel.gameObject.SetActive(false);
        Island.I.paused = false;
        Island.interactable = true;
    }

    public void ShowExit() {
        Island.interactable = false;
        exitPanel.gameObject.SetActive(true);
    }

    public void Tagout() {
        Island.I.exitConfirmed = true;
        HideExit();
    }

    public void HideExit() {
        exitPanel.gameObject.SetActive(false);
        Island.interactable = true;
    }
}
