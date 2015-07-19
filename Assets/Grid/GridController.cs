using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {
	//grid sprites
	public Sprite GoldPot, ClayPot;

	//tuning variables
	public int sizeOfGrid;
	public GameObject gridSpace;
	public float scale = 1.28f;

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
				GameObject square = (GameObject) Instantiate(gridSpace, new Vector2(x * scale - sizeOfGrid/2 * scale, y * scale - sizeOfGrid/2 * scale), Quaternion.identity);
				square.transform.parent = transform;
				square.GetComponent<GridSpace>().SetBehavior((GridSpace.Space) Random.Range(0, 3));
			}
		}

		//draws a box around the whole game
		for (int i = 0; i < sizeOfGrid + 8; i++) { 
			//above and below
			GameObject square = (GameObject) Instantiate(gridSpace, new Vector2(i * scale - sizeOfGrid/2 * scale - 4 * scale, sizeOfGrid-1 * scale + scale), Quaternion.identity);
			square.transform.parent = transform;
			square.GetComponent<GridSpace>().SetBehavior(GridSpace.Space.Solid);
			square.name = "GridSpaceAbove";

			square = (GameObject) Instantiate(gridSpace, new Vector2(i * scale - sizeOfGrid/2 * scale - 4 * scale, -sizeOfGrid * scale + scale), Quaternion.identity);
			square.transform.parent = transform;
			square.GetComponent<GridSpace>().SetBehavior(GridSpace.Space.Solid);
			square.name = "GridSpaceBelow";

			//left and right
			square = (GameObject) Instantiate(gridSpace, new Vector2( -sizeOfGrid * scale, i * scale - sizeOfGrid/2 * scale - 4 * scale), Quaternion.identity);
			square.transform.parent = transform;
			square.GetComponent<GridSpace>().SetBehavior(GridSpace.Space.Solid);
			square.name = "GridSpaceLeft";

			square = (GameObject) Instantiate(gridSpace, new Vector2((sizeOfGrid * scale) - scale, i * scale - sizeOfGrid/2 * scale - 4 * scale), Quaternion.identity);
			square.transform.parent = transform;
			square.GetComponent<GridSpace>().SetBehavior(GridSpace.Space.Solid);
			square.name = "GridSpaceRight";

		}
		//destroys the original object
		Destroy (gridSpace);
	}
}
