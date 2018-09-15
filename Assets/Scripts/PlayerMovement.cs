using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;
	public float speed;
	public float jumpSpeed;
	public float wallrunSpeed;
	public float slidingSpeed;
	public float sensitivity;
	public GameObject camera;
	//the same collision in OnCollisionEnter just useable by the whole script
	Collision exportedCollision;
	//same as exportedCollision just for the Collider
	Collider exportedCollider;
	bool grounded = true;
	float mouseRotationX;
	float mouseRotationY;
	bool wallrunTimerStarter = false;
	float wallrunTimer = 1.5f;
	bool stopWallRunning = false;
	bool grabbing = false;
	GameObject foundedGameObject;
	public GameManager gameManager;
	bool enableMouseMovement = true;
	bool rotateRight = false;
	bool rotateLeft = false;
	int timesRotated = 0;
	Quaternion cameraRotation;
	bool doingTrick = false;
	float score = 0;
	float currentScore = 0;
	float scoreMultiplier = 1;
	bool scoreMultiplierStopper = false;
	float scoreMultiplierStopperTimer = 3f;
	public Text scoreText;

	public void AddScorePoints(float scoreInput) {
		currentScore += scoreInput * scoreMultiplier;
		scoreMultiplier += 1;
		scoreText.enabled = true;
		scoreText.text = currentScore.ToString();
		scoreMultiplierStopperTimer = 3f;
		scoreMultiplierStopper = true;
	}
	void FixedUpdate() {
		if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f || Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
			if (Input.GetKey(KeyCode.S))
				transform.Translate(new Vector3(speed / 1.5f * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0f, speed / 1.5f * Input.GetAxisRaw("Vertical") * Time.deltaTime));
			else
				transform.Translate(new Vector3(speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0f, speed * Input.GetAxisRaw("Vertical") * Time.deltaTime));

		if (Input.GetKeyDown(KeyCode.Space) && grounded) {
			rb.velocity = new Vector3(rb.velocity.x * Time.deltaTime, Input.GetAxisRaw("Jump") * jumpSpeed * Time.deltaTime);
			grounded = false;
		}

		if (wallrunTimer > 0f && wallrunTimerStarter) {
			transform.Translate(new Vector3(0f, wallrunSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime, 0f));
		}

		if (rotateRight) {
			if (grounded) {
				score += currentScore;
				currentScore = 0;
				timesRotated = 0;
				enableMouseMovement = true;
				doingTrick = false;
				rotateRight = false;
			} else {
				camera.transform.Rotate(new Vector3(0f, 13.9166667f, 0f));
				timesRotated += 1;
				if (timesRotated >= 25) {
					camera.transform.Rotate(new Vector3(0f, 12.0833323f, 0f));
					AddScorePoints(100);
					timesRotated = 0;
					enableMouseMovement = true;
					doingTrick = false;
					rotateRight = false;
				}
			}
		}

		if (rotateLeft) {
			if (grounded) {
				score += currentScore;
				currentScore = 0;
				timesRotated = 0;
				enableMouseMovement = true;
				doingTrick = false;
				rotateLeft = false;
			} else {
				camera.transform.Rotate(new Vector3(0f, -13.9166667f, 0f));
				timesRotated += 1;
				if (timesRotated >= 25) {
					camera.transform.Rotate(new Vector3(0f, -12.0833323f, 0f));
					AddScorePoints(100);
					timesRotated = 0;
					enableMouseMovement = true;
					doingTrick = false;
					rotateLeft = false;
				}
			}
		}
	}

	void Update () {
		if (grabbing) {
			if (Input.GetAxisRaw("Jump") > 0.5f)
				grabbing = false;
			else
				transform.position = exportedCollider.gameObject.transform.position;
		} else if (rb.useGravity == false)
			rb.useGravity = true;

		if (enableMouseMovement) {
			mouseRotationX = Input.GetAxis("Mouse X") * sensitivity;
			mouseRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
			mouseRotationY = Mathf.Clamp(mouseRotationY, -80f, 80f);

			transform.Rotate(0f , mouseRotationX, 0f);
			camera.transform.localRotation = Quaternion.Euler(mouseRotationY, 0, 0);
		}

		//when starting to wallrun this timer will start, so you wouldn't wallrun to eternitiy :)
		if (wallrunTimerStarter) {
			//if you touched something that isnt a platform and the timer didnt stop, it will stop it
			if (exportedCollision.gameObject.tag != "Platform") {
				stopWallRunning = true;
				wallrunTimer = 1.5f;
				wallrunTimerStarter = false;
			// makes the timer actually work and makes u go up
			} else if (wallrunTimer > 0f) {
				wallrunTimer -= Time.deltaTime;
			//stops the timer so you cant continue wallrunning and to wallrun again you need to a object tagged with ground
			} else {
				stopWallRunning = true;
				wallrunTimer = 1.5f;
				wallrunTimerStarter = false;
			}
		}

		if (exportedCollision.gameObject.tag != "Platform" && stopWallRunning) {
			stopWallRunning = false;
		}

		if (scoreMultiplierStopper) {
			if (scoreMultiplierStopperTimer > 0) {
				if (grounded == false)
					scoreMultiplierStopperTimer -= Time.deltaTime;
				else
					scoreMultiplierStopperTimer = 3f;
			} else {
				score += currentScore;
				scoreText.enabled = false;
				currentScore = 0;
				scoreMultiplier = 0;
				scoreMultiplierStopperTimer = 3f;
				scoreMultiplierStopper = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.E) && grabbing == false && grounded == false && wallrunTimerStarter == false && doingTrick == false) {
			doingTrick = true;
			enableMouseMovement = false;
			cameraRotation = camera.transform.rotation;
			rotateRight = true;
		}

		if (Input.GetKeyDown(KeyCode.Q) && grabbing == false && grounded == false && wallrunTimerStarter == false && doingTrick == false) {
			doingTrick = true;
			enableMouseMovement = false;
			cameraRotation = camera.transform.rotation;
			rotateLeft = true;
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name != "Ground" && collision.gameObject.GetComponent<WallJumpChecker>().jumpable) {
			grounded = true;
		}
		if (collision.gameObject.name == "Ground") {
			grounded = true;
				for (int i = 0; i < i + 1; i++) {
					foundedGameObject = GameObject.Find("/Wallrun Part/Wall (" + i + ")");
					if (foundedGameObject == null)
						break;
					else
						GameObject.Find("/Wallrun Part/Wall (" + i + ")").GetComponent<WallJumpChecker>().jumpable = true;
				}

				for (int i = 0; i < i + 1; i++) {
					foundedGameObject = GameObject.Find("/Wall (" + i + ")");
					if (foundedGameObject == null)
						break;
					else
						GameObject.Find("/Wall (" + i + ")").GetComponent<WallJumpChecker>().jumpable = true;
				}
		}
		
		print(collision.gameObject.tag);
		if (collision.gameObject.tag == "Platform" && stopWallRunning == false) {
			print("hello njigg");
			wallrunTimerStarter = true;
		}

		exportedCollision = collision;
	}

	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag == "Grabable") {
			grabbing = true;
			rb.useGravity = false;
		}

		exportedCollider = collision;
	}
}
