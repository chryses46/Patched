using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnEnable : MonoBehaviour
{

	[SerializeField] private AudioSource source;
	[SerializeField] private AudioClip clip;

	private void OnEnable() {
		source.PlayOneShot(clip);
	}
}
