using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleUI:MonoBehaviour {
    public void PlayGame() {
        GM.gold = 0;
        GM.teamGold = Random.Range(500, 1000);
        GM.limbsRemaining = 4;
        SceneManager.LoadScene("Ocean");
    }
}
