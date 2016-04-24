using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailedUI:MonoBehaviour {
    private static string _reason = "";

    public Text reasonText;
    public Image bubbleImage;
    public Image pirateImage;
    public Text limbsText;
    public ParticleSystem blood;
    public Sprite[] pirateSprites;

    public static void Fail(string reason) {
        _reason = reason;
        SceneManager.LoadScene("Failed");
    }

    void Start() {
        reasonText.text = "Reason: " + _reason;
        _reason = "";
        UpdateUI();
        StartCoroutine(DisplayTimer());
    }

    private void UpdateUI() {
        pirateImage.sprite = pirateSprites[GM.limbsRemaining];
        limbsText.text = GM.limbsRemaining.ToString();
    }

    private IEnumerator DisplayTimer() {
        yield return new WaitForSeconds(2f);
        if(GM.limbsRemaining > 0) {
            GM.limbsRemaining--;
            UpdateUI();
        }
        // Flash red
        bubbleImage.color = Color.red;
        bubbleImage.DOColor(Color.white, 2f);
        blood.Play();

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Ocean");
    }
}
