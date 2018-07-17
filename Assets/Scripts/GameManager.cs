using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject player;
	Vector3 spawnpoint = new Vector3(0f, 2.48f, 0f);
	GameObject foundedGameObject;

	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
			player.transform.position = spawnpoint;
	}

	public void ResetWallJumpable() {
		
	}
}
