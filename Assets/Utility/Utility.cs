//#define DEBUG
using UnityEngine;
using System.Collections;

public class Utility {

	public static void EstablishReferences (GameObject myObject, ref SpriteRenderer sprite) {
		sprite = myObject.GetComponent<SpriteRenderer>();
	}

	public static void EstablishReferences (GameObject myObject, ref SpriteRenderer sprite, ref Collider2D collider) {
		sprite = myObject.GetComponent<SpriteRenderer>();
		collider = myObject.GetComponent<Collider2D>();
#if DEBUG
		Debug.Log (collider);
#endif
	}
}
