using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {
	public int sizeOfGrid;
	public GameObject gridSpace;

	// Use this for initialization
	void Start () {
		//makes the grid of objects
		GenerateGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void GenerateGrid () {
		for (int x = 0; x < sizeOfGrid; x++ ) {
			for (int y = 0; y < sizeOfGrid; y++) {
				GameObject square = (GameObject) Instantiate(gridSpace, new Vector2(x - sizeOfGrid/2, y - sizeOfGrid/2), Quaternion.identity);
				square.transform.parent = transform;

			}
		}
	}
}
