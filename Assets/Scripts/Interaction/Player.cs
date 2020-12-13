using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public static Player instance;

	public static void Respawn() {
		instance.RespawnAtLastCheckpoint();
	}

	[SerializeField] protected Transform rotator;
	[SerializeField] protected Transform squashTilt;
	[SerializeField] protected Transform wings;
	[SerializeField] protected Transform cam;
	[SerializeField] protected GameObject[] armlist;
	[SerializeField] protected GameObject[] winglist;
	[SerializeField] protected GameObject animIdle;
	[SerializeField] protected GameObject animRun;
	[SerializeField] protected GameObject animJump;
	[SerializeField] protected GameObject animGlide;
	[SerializeField] protected GameObject animClimb;
	[SerializeField] protected float speed;
	[SerializeField] protected float friction;
	[SerializeField] protected float jumpVelocity;
	[SerializeField] protected ParticleSystem dustFX;
	[SerializeField] protected ParticleSystem dustFXLand;
	[SerializeField] protected ParticleSystem wingsFX;
	protected Rigidbody rb;
	protected Vector2 input;
	protected GroundingPID groundingPID;
	protected bool facingRight=true;
	public bool hasArms = false;
	public bool hasDoubleJump = false;
	protected bool doubleJump = true;
	protected bool gliding = false;
	public int life = 3;
	protected Climbable climbing = null;
	protected Checkpoint currentCheckpoint;
	protected Vector3 lastPosition;

	public void Idle() {
		input = new Vector2(0f, input.y);
	}

	public void MoveUp() {
		input = new Vector2(input.x, 1f);
	}

	public void MoveLeft() {
		input = new Vector2(-1f, input.y);
		facingRight = false;
	}

	public void MoveDown() {
		input = new Vector2(input.x, -1f);
	}

	public void MoveRight() {
		input = new Vector2(1f, input.y);
		facingRight = true;
	}

	public void DirectInput(Vector2 newInput) {
		input = newInput;
	}

	public void GiveArms(bool isGive = true){
		hasArms = isGive;
		foreach (GameObject arm in armlist) {
			arm.SetActive(isGive);
		}
	}

	public void GiveDoubleJump(bool isGive = true) {
		hasDoubleJump = isGive;
		foreach (GameObject wing in winglist) {
			wing.SetActive(isGive);
		}
	}

	public void SetInput(Vector2 newInput) {
		input = newInput;
	}

	public void Jump() {
		if (groundingPID.grounded || climbing!=null || (hasDoubleJump && doubleJump)) {
			if (groundingPID.grounded) Land();
			if (!(groundingPID.grounded || climbing != null)) {
				doubleJump = false;
				startGlide();
			}
			if (climbing!=null) {
				rb.velocity = new Vector2(input.x * speed, 0f);
			}
			groundingPID.Unground();
			rb.velocity = rb.velocity.With(y: jumpVelocity);
			climbing = null;
		}
	}

	public void Unjump() {

	}

	public void Land() {
		dustFXLand.Play();
	}

	public void TakeDamage() {
		life--;
		UIController.instance.UpdateLife(life);
	}

	public void SetCheckpoint(Checkpoint newCheckpoint) {
		currentCheckpoint = newCheckpoint;
	}

	public void RespawnAtLastCheckpoint() {
		life = 3;
		rb.velocity = Vector3.zero;
		if (currentCheckpoint!=null) {
			transform.position = currentCheckpoint.transform.position;
		} else {
			transform.position = Checkpoint.GetFirstCheckpoint().transform.position;
		}
	}

	protected virtual void Awake() {
		instance = this;
		rb = GetComponent<Rigidbody>();
		groundingPID = GetComponent<GroundingPID>();
		GiveArms(false);
		GiveDoubleJump(false);
	}

	protected virtual void FixedUpdate() {
		input = new Vector2(input.x, Mathf.MoveTowards(input.y, 0f, Time.deltaTime * 10f));
		if (groundingPID.groundedTruth || climbing != null) {
			ApplyFriction(input == Vector2.zero ? 3f : 1f);
			doubleJump = true;
			if (gliding) StopGlide();
		}
		ApplyAccelleration(groundingPID.groundedTruth ? 1f : 0.3f);
		rb.velocity = rb.velocity.With(y: Mathf.Max(rb.velocity.y, gliding ? -2f : -jumpVelocity * 2f));
		if (climbing != null) {
			rb.velocity = Vector3.up * input.y * speed * 0.6f;
			transform.position = climbing.transform.position.With(y: transform.position.y);
			if (transform.position.y > climbing.transform.position.y) {
				groundingPID.Unground();
				rb.velocity = rb.velocity.With(y: jumpVelocity);
				climbing = null;
			}
			else if (transform.position.y < climbing.bottom.position.y) climbing = null;
		}
		if (transform.position.y < -20f) TakeDamage();
		if (groundingPID.groundedTruth && input.x != 0f) {
			if (dustFX.time >= dustFX.main.duration || !dustFX.isPlaying) dustFX.Play();
		}
		if (groundingPID.groundedTruth) {
			if (Mathf.Abs(input.x) > 0f) {
				SetAnim(animRun);
			} else {
				SetAnim(animIdle);
			}
		} else if (climbing) {
			SetAnim(animClimb);
		} else {
			if (gliding) {
				SetAnim(animGlide);
			} else {
				SetAnim(animJump);
			}
		}
	}

	protected virtual void Update() {
		float rotationtarget = facingRight ? 92f : -92f;
		if (climbing) rotationtarget = 1f;
		rotator.localRotation = Quaternion.Lerp(rotator.localRotation, Quaternion.Euler(0f, rotationtarget, 0f), Time.deltaTime * 10f);
		Quaternion targetRotation;
		float squashStretch = 0f;
		if (groundingPID.groundedTruth) {
			targetRotation = Quaternion.Euler(0f, 0f, rb.velocity.x / speed * -20f);
		} else {
			targetRotation = Quaternion.Euler(0f, 0f, rb.velocity.x / speed * Mathf.Clamp(rb.velocity.y/jumpVelocity, -1f, 1f) * -20f);
			squashStretch = (Mathf.Abs(rb.velocity.y / jumpVelocity) - 0.5f);
			squashStretch = Mathf.Clamp(squashStretch, -1f, 1f) * 0.2f;
		}
		squashTilt.localScale = new Vector3(1f - squashStretch, 1f + squashStretch, 1f);
		squashTilt.localRotation = Quaternion.Lerp(squashTilt.localRotation, targetRotation, Time.deltaTime * 6f);
		cam.position += lastPosition - transform.position;
		cam.position = Vector3.Lerp(cam.position, transform.position.With(z: cam.position.z), Time.deltaTime*4f);
		cam.rotation = Quaternion.Lerp(cam.rotation, Quaternion.LookRotation(transform.position-cam.position, Vector3.up), Time.deltaTime*3f);
		lastPosition = transform.position;
	}

	private void OnTriggerEnter(Collider other) {
		if (hasArms) {
			Climbable climbable = other.GetComponent<Climbable>();
			if (climbable != null) {
				climbing = climbable;
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (hasArms) {
			Climbable climbable = other.GetComponent<Climbable>();
			if (climbable != null && rb.velocity.y < 0f) {
				climbing = climbable;
			}
		}
	}

	private void ApplyAccelleration(float multiplier = 1f) {
		float maxSpeed = Mathf.Max(rb.velocity.GroundVector().magnitude, speed);
		rb.velocity += input.x * Vector3.right * friction * speed * multiplier * Time.deltaTime;
		if (rb.velocity.GroundVector().magnitude > maxSpeed) rb.velocity = rb.velocity.GroundVector().normalized * maxSpeed + Vector3.up * rb.velocity.y;
	}

	private void ApplyFriction(float multiplier = 1f) {
		rb.velocity = rb.velocity.GroundVector() * (1f - friction * multiplier * Time.deltaTime) + Vector3.up * rb.velocity.y;
	}

	private void StopGlide() {
		gliding = false;
		wings.gameObject.SetActive(false);
		wingsFX.Stop();
	}

	private void startGlide() {
		gliding = true;
		wings.gameObject.SetActive(true);
		wingsFX.Play();
	}

	private void SetAnim(GameObject anim) {
		if (anim!=animIdle) animIdle.SetActive(false);
		if (anim != animRun) animRun.SetActive(false);
		if (anim != animJump) animJump.SetActive(false);
		if (anim != animGlide) animGlide.SetActive(false);
		if (anim != animClimb) animClimb.SetActive(false);
		anim.SetActive(true);
	}


}
