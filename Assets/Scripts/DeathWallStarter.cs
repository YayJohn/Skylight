using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallStarter : MonoBehaviour {

	public GameObject DeathWall;

	void OnTriggerExit() {
		DeathWall.SetActive(true);
		DeathWall.GetComponent<DeathWallMover>().DeathWallMoverStarter = true;
	}
}
