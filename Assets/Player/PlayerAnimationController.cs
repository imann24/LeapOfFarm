﻿//#define DEBUG

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
		//sets event references
		PlayerController.OnChangeDirection += ChangeDirection;
		Player.OnPlayerDamaged += UpdateHealthBar;

		//establishes component references
		SetReferences();
	}

	//dereferences the animation event calls
	void OnDestroy () {
		PlayerController.OnChangeDirection -= ChangeDirection;
		Player.OnPlayerDamaged -= UpdateHealthBar;
	}

	//plays the walking animation for the player	
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
	
	//sets the player class
	public void SetPlayer (Player player) {
		this.player = player;
	}

	public void UpdateHealthBar (float damage) {
		if (healthBarAnimation != null) {
			StopCoroutine(healthBarAnimation);
		}
		healthBarAnimation = AnimateHealthBar();
		StartCoroutine(healthBarAnimation);
	}

	public void FillHealthBar () {
		StartCoroutine(AnimateHealthBar());
	}

	IEnumerator AnimateHealthBar () {

		float newValue = player.health/GetComponent<PlayerController>().PlayerHealth;
		float oldValue = healthBar.fillAmount;
		float healthChange = newValue - oldValue;
		float steps = 10f;
		float step = healthChange/steps;

		//gives it a little tolerance
		while (Mathf.Abs(healthBar.fillAmount - newValue) < 0.05f) {
			healthBar.fillAmount += step;
			yield return new WaitForEndOfFrame();
		}

		//sets the health bar value
		healthBar.fillAmount = newValue;

		//sets the coroutine reference to null
		healthBarAnimation = null;
#if DEBUG
		Debug.Log("Displayed health: " + healthBar.fillAmount * GetComponent<PlayerController>().PlayerHealth);
#endif
	}
}
