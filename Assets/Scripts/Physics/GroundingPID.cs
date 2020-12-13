using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LandEvent : UnityEvent<float> { }

[RequireComponent(typeof(Rigidbody))]
public class GroundingPID : MonoBehaviour {
	[Tooltip("Error correction intensity")]
	[SerializeField] private float P = 300f;
	[Tooltip("Anticipatory control")]
	[SerializeField] private float D = 1000f;
	[Tooltip("Distance between object origin and ground at rest")]
	[SerializeField] private float groundingHeight;
	public LayerMask collisionLayerMask;
	public LandEvent OnLand;

	[HideInInspector] public float groundingHeightMod = 1f;
	public bool grounded { get; private set; }
	public bool groundedTruth { get; private set; }
	public Vector3 groundNormal { get; private set; }
	private float coyoteTime = 0f;
	private float distanceToGround;
	private PID pid;
	private Rigidbody rb;

	public delegate void landDelegate(float power);
	protected landDelegate landCallback;
	public void RegisterLand(landDelegate landDelegate) { landCallback = landDelegate; }

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		pid = new PID(P, D, groundingHeight);
	}

	public void FixedUpdate() {
		coyoteTime -= Time.deltaTime;
		RaycastHit hit;
		distanceToGround = 10f;
		if (Physics.SphereCast(transform.position + Vector3.up * 0.2f, 0.1f, -transform.up, out hit, 3f, collisionLayerMask)) {
			hit.collider.GetComponent<DropPlatform>()?.Drop();
			distanceToGround = hit.distance;
			groundNormal = hit.normal;
		}
		if ((groundedTruth || rb.velocity.y<0f) && distanceToGround <= groundingHeight + (groundedTruth ? 0.5f : 0f) && hit.normal.y > 0.7f) {
			if (!grounded) OnLand.Invoke(-rb.velocity.y); // landCallback(-rb.velocity.y);
			groundedTruth = true;
		} else {
			groundedTruth = false;
		}
		if (groundedTruth) {
			pid.UpdatePID(distanceToGround, Time.deltaTime, groundingHeightMod);
			rb.velocity += Vector3.up * pid.getCorrection();
			coyoteTime = 0.1f;
		}
		grounded = coyoteTime > 0f;
	}

	public void Unground() {
		if (groundedTruth) rb.MovePosition(transform.position.With(y: transform.position.y-distanceToGround+groundingHeight));
		groundedTruth = false;
	}

}