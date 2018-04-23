using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackOnLoad : MonoBehaviour {

	public int track;

	void Start () {
		MusicSystem ms = FindObjectOfType<MusicSystem> ();
		if (ms != null) {
			ms.currentTrack = track;
		}
	}

}
