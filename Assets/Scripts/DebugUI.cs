using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugUI:MonoBehaviour {
    public void StartGame() {
        GM.gold = 0;
        GM.teamGold = Random.Range(500, 1000);
        GM.limbsRemaining = 4;
        SceneManager.LoadScene("Title");
    }
}
