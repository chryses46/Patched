using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

	public static List<Checkpoint> checkpointList;

	public static Checkpoint GetFirstCheckpoint() {
		return checkpointList[0];
	}

	public static void DeactivateAllCheckpoints() {
		foreach (Checkpoint checkpoint in checkpointList) {
			checkpoint.Deactivate();
		}
	}

	[SerializeField] private bool isFirstCheckpoint;
	[SerializeField] private GameObject activeFX;

	private void Awake() {
		if (checkpointList == null) checkpointList = new List<Checkpoint>();
		checkpointList.Add(this);
		if (!isFirstCheckpoint) Deactivate();
	}

	private void OnTriggerEnter(Collider other) {
		Player player = other.GetComponent<Player>();
		if (player!=null) {
			player.SetCheckpoint(this);
			DeactivateAllCheckpoints();
			Activate();
		}
	}

	public void Activate() {
		activeFX.SetActive(true);
	}

	public void Deactivate() {
		activeFX.SetActive(false);
	}

}
