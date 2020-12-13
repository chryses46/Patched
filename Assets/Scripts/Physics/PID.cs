using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID {

	private float P;
	private float D;
	private float targetDistance;
	private float error = 0f;
	private float errorDifference = 0f;
	private float lastError = 0f;
	private float timeStep = 0f;

	public PID(float P, float D, float targetDistance) {
		this.P = P;
		this.D = D;
		this.targetDistance = targetDistance;
	}

	public void UpdateTargetDistance(float distance) {
		targetDistance = distance;
	}

	public void UpdatePID(float distance, float timeElapsed, float mod=1f) {
		error = targetDistance * mod - distance;
		errorDifference = error - lastError;
		lastError = error;
		timeStep = timeElapsed;
	}

	public float getCorrection() {
		return error * P * timeStep + errorDifference * D * timeStep;
	}

}