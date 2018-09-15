using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallMover : MonoBehaviour {

	public bool DeathWallMoverStarter = false;

	void Update () {
		if (DeathWallMoverStarter)
			transform.Translate(0f, 0f, -10f * Time.deltaTime);
	}
}
