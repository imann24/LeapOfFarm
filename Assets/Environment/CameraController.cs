using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public bool isFollowingPlayer = true;

	// Update is called once per frame
	void Update () {
		if (isFollowingPlayer) {
			FollowPlayer();
		}
	}

	public void FollowPlayer () {
		transform.position = Vector3.Lerp(transform.position, new Vector3(Global.Player.transform.position.x, Global.Player.transform.position.y, transform.position.z), 1.0f);
	}
}
