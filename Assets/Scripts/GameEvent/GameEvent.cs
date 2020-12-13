using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vilar.Events {

	[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Data/GameEvent", order = 1)]
	public class GameEvent : ScriptableObject {

		public UnityEvent events;
		private List<GameEventListener> listeners = new List<GameEventListener>();

		public void RegisterListener(GameEventListener listener) { listeners.Add(listener); }
		public void UnregisterListener(GameEventListener listener) { listeners.Remove(listener); }

		public void Raise() {
			events.Invoke();
			for (int i = listeners.Count - 1; i >= 0; i--) {
				listeners[i].OnEventRaised();
			}
		}

	}

	[CreateAssetMenu(fileName = "NewGameEventInt", menuName = "Data/GameEventInt", order = 1)]
	public class GameEventInt : ScriptableObject {

		private List<GameEventListenerInt> listeners = new List<GameEventListenerInt>();

		public void RegisterListener(GameEventListenerInt listener) { listeners.Add(listener); }
		public void UnregisterListener(GameEventListenerInt listener) { listeners.Remove(listener); }

		public void Raise(int value) {
			for (int i = listeners.Count - 1; i >= 0; i--) {
				listeners[i].OnEventRaised(value);
			}
		}

	}

	[CreateAssetMenu(fileName = "NewGameEventTransform", menuName = "Data/GameEventTransform", order = 1)]
	public class GameEventTransform : ScriptableObject {

		public UnityEvent events;
		private List<GameEventListenerTransform> listeners = new List<GameEventListenerTransform>();

		public void RegisterListener(GameEventListenerTransform listener) { listeners.Add(listener); }
		public void UnregisterListener(GameEventListenerTransform listener) { listeners.Remove(listener); }

		public void Raise(Transform transform) {
			events.Invoke();
			for (int i = listeners.Count - 1; i >= 0; i--) {
				listeners[i].OnEventRaised(transform);
			}
		}

	}

}