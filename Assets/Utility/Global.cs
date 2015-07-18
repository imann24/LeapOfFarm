using UnityEngine;
using System.Collections;

public class Global {
	public static GameObject Player;	
	public const string PLAYER_TAG = "Player";
	public const string SOLID_TAG = "Ground";
	public const string EMPTY_TAG = "Empty";
	public const string PICK_UP_TAG = "PickUp";

	public static KeyCode [] LeftButton = {KeyCode.LeftArrow, KeyCode.A};
	public static KeyCode [] RightButton = {KeyCode.RightArrow, KeyCode.D};
	public static KeyCode [] JumpButton = {KeyCode.Space};
}
