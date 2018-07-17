using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpChecker : MonoBehaviour {

	public bool jumpable = true;

	void OnCollisionExit(Collision Collision) {
		if (Collision.gameObject.name == "Player")
			jumpable = false;
	}
}
