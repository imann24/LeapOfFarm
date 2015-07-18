using System.Collections;

public class Player {
	//creates the player
	public Player (float maxHealth, float speed, float baseJumpHeight) {
		this.maxHealth = maxHealth;
		this.speed = speed;
		this.baseJumpHeight = baseJumpHeight;
		resetPlayerStats();
	}


	//event calls
	public delegate void PlayerDamagedAction();
	public static event PlayerDamagedAction OnPlayerDamaged;
	public delegate void PlayerKilledAction();
	public static event PlayerKilledAction OnPlayerKilled;

	//player variables
	public float health;
	public float speed;
	public float jumpHeight;

	private float maxHealth;
	private float baseJumpHeight;

	//reset player stats
	public void resetPlayerStats () {
		health = maxHealth;
		jumpHeight = baseJumpHeight;
	}

	//damages the player
	public void damage (float damage) {
		if (health - damage >= 0) {
			health -= damage;
			if (OnPlayerDamaged != null) {
				OnPlayerDamaged();
			}
		} else {
			if (OnPlayerKilled != null) {
				OnPlayerKilled();
			}
		}
	}
}
