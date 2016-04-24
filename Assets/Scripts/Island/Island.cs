using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Island:MonoBehaviour {
    public static int totalValue;
    public static bool interactable = false;

    public static Island I = null;

    private bool _warningPresent;
    public bool warningPresent {
        get { return _warningPresent; }
        set {
            _warningPresent = value;
            if(!value)
                _warningTimer = 0f;
        }
    }

    private int _collectedValue;
    public int collectedValue {
        get { return _collectedValue; }
        set {
            _collectedValue = value;
            islandUI.goldText.text = "Found Gold: " + _collectedValue;
        }
    }

    public Ship ship;
    public IslandUI islandUI;

    public Treasure treasurePrefab;
    public WarningSign warningSignPrefab;

    public Transform treasureSpawns;
    public Transform warningSignSpawns;

    private List<Treasure> _treasures;
    private WarningSign _warningSign;

    private float _warningTimer = 0f;

    public void Awake() {
#if UNITY_EDITOR
        totalValue = 300;
#endif
        collectedValue = 0;
        interactable = false;
        warningPresent = true;
        I = this;

        LocationManager.Exit += OnExit;

        List<Spawn> spawns;
        // Split up treasure
        spawns = new List<Spawn>(treasureSpawns.GetComponentsInChildren<Spawn>());
        spawns.Shuffle();
        _treasures = new List<Treasure>(spawns.Count);
        foreach(Spawn spawn in spawns) {
            // Pick value based on leftover value
            int value;
            if(totalValue > 150)
                value = Random.Range(150, 250);
            else if(totalValue > 50)
                value = Mathf.Min(Random.Range(50, 100), totalValue - Random.Range(1, 50));
            else
                value = totalValue;
            // Spawn
            Treasure treasure = Instantiate(treasurePrefab);
            treasure.value = value;
            treasure.transform.SetParent(transform, false);
            treasure.transform.position = spawn.transform.position;
            // Add to list
            _treasures.Add(treasure);
            // Subtract value
            totalValue -= value;
        }

        // Warning sign
        spawns = new List<Spawn>(warningSignSpawns.GetComponentsInChildren<Spawn>());
        spawns.Shuffle();
        _warningSign = Instantiate(warningSignPrefab);
        _warningSign.name = warningSignPrefab.name;
        _warningSign.transform.SetParent(transform, false);
        _warningSign.transform.position = spawns[0].transform.position;

        // Start off screen
        transform.localPosition = Vector3.right * 10f;
    }

    private void OnExit() {
        StartCoroutine(Leave());
    }

    public IEnumerator Start() {
        // Tween in
        Tween t = transform.DOMoveX(0f, 5f).SetEase(Ease.OutQuad);
        yield return t.WaitForCompletion();
        interactable = true;
        // Arrive
        ship.Arrive();
        // Leave after timeout
        _warningTimer = 30f;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.R) || Input.touchCount == 5) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(_warningTimer > 0f) {
            _warningTimer -= Time.deltaTime;
            if(_warningTimer <= 0f) {
                StartCoroutine(Leave());
            }
            islandUI.timerText.text = Mathf.RoundToInt(_warningTimer).ToString();
        } else {
            islandUI.timerText.text = "";
        }
    }

    public void ShowWarning() {
        _warningTimer = 0f;
        islandUI.ShowWarning();
    }

    private IEnumerator Leave() {
        const float leaveTime = 5f;

        // Leave
        ship.Leave(leaveTime);
        interactable = true;

        // Tween out
        Tween t = transform.DOMoveX(10f, leaveTime).SetEase(Ease.InQuad);
        yield return t.WaitForCompletion();

        // Back to location manager
        if(warningPresent) {
            // Failed
            SceneManager.LoadScene("Failed");
        } else {
            // Succeeded
            GM.gold += collectedValue;
            GM.teamGold += collectedValue;
            _collectedValue = 0;
            SceneManager.LoadScene("Ocean");
        }
    }

    public void OnDestroy() {
        I = null;
    }
}
