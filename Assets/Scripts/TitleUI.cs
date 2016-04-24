using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleUI:MonoBehaviour {
    private float _startTimer;

    public void PlayGame() {
        GM.gold = 0;
        GM.teamGold = Random.Range(500, 1000);
        GM.limbsRemaining = 4;
        SceneManager.LoadScene("Ocean");
    }

    public void Update() {
        if(Input.GetMouseButton(0) || Input.touchCount > 0) {
            _startTimer += Time.deltaTime;
            if(_startTimer > 1f) {
                PlayGame();
            }
        } else {
            _startTimer = 0f;
        }
    }
}
