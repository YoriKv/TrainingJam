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

    public bool paused = false;

    private bool _exitConfirmed;
    public bool exitConfirmed {
        get { return _exitConfirmed; }
        set {
            _exitConfirmed = value;
            if(value) {
                LocationManager.Exit_Confirm();
            }
        }
    }

    private bool _warningPresent;
    public bool warningPresent {
        get { return _warningPresent; }
        set {
            _warningPresent = value;
            if(!value) {
                LocationManager.Enter_Confirm();
                _warningTimer = 0f;
            }
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
    private float _exitTimer = 0f;

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
        interactable = false;
        _warningTimer = 0f;
        if(_warningPresent) {
            // Didn't clear the warning
            StartCoroutine(Leave());
        } else {
            _exitTimer = 10f;
            islandUI.ShowExit();
        }
    }

    public IEnumerator Start() {
        // Tween in
        Tween t = transform.DOMoveX(0f, 5f).SetEase(Ease.OutQuad);
        yield return t.WaitForCompletion();
        interactable = true;
        // Arrive
        ship.Arrive();
        // Leave after timeout
        _warningTimer = 10f;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.R) || Input.touchCount == 5) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(_warningTimer > 0f && warningPresent && !paused) {
            _warningTimer -= Time.deltaTime;
            if(_warningTimer <= 0f) {
                StartCoroutine(Leave());
            }
            islandUI.timerText.text = Mathf.RoundToInt(_warningTimer).ToString();
        }

        if(_exitTimer > 0f) {
            _exitTimer -= Time.deltaTime;
            if(_exitTimer <= 0f) {
                StartCoroutine(Leave());
            }
            islandUI.timerText.text = Mathf.RoundToInt(_exitTimer).ToString();
        }

        if(_warningTimer <= 0f && _exitTimer <= 0f) {
            islandUI.timerText.text = "";
        }
    }

    public void ShowWarning() {
        islandUI.ShowWarning();
    }

    private IEnumerator Leave() {
        const float leaveTime = 5f;

        // Hide UIs
        islandUI.HideWarning();
        islandUI.HideExit();

        // Leave
        ship.Leave(leaveTime);
        interactable = false;

        // Tween out
        Tween t = transform.DOMoveX(10f, leaveTime).SetEase(Ease.InQuad);
        yield return t.WaitForCompletion();

        // Back to location manager
        if(warningPresent) {
            // Failed
            FailedUI.Fail("Didn't explore the island, your crew is mad at the lost gold.");
        } else if(!exitConfirmed) {
            FailedUI.Fail("Didn't remove the island trap tags, the navy finds you with the evidence left behind.");
        } else {
            // Succeeded
            GM.gold += collectedValue;
            GM.teamGold += collectedValue;
            _collectedValue = 0;
            SceneManager.LoadScene("Ocean");
        }
    }

    public void OnDestroy() {
        LocationManager.Exit -= OnExit;
        I = null;
    }
}
