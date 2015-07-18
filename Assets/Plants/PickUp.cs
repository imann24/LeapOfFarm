using System.Collections;

public abstract class PickUp {
	public string name {get; private set;}
	public PickUp (string name) {
		this.name = name;
	}

	public abstract void OnPickUp ();
}
