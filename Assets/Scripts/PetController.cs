using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PetController : MovingObject {

    // amount of time until pet HAS to change direction
    public float chargeCooldownMax = 1f;
	public bool isDone = false;
	public GameObject Poop;

	private GameManager gameManager;
    private float chargeCooldown;
	private float taskTimer = 4f;
	private float taskFrequency;
	private bool isHungry = false;
	private int happiness = 3;
    private int x = 0;
    private int y = 0;

	protected override void Start () {
		//gameManager = (GameManager)GameObject.Find("Manager").GetComponent(typeof(GameManager));
        moveTime = Random.Range(0.5f, 1f);
        ChargeMove();
        base.Start();
	}

    private void ChargeMove() {
        chargeCooldown = Random.Range(0, chargeCooldownMax);
		taskFrequency = Random.Range (0, 6f);
        x = Random.Range(-1, 2);
        y = Random.Range(-1, 2);
        if (x != 0) {
            y = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (happiness < 1) {
			//gameManager.incrementUnhappyPetCounter();
			Destroy (this);
		}
		if (!isHungry && taskFrequency <= 0) {
			isHungry = true;
		} else {
			taskFrequency -= Time.deltaTime;
		}
        if (chargeCooldown >= 0) {
            chargeCooldown -= Time.deltaTime;
            if (CanMove()) {
                RaycastHit2D hit;
                Move(x, y, out hit);
            }
			makePoop (1);
        } else {
            ChargeMove();
        }
		if (taskTimer >= 0) {
			taskTimer -= Time.deltaTime;
		} else {
			happiness--;
			taskTimer = 4f;
		}
	}

	public bool feed() {
		if (isHungry) {
			isHungry = false;
			taskFrequency = Random.Range (0, 6f);
			if (Random.Range (0, 3) == 1) {
				isDone = true;
			}
			makePoop (1);
			return true;
		}
		return false;
	}

	private IEnumerator makePoop(int poopTime) {
		yield return new WaitForSecondsRealtime (poopTime);
		GameObject newPoop = (GameObject)Instantiate (Poop);
		newPoop.transform.position = this.transform.position;
	}
}