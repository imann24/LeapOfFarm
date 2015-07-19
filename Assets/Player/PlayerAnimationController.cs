#define DEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {
	private Sprite stillIdleFrame;
	private Player player;
	private Animator animator;
	private SpriteRenderer sprite;

	public Image healthBar;

	//coroutines 
	IEnumerator healthBarAnimation; 

	// Use this for initialization
	void Start () {
		PlayerController.OnChangeDirection += ChangeDirection;
		Player.OnPlayerDamaged += UpdateHealthBar;

		SetReferences();
	}

	void OnDestroy () {
		PlayerController.OnChangeDirection -= ChangeDirection;
		Player.OnPlayerDamaged -= UpdateHealthBar;
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
				sprite.transform.rotation = new Quaternion(0, 180, 0, 0);
			} else if (previousDirection == PlayerController.Direction.Right) { 
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

	public void UpdateHealthBar (float damage) {
		if (healthBarAnimation == null) {
			healthBarAnimation = AnimateHealthBar(damage/100f);
			StartCoroutine(healthBarAnimation);
		}
	}

	public void FillHealthBar () {
		StartCoroutine(AnimateHealthBar(healthBar.fillAmount - 1f));
	}

	IEnumerator AnimateHealthBar (float healthChange) {
		float steps = 10f;
		float step = healthChange/steps;
		for (int i = 0; i < steps; i++) {
			healthBar.fillAmount -= step;
			yield return new WaitForEndOfFrame();
		}
	}
}
