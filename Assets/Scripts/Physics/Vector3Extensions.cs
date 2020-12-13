using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions {

	public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) {
		return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
	}

	public static Vector3 GroundVector(this Vector3 original) {
		return original.With(y:0f);
	}

	public static Vector3 KillDepth(this Vector3 original) {
		return original.With(z:0f);
	}

	public static Vector3 MagnitudeClamped(this Vector3 original, float? min = null, float? max = null) {
		float magnitude = original.magnitude;
		magnitude = Mathf.Max(magnitude, min ?? magnitude);
		magnitude = Mathf.Min(magnitude, max ?? magnitude);
		return original.normalized * magnitude;
	}

	public static Vector3 DirectionTo(this Vector3 source, Vector3 destination) {
		return Vector3.Normalize(destination - source);
	}

	public static float DistanceTo(this Vector3 source, Vector3 destination) {
		return Vector3.Magnitude(destination - source);
	}

	public static Vector3 VectorTo(this Vector3 source, Vector3 destination) {
		return destination - source;
	}

}
