//#define DEBUG
using UnityEngine;
using System.Collections;

public class VirtualButtonController : MonoBehaviour {
	private bool shouldDestroyButtons = true;
	// Use this for initialization
	void Start () {
		#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_WP8_1)
			shouldDestroyButtons = false;
		#endif
		//destroys the virtual buttons 
		if (shouldDestroyButtons) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
