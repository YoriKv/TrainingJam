using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InternetInfoTest:MonoBehaviour {
    public Graph graph;
    public Text statusText;
    public Text infoText;

    private bool _justChanged = false;
    private int _signalValue = 0;
    private float _updateTimer = 0f;

    private InternetPlugin _internetPlugin;

    public void Awake() {
        _internetPlugin = InternetPlugin.GetInstance();
        _internetPlugin.setInternetCallbackListener(OnWifiConnect, OnWifiDisconnect, OnWifiSignalStrengthChange);
        _internetPlugin.RegisterEvent();
    }

    public void Update() {
#if !UNITY_EDITOR
        _updateTimer -= Time.deltaTime;
#endif
        if(_updateTimer <= 0f) {
            graph.SetValue(_signalValue / 100f, _justChanged);
            _updateTimer += 1f;
            _justChanged = false;
        }
    }

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
        statusText.text = "Connected";
    }

    void OnWifiDisconnect() {
        statusText.text = "Disconnected";
        infoText.text = "";
    }

    void OnWifiSignalStrengthChange(int signalStrength, int signalDifference) {
        _signalValue = signalDifference;
        _justChanged = true;
        infoText.text = string.Format("wifi Signal Strength: {0}, {1}", signalStrength, signalDifference);
    }
}
