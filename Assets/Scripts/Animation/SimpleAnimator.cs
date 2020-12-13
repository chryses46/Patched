using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleAnimation {
	public AnimationCurve animationCurve;
	public float speed;
	public float power;
	public SimpleAnimator.Axis axis;
	public float progress;
}

public class SimpleAnimator : MonoBehaviour {

	public enum Axis { X, Y, Z, RX, RY, RZ }

	[SerializeField] private SimpleAnimation[] simpleAnimations;

	private Vector3 cachedPosition;

	private void Awake() {
		cachedPosition = transform.localPosition;
	}

	private void Update() {
		bool position = false;
		bool firstPosition = true;
		foreach (SimpleAnimation simpleAnimation in simpleAnimations) {
			simpleAnimation.progress += simpleAnimation.speed * Time.deltaTime;
			simpleAnimation.progress = simpleAnimation.progress % 1f;
			Vector3 offsetPosition = Vector3.zero;
			Vector3 offsetRotation = Vector3.zero;
			switch (simpleAnimation.axis) {
				case Axis.X:
					position = true;
					offsetPosition = Vector3.right * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
				case Axis.Y:
					position = true;
					offsetPosition = Vector3.up * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
				case Axis.Z:
					position = true;
					offsetPosition = Vector3.forward * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
				case Axis.RX:
					position = false;
					offsetRotation = Vector3.right * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
				case Axis.RY:
					position = false;
					offsetRotation = Vector3.up * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
				case Axis.RZ:
					position = false;
					offsetRotation = Vector3.forward * simpleAnimation.animationCurve.Evaluate(simpleAnimation.progress);
					break;
			}
			if (position) {
				if (firstPosition) {
					transform.localPosition = cachedPosition + offsetPosition * simpleAnimation.power;
					firstPosition = false;
				} else {
					transform.localPosition += offsetPosition * simpleAnimation.power * Time.deltaTime;
				}
			} else {
				transform.localRotation *= Quaternion.Euler(offsetRotation * simpleAnimation.power * Time.deltaTime);
			}
		}
	}

}
