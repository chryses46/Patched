using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{

	private float timer = 100f;
	private bool dropped;
	private Vector3 origin;

	private void Awake() {
		origin = transform.position;
	}

	private void Update() {
		//Debug.Log(timer);
		timer += Time.deltaTime;
		dropped = (timer > 0f && timer < 3f);
		transform.position = Vector3.MoveTowards(transform.position, origin + Vector3.forward * (dropped ? 3f : 0f), Time.deltaTime * 20f);
		if (timer < 0f) transform.position += Random.insideUnitSphere * 0.05f;
	}

	public void Drop() {
		if (timer>3f) timer = -1f;
	}

}
