using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vilar.Events {

	[System.Serializable]
	public class EventInt : UnityEvent<int> { }

	public class GameEventListenerInt : MonoBehaviour {

		public GameEventInt Event;
		public EventInt Response;

		private void OnEnable() { Event.RegisterListener(this); }
		private void OnDisable() { Event.UnregisterListener(this); }
		public void OnEventRaised(int value) { Response.Invoke(value); }

	}

}

