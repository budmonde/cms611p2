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

	public AudioClip pooped;
	public AudioClip doggoFail;
	new AudioSource audio;

//	private Animator animator;
	private GameManager gameManager;
    private int xDir = 0;
    private int yDir = 0;
    private float chargeCooldown = 0;
	private float taskTimer = 20f;
    private bool needsToPoop = false;
    private float poopTimer = 0;
	private bool isHungry = true;
	private int happiness = 4;


	private Animator animator;

	protected override void Start () {
		animator = gameObject.GetComponent<Animator> ();
		gameManager = (GameManager) GameObject.Find("GameManager(Clone)").GetComponent(typeof(GameManager));
		audio = GetComponent<AudioSource> ();
        moveTime = Random.Range(0.5f, 1f);
        ChargeMove();
        base.Start();
	}

    private void ChargeMove() {
        chargeCooldown = Random.Range(0, chargeCooldownMax);
        xDir = Random.Range(-1, 2);
        yDir = Random.Range(-1, 2);
        if (xDir != 0) {
            yDir = 0;
        }
    }

	// Update is called once per frame
	void Update () {
		var animator = gameObject.GetComponent<Animator>();
		if (isHungry) {
			switch (happiness) {
			case 4:
				animator.SetTrigger ("greenFud");
				break;
			case 3: 
				animator.SetTrigger ("yellowFud");
				break;
			case 2:
				animator.SetTrigger ("redFud");
				break;
			case 1:
				animator.SetTrigger ("cryEmote");
				break;
			}
		}

		if (happiness < 1) {
			gameManager.incrementUnhappyPetCounter();
			audio.PlayOneShot (doggoFail);
			Destroy (gameObject);
		}

        // amount of time between tasks
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
                Transform transform = Move (xDir, yDir, out hit);
                if (!transform && needsToPoop && poopTimer < 0) {
					audio.PlayOneShot (pooped);
                    Instantiate (Poop, oldPos, Quaternion.identity);
                    needsToPoop = false;
                }
            }
        } else {
            ChargeMove();
        }
	}

	public bool feed() {
		if (isHungry) {
			isHungry = false;
			gameManager.IncreaseScore (10);
			animator.SetTrigger ("heartEmote");
			needsToPoop = true;
            poopTimer = Random.Range(0, poopTimerMax);
			return true;
		}
		return false;
	}

	public void play() {
		Debug.Log ("playing");
		animator.SetTrigger ("dogPlay");
	}
}