using System;
using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine.UI;

public class LocationManager:MonoBehaviour {
    public static Location location = Location.Away;
    public enum Location {
        Away,
        Present
    }

    public static event Action Enter;
    public static event Action<bool> EnterDone;
    public static event Action Exit;
    public static event Action<bool> ExitDone;

    public enum States {
        Away,
        Enter,
        Present,
        Exit
    }

    // Debug
    public Text debugText;

    // Instance
    private static LocationManager _instance;

    // State machine
    private StateMachine<States> _fsm;

    // Location tracker
    public const float UPDATE_STATE_TIME = 30f;
    private const float UPDATE_LOCATION_TIME = 5f;
    private InternetPlugin _internetPlugin;
    private int _signalValue = 0;
    private float _updateTimer = UPDATE_LOCATION_TIME;
    private float _stateTimer = UPDATE_STATE_TIME;

    public void Awake() {
        // Singleton
        _instance = this;
        // Wifi tracker
        _internetPlugin = InternetPlugin.GetInstance();
        _internetPlugin.setInternetCallbackListener(OnWifiConnect, OnWifiDisconnect, OnWifiSignalStrengthChange);
        _internetPlugin.RegisterEvent();
        // FSM
        _fsm = StateMachine<States>.Initialize(this);
        _fsm.ChangeState(States.Away);
    }

    public void Update() {
        if(SignalToState(_signalValue) != location) {
            _updateTimer -= Time.deltaTime;
            if(_updateTimer < 0f) {
                location = SignalToState(_signalValue);
                _updateTimer = UPDATE_LOCATION_TIME;
            }
        } else {
            _updateTimer = UPDATE_LOCATION_TIME;
        }
    }

    // ******************** AWAY ********************

    public void Away_Update() {
        // Wait for present
        if(location == Location.Present) {
            // Arrived
            _fsm.ChangeState(States.Enter);
        }
    }

    // ******************** ENTER ********************

    public void Enter_Enter() {
        _stateTimer = UPDATE_STATE_TIME;
        if(Enter != null)
            Enter();
    }

    public void Enter_Update() {
        _stateTimer -= Time.deltaTime;
        if(_stateTimer < 0f || location == Location.Away) {
            if(EnterDone != null)
                EnterDone(false);
            _fsm.ChangeState(States.Present);
        }
    }

    public static void Enter_Confirm() {
        if(_instance._fsm.State == States.Enter) {
            if(EnterDone != null)
                EnterDone(true);
            _instance._fsm.ChangeState(States.Present);
        }
    }

    // ******************** PRESENT ********************

    public void Present_Enter() {
        _stateTimer = UPDATE_STATE_TIME;
    }

    public void Present_Update() {
        if(location == Location.Away) {
            _instance._fsm.ChangeState(States.Exit);
        }
    }

    // ******************** EXIT ********************

    public void Exit_Enter() {
        _stateTimer = UPDATE_STATE_TIME;
        if(Exit != null)
            Exit();
    }

    public void Exit_Update() {
        _stateTimer -= Time.deltaTime;
        if(_stateTimer < 0f || location == Location.Present) {
            if(ExitDone != null)
                ExitDone(false);
            _fsm.ChangeState(States.Away);
        }
    }

    public static void Exit_Confirm() {
        if(_instance._fsm.State == States.Exit) {
            if(ExitDone != null)
                ExitDone(true);
            _instance._fsm.ChangeState(States.Away);
        }
    }

    // ******************** Signal Tracking ********************

    private void OnApplicationPause(bool val) {
        if(_internetPlugin != null) {
            if(val) {
                _internetPlugin.UnRegisterEvent();
            } else {
                _internetPlugin.RegisterEvent();
            }
        }
    }

    void OnWifiConnect() {
        debugText.text = string.Format("wifi Connected");
    }

    void OnWifiDisconnect() {
        if(debugText != null) {
            debugText.text = string.Format("wifi Disconnected");
        }
        _signalValue = 0;
    }

    void OnWifiSignalStrengthChange(int signalStrength, int signalDifference) {
        _signalValue = signalStrength;
        if(debugText != null) {
            debugText.text = string.Format("wifi Signal Strength: {0}, {1}", signalStrength, signalDifference);
        }
    }

    private static Location SignalToState(int signal) {
        return signal < 4 ? Location.Away : Location.Present;
    }

    public void OnDestroy() {
        _instance = null;
    }
}
