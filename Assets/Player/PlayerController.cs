#define DEBUG

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//events
	public delegate void ChangeDirectionAction (Direction direction);
	public static event ChangeDirectionAction OnChangeDirection;

	//player variables
	private Player player = new Player(100f, 2.5f, 2.5f);
	private float playerHeight;
	private bool jumping;
	private Direction currentDirection;

	//coroutine references
	private IEnumerator jumpCoroutine;
	public enum Direction {Left, Right, None};
	// Use this for initialization
	void Start () {
		Global.Player = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//controls player movement
		UpdatePosition();
	}
	
	float HorizontalMovement () {
		float translation = Input.GetAxis("Horizontal") * player.speed * Time.deltaTime;
		if (UpdateDirection(translation) != currentDirection) {
			currentDirection = UpdateDirection(translation);
			if (OnChangeDirection != null) {
				OnChangeDirection(currentDirection);
			}
		}
		return transform.position.x + translation;
	}

	float VerticalMovement () {
		float jumpTime = 1f;
		float jumpHeightIncrement = 5.0f;
		float translation = 0;

		if (Input.GetButtonDown("Jump")) {
			jumping = true;
			StartCoroutine(HaltJump(jumpTime));
		}

		if (jumping) {
			translation = jumpHeightIncrement * Time.deltaTime;
		}

#if DEBUG
		Debug.Log(translation);
#endif

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

	IEnumerator HaltJump (float seconds) {
		yield return new WaitForSeconds(seconds);
		jumping = false;
	}
}
