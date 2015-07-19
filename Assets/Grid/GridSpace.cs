//#define DEBUG

using UnityEngine;
using System.Collections;

public class GridSpace : MonoBehaviour {

	public enum Space {Solid, Empty, PickUp};

	public Space type {get; private set;}
	private SpriteRenderer sprite;
	private Collider2D myCollider;

	// if the square has a pick up in it, e.g. a plant or a pot
	private PickUp pickUp;
	public bool isSolid = true;

	void Awake () {
		Utility.EstablishReferences(gameObject, ref sprite, ref myCollider);
	}

	// Use this for initialization
	void Start () {

		#if DEBUG
		print (myCollider);
		
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleSolid (bool active) {
#if DEBUG
		print (myCollider);

#endif
		if (type != Space.Empty) {
			myCollider.isTrigger = !active;
		} else {
			myCollider.enabled = false;
		}
	}

	public void ToggleSprite (bool active) {
		sprite.enabled = active;
	}

	public void SetBehavior (Space type) {
		this.type = type;
		if (type == Space.Solid) {
			ToggleSolid(true);
			ToggleSprite(true);
			tag = Global.SOLID_TAG;
		} else if (type == Space.Empty) {
			ToggleSolid(false);
			ToggleSprite(false);
			tag = Global.EMPTY_TAG;
		} else if (type == Space.PickUp) {
			ToggleSolid(false);
			ToggleSprite(true);
			SetSprite(transform.parent.GetComponent<GridController>().GoldPot);
			transform.localScale = new Vector3(0.25f, 0.25f);
			sprite.transform.position += Vector3.down * 1.75f;
			tag = Global.PICK_UP_TAG;
		}
	}

	public void SetSprite (Sprite sprite) {
		this.sprite.sprite = sprite;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == Global.PLAYER_TAG) {
#if DEBUG 
			Debug.Log("Picked up");

#endif
		}
	}
}
