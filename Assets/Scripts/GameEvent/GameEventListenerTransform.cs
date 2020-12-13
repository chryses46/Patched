using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vilar.Events {

	[System.Serializable]
	public class EventTransform : UnityEvent<Transform> { }

	public class GameEventListenerTransform : MonoBehaviour {

		public GameEventTransform Event;
		public EventTransform Response;

		private void OnEnable() { Event.RegisterListener(this); }
		private void OnDisable() { Event.UnregisterListener(this); }
		public void OnEventRaised(Transform value) { Response.Invoke(value); }

	}

}

