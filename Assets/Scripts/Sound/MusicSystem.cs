using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour {

	[SerializeField] float targetVolume = 1f;
	[SerializeField] float ambientVolume = 0.2f;
	[SerializeField] float fadeTime = 30f;

	public AudioSource[] tracks;
	public AudioSource ambience;
	public int currentTrack;

	float fadeAmt;

	void Start () {
		if (FindObjectsOfType<MusicSystem> ().Length > 1) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);
			for (int i = 0; i < tracks.Length; i++) {
				tracks [i].volume = 0;
			}
			ambience.volume = 0;
		}
		fadeAmt = 1.0f / fadeTime;
	}

	void Update () {
		for (int i = 0; i < tracks.Length; i++) {
			if (i == currentTrack) {
				tracks [i].volume = Mathf.MoveTowards (tracks [i].volume, targetVolume, fadeAmt * Time.deltaTime);
			} else if (tracks [i].volume > 0) {
				tracks [i].volume = Mathf.MoveTowards (tracks [i].volume, 0, fadeAmt * Time.deltaTime);
			}
		}
		if (ambience.volume < ambientVolume) {
			ambience.volume = Mathf.MoveTowards (ambience.volume, ambientVolume, fadeAmt * Time.deltaTime);
		}
	}
}
