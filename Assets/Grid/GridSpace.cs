using UnityEngine;
using System.Collections;

public class GridSpace : MonoBehaviour {

	public bool isSolid = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleSolid (bool active) {
		GetComponent<Collider>().isTrigger = !active;
	}
}
