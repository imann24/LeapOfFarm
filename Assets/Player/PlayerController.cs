//#define DEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//tuning variables
	public float PlayerHealth = 100f;
	public float DamageTolerance = 10f; //determines how hard player has to fall to take damage
	public float PlayerSpeed = 2.5f;
	public float PlayerJumpHeight = 150f;
	public Vector3 SpawnPosition;

	//events
	public delegate void ChangeDirectionAction (Direction direction);
	public static event ChangeDirectionAction OnChangeDirection;

	//player variables
	public Player player;
	private float playerHeight;
	private bool movingLeft;
	private bool movingRight;
	private bool jumping;
	private bool canJump;
	private bool invulnerable;
	private float baseSpeed = 0.25f;
	private float speed;
	private Direction currentDirection;

	//for touch input 
	private bool leftKeyDown;
	private bool leftTouchDown;
	private bool leftAxisDown;
	private bool rightKeyDown;
	private bool rightTouchDown;
	private bool rightAxisDown;

	//coroutine references
	private IEnumerator jumpCoroutine;
	private IEnumerator accelerateCoroutine;

	public enum Direction {Left, Right, None};

	//inititializing variables
	void Awake () {
		player = new Player(PlayerHealth, PlayerSpeed, PlayerJumpHeight);
	}

	// Use this for initialization
	void Start () {
		Global.Player = gameObject;
		transform.position = SpawnPosition;

		Player.OnPlayerKilled += RespawnPlayer;

		//sets the player
		GetComponent<PlayerAnimationController>().SetPlayer(player);
	}

	void OnDestroy () {
		Player.OnPlayerKilled -= RespawnPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		//checks for button input
		CheckForButtonInput();

		//controls player movement
		UpdatePosition();

		if (Input.GetKeyDown(KeyCode.R)) {
			RespawnPlayer();
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == Global.SOLID_TAG && 
		    coll.transform.position.y < transform.position.y && 
		    Mathf.Abs (transform.position.x - coll.transform.position.x) < 0.5f 
		    ) {
			canJump = true;

			//prevents the player from being damaged
			if (invulnerable) {
				return;
			}

			if (coll.relativeVelocity.magnitude > 10f) {
				player.Damage(coll.relativeVelocity.magnitude);
				InvulnerableDuration(0.1f);
#if DEBUG
				Debug.Log("Player health: " + player.health);
#endif
			}
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == Global.SOLID_TAG) {
			canJump = false;
		} 
	}

	float HorizontalMovement () {


		if (!(movingLeft || movingRight)) {
			speed = 0;
		}

		float translation = speed * player.speed * Time.deltaTime;
		if (movingLeft) {
			translation *= -1;	
		}
		if (UpdateDirection(translation) != currentDirection) {
			currentDirection = UpdateDirection(translation);
			if (OnChangeDirection != null) {
				OnChangeDirection(currentDirection);
			}
		}

		return transform.position.x + translation;
	}

	float VerticalMovement () {
		float jumpHeightIncrement = 5.0f;
		float translation = 0;

		if (jumping) {
			translation = jumpHeightIncrement * Time.deltaTime;
		}

		return translation + transform.position.y;
	}

	void UpdatePosition() {
		transform.position = new Vector3(HorizontalMovement(), VerticalMovement());
	}

	Direction UpdateDirection (float translation) {
		if (translation > 0) {
			return Direction.Right;
		} else if (translation < 0) {
			return Direction.Left;
		} else {
			return Direction.None;
		}
	}

	IEnumerator HaltJump () {
		while (!canJump) {
			yield return new WaitForEndOfFrame();
		}
		jumping = false;
	}

	public void LeftButtonPressed () {
		movingLeft = true;
		movingRight = false;
		StartAcceleration(Direction.Left);
	}

	public void RightButtonPressed () {
		movingRight = true;
		movingLeft = false;
		StartAcceleration(Direction.Right);
	}

	public void Jump () {
		//if (canJump) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, player.jumpHeight), ForceMode2D.Force);
			jumping = true;
			StartCoroutine(HaltJump());
		//}
	}

	public void LeftButtonDown () {
#if DEBUG
		Debug.Log("Left button down");
#endif
		leftKeyDown = true;
		movingLeft = true;
		leftTouchDown = true;
		LeftButtonPressed();
	}

	public void RightButtonDown () {
		rightKeyDown = true;
		movingRight = true;
		rightTouchDown = true;
		RightButtonPressed();
	}

	public void LeftButtonUp () {
		leftKeyDown = false;
		movingLeft = false;
		leftTouchDown = false;
	}

	public void RightButtonUp () {
		rightKeyDown = false;
		movingRight = false;
		rightTouchDown = false;
	}

	private void CheckForButtonInput () {

		if (!leftTouchDown) {
			foreach (KeyCode key in Global.LeftButton) {
				if (Input.GetKeyDown(key) ||(Input.GetAxis("Horizontal") < 0 && !leftAxisDown)) {
					leftKeyDown = true;
					LeftButtonPressed();
					if (Input.GetAxis("Horizontal") < 0) {
						leftAxisDown = true;
						rightAxisDown = false;
					}
					break;
				} 
			}

			movingLeft = false;
			foreach (KeyCode key in Global.LeftButton) {
				if (Input.GetKey(key) || Input.GetAxis("Horizontal") < 0) {
					movingLeft = true;
					break;
				} 
			}
		}

		if (!rightTouchDown) {
			foreach (KeyCode key in Global.RightButton) {
				if (Input.GetKeyDown(key) || (Input.GetAxis("Horizontal") > 0 && !rightAxisDown)) {
					rightKeyDown = true;
					RightButtonPressed();
					if (Input.GetAxis("Horizontal") > 0) {
						rightAxisDown = true;
						leftAxisDown = false;
					} 
				} 
			}

			movingRight = false;
			foreach (KeyCode key in Global.RightButton) {
				if (Input.GetKey(key) || Input.GetAxis("Horizontal") > 0) {
					movingRight = true;
					break;
				} 
			}
		}

		if (Input.GetAxis("Horizontal") == 0) {
			rightAxisDown = false;
			leftAxisDown = false;
		}

		foreach (KeyCode key in Global.JumpButton) {
			if ((Input.GetKeyDown(key) || Input.GetButton("Jump"))&& canJump) {
				jumping = true;
				Jump ();
				StartCoroutine(HaltJump());
				break;
			} 
		}
	}

	private void StartAcceleration (Direction direction) {
		if (accelerateCoroutine != null) {
			StopCoroutine (accelerateCoroutine);
		}
		accelerateCoroutine = HorizontalSpeed(direction);
		StartCoroutine(accelerateCoroutine);
	}

	IEnumerator HorizontalSpeed (Direction direction) { 
		speed = baseSpeed;
		bool isAccelerating = false;
		do {
			speed += 0.25f;
			isAccelerating = (direction == Direction.Left)?leftKeyDown:rightKeyDown;
			yield return new WaitForSeconds(0.1f);
		} while (speed < 1.5f && isAccelerating); 
	}
	IEnumerator InvulnerableDuration (float seconds) {
		yield return new WaitForSeconds(seconds);
		invulnerable = false;
	}

	public void RespawnPlayer () {
		transform.position = SpawnPosition;
		player.resetPlayerStats();
		GetComponent<PlayerAnimationController>().FillHealthBar();

	}
}
