using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PetController : MovingObject {

    // amount of time until pet HAS to change direction
    public float chargeCooldownMax = 1f;
    public float poopTimerMax = 1f;

	public bool isDone = false;
	public GameObject Poop;

	private GameManager gameManager;
    private int xDir = 0;
    private int yDir = 0;
    private float chargeCooldown = 0;
	private float taskTimer = 20f;
	private float taskFrequency;
    private bool needsToPoop = false;
    private float poopTimer = 0;
	private bool isHungry = false;
	private int happiness = 3;

	protected override void Start () {
		gameManager = (GameManager) GameObject.Find("GameManager(Clone)").GetComponent(typeof(GameManager));
        moveTime = Random.Range(0.5f, 1f);
        ChargeMove();
        base.Start();
	}

    private void ChargeMove() {
        chargeCooldown = Random.Range(0, chargeCooldownMax);
		taskFrequency = Random.Range (0, 6f);
        xDir = Random.Range(-1, 2);
        yDir = Random.Range(-1, 2);
        if (xDir != 0) {
            yDir = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (happiness < 1) {
			gameManager.incrementUnhappyPetCounter();
			Destroy (gameObject);
		}
        // amount of time between tasks
		if (!isHungry && taskFrequency >= 0) {
			taskFrequency -= Time.deltaTime;
		} else {
			isHungry = true;
		}
		if (taskTimer >= 0) {
			taskTimer -= Time.deltaTime;
		} else {
			happiness--;
			taskTimer = 4f;
		}
        if (poopTimer >= 0) {
            poopTimer -= Time.deltaTime;
        }
        // movement code
        if (chargeCooldown >= 0) {
            chargeCooldown -= Time.deltaTime;
            if (CanMove()) {
                RaycastHit2D hit;
                Vector3 oldPos = this.transform.position;
				if (this.transform.position.x + xDir == 8 &&
				    this.transform.position.y + yDir == 9) {
					Transform transform = Move (-2, 0, out hit);
					if (!transform && needsToPoop && poopTimer < 0) {
						Instantiate (Poop, oldPos, Quaternion.identity);
						needsToPoop = false;
					}
				} else {
					Transform transform = Move (-xDir, -yDir, out hit);
					if (!transform && needsToPoop && poopTimer < 0) {
						Instantiate (Poop, oldPos, Quaternion.identity);
						needsToPoop = false;
					}
				}
					
            }
        } else {
            ChargeMove();
        }
	}

	public bool feed() {
		if (isHungry) {
			isHungry = false;
			taskFrequency = Random.Range (0, 6f);
			if (Random.Range (0, 3) == 1) {
				isDone = true;
				gameManager.IncreaseScore (10);

			}
            needsToPoop = true;
            poopTimer = Random.Range(0, poopTimerMax);
			return true;
		}
		return false;
	}
}