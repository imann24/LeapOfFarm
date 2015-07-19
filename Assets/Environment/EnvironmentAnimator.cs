#define DEBUG
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnvironmentAnimator : MonoBehaviour {
	Image image;
	private float animationTime = 0.5f;

	// Use this for initialization
	void Start () {
		//etsablishes references to images
		Utility.EstablishReferences(gameObject, ref image); 
		image.color = Global.Transparent;
		Player.OnPlayerDamaged += DamageAnimation;
	}

	void Destroy () {
		//unlinks the animation
		Player.OnPlayerDamaged -= DamageAnimation;
	}


	private void DamageAnimation (float damage) {
		float fadeTime = animationTime/2.0f;
		StartCoroutine(ColorFade(Global.Transparent, Global.SemiTransparent, fadeTime, true));
	}

	IEnumerator ColorFade (Color startColor, Color endColor, float seconds) {
		float steps = 10f;
		float timeStep = seconds/steps;
		float progress = 0;

		for (int i = 0; i < steps; i++) {
			image.color = Color.Lerp(startColor, endColor, progress);
			progress += timeStep;
			yield return new WaitForEndOfFrame();
		}
		image.color = Color.Lerp(startColor, endColor, 1.0f);
	}

	//overloaded method that repeats the fade and swaps the colors
	IEnumerator ColorFade (Color startColor, Color endColor, float seconds, bool reverseFade) {
		float steps = 10f;
		float timeStep = seconds/steps;
		float progress = 0;
		
		for (int i = 0; i < steps; i++) {
			image.color = Color.Lerp(startColor, endColor, progress);
			progress += timeStep;
			yield return new WaitForEndOfFrame();
		}
		image.color = Color.Lerp(startColor, endColor, 1.0f);
	
		//fades the image back to start color
		if (reverseFade) {
			StartCoroutine(ColorFade(endColor, startColor, seconds));
		}
	}
}
