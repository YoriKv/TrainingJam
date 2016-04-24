using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OceanUI:MonoBehaviour {
    public Image locationIndicator;
    public Text playerGold;
    public Text teamGold;

    public void Awake() {
        LocationManager.Enter += OnEnter;

        playerGold.text = playerGold.text + GM.gold;
        teamGold.text = teamGold.text + GM.teamGold;
    }

    private void OnEnter() {
        Island.totalValue = 100 + Mathf.RoundToInt(200f * (GM.limbsRemaining / 4f));
        SceneManager.LoadScene("Island");
    }

    public void OnDestroy() {
        LocationManager.Enter -= OnEnter;
    }

    public void Update() {
        locationIndicator.color = LocationManager.location == LocationManager.Location.Present ? Color.green : Color.red;
    }
}
