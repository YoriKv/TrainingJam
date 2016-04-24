using UnityEngine;
using System.Collections;

public class MusicPlayer:MonoBehaviour {
	public AudioClip music;

	// Play music
	void Start() {
		GM.SM.PlayMusic(music, true);
	}
}
