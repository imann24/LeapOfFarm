//#define DEBUG

using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {
	private Sprite stillIdleFrame;

	private Animator animator;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		PlayerController.OnChangeDirection += ChangeDirection;

		SetReferences();
	}

	void OnDestroy () {
		PlayerController.OnChangeDirection -= ChangeDirection;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void ChangeDirection (PlayerController.Direction direction) {
		PlayerController.Direction previousDirection;
		animator.enabled = true;
		if (animator.GetBool("movingLeft")) {
			previousDirection = PlayerController.Direction.Left;
		} else if (animator.GetBool("movingRight")) {
			previousDirection = PlayerController.Direction.Right;
		} else {
			previousDirection = PlayerController.Direction.None;
		}

		if (direction == PlayerController.Direction.Left) {
			animator.SetBool("movingLeft", true);
			animator.SetBool("movingRight", false);
		} else if (direction == PlayerController.Direction.Right) {
			animator.SetBool("movingLeft", false);
			animator.SetBool("movingRight", true);
		} else {
			animator.SetBool("movingLeft", false);
			animator.SetBool("movingRight", false);

			animator.enabled = false;
			sprite.sprite = stillIdleFrame;
			//sets the sprite if not moving
			if (previousDirection == PlayerController.Direction.Left) {
#if DEBUG
				Debug.Log("Set sprite to the left facing one");
#endif
				//renderer.sprite = stillLeftFacingFrame;
				sprite.transform.rotation = new Quaternion(0, 180, 0, 0);
			} else if (previousDirection == PlayerController.Direction.Right) { 
				#if DEBUG
				Debug.Log("Set sprite to the right facing one");
				#endif
				sprite.transform.rotation = new Quaternion(0, 0, 0, 0);
			}
		}
	}

	//sets the starting references
	void SetReferences () {
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		stillIdleFrame = sprite.sprite;
	}
}
