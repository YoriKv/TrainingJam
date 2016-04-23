using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocationStateUI:MonoBehaviour {
    public Image locationIndicator;
    public Text stateText;
    private const float STATE_DISPLAY_TIME = LocationManager.UPDATE_STATE_TIME;
    private float _stateTimer = 0f;

    public void Awake() {
        LocationManager.Enter += LocationManagerOnEnter;
        LocationManager.EnterDone += LocationManagerOnEnterDone;
        LocationManager.Exit += LocationManagerOnExit;
        LocationManager.ExitDone += LocationManagerOnExitDone;
    }

    private void LocationManagerOnEnter() {
        stateText.text = "Enter";
        _stateTimer = STATE_DISPLAY_TIME;
    }

    private void LocationManagerOnEnterDone(bool success) {
        if(success) {
            stateText.text = "Enter Confirmed";
        } else {
            stateText.text = "Enter Failed";
        }
        _stateTimer = STATE_DISPLAY_TIME;
    }

    private void LocationManagerOnExit() {
        stateText.text = "Exit";
        _stateTimer = STATE_DISPLAY_TIME;
    }

    private void LocationManagerOnExitDone(bool success) {
        if(success) {
            stateText.text = "Exit Confirmed";
        } else {
            stateText.text = "Exit Failed";
        }
        _stateTimer = STATE_DISPLAY_TIME;
    }

    public void ConfirmEnter() {
        LocationManager.Enter_Confirm();
    }

    public void ConfirmExit() {
        LocationManager.Exit_Confirm();
    }

    public void Update() {
        locationIndicator.color = LocationManager.location == LocationManager.Location.Present ? Color.green : Color.red;

        if(_stateTimer < 0f) {
            stateText.text = LocationManager.location.ToString();
        } else {
            _stateTimer -= Time.deltaTime;
        }
    }
}
