using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI:MonoBehaviour {
    public Text startButtonText;
    public GameObject tutorialScreen;

    public void Awake() {
        tutorialScreen.gameObject.SetActive(false);
        startButtonText.text = "Show Tutorial";
    }

    public void PlayGame() {
        if(tutorialScreen.gameObject.activeSelf) {
            GM.gold = 0;
            GM.teamGold = Random.Range(500, 1000);
            GM.limbsRemaining = 4;
            SceneManager.LoadScene("Ocean");
        } else {
            tutorialScreen.gameObject.SetActive(true);
            startButtonText.text = "Start Game";
        }
    }
}
