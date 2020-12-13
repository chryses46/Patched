using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vilar.Events;

public class GameEventTrigger : MonoBehaviour
{

	public GameEvent gameEvent;

	private void OnTriggerEnter(Collider other) {
		gameEvent.Raise();
	}

}
