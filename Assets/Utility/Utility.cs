//#define DEBUG
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Utility {

	//overloaded method to establish reference
	public static void EstablishReferences<T> (GameObject myObject, ref T component) {
		component = myObject.GetComponent<T>();
	}

	public static void EstablishReferences (GameObject myObject, ref SpriteRenderer sprite) {
		sprite = myObject.GetComponent<SpriteRenderer>();
	}

	public static void EstablishReferences (GameObject myObject, ref Image image) {
		image = myObject.GetComponent<Image>();
	}

	public static void EstablishReferences (GameObject myObject, ref SpriteRenderer sprite, ref Collider2D collider) {
		sprite = myObject.GetComponent<SpriteRenderer>();
		collider = myObject.GetComponent<Collider2D>();
#if DEBUG
		Debug.Log (collider);
#endif
	}


}
