using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PetController : MovingObject {

    // Object initializations
    public GameObject Poop;
    private GameManager gameManager;
    private Animator animator;

    // Audio variables
    public AudioClip pooped;
    public AudioClip doggoFail;
    new AudioSource audio;

    // Doggo movement variables
    public float chargeCooldownMax = 1f;
    private float chargeCooldown = 0;
    private int xDir = 0;
    private int yDir = 0;

    // Doggo status variables
    public int happinessMax = 4;
    private int happiness;

    // Hungry Timer
    public float patienceMax = 7f;
    private float patience;
    public float hungerCooldownMax = 10f;
    private float hungerCooldown;
    private bool isHungry = false;
    public int reqFoodMax = 5;
    public int reqFoodMin = 3;
    private int reqFood;
    private int foodEaten = 0;


    // Play Cooldown
    public float playCooldownMax = 5f;
    private float playCooldown = 0;
    private bool hasPlayed = false;

    // Doggo poop timer
    public float poopTimerMax = 1f;
    private float poopTimer = 0;
    private bool needsToPoop = false;

    protected override void Start() {
        moveTime = Random.Range(0.5f, 1f);
        animator = gameObject.GetComponent<Animator>();
        gameManager = (GameManager)GameObject.Find("GameManager(Clone)").GetComponent(typeof(GameManager));
        audio = GetComponent<AudioSource>();
        happiness = happinessMax;
        patience = patienceMax;
        hungerCooldown = hungerCooldownMax;
        // TODO 0 is a bad number
        reqFood = Random.Range(reqFoodMin, reqFoodMax);
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

    void Update() {
        // Hunger variables update 
        if (isHungry) {
            if (patience >= 0) {
                patience -= Time.deltaTime;
            } else {
                happiness--;
                // TODO make the values random
                patience = patienceMax;
            }
            switch (happiness) {
                case 4:
                    animator.SetTrigger("greenFud");
                    break;
                case 3:
                    animator.SetTrigger("yellowFud");
                    break;
                case 2:
                    animator.SetTrigger("redFud");
                    break;
                case 1:
                    animator.SetTrigger("cryEmote");
                    break;
            }
        } else {
            if (hungerCooldown >= 0) {
                hungerCooldown -= Time.deltaTime;
            } else {
                hungerCooldown = hungerCooldownMax;
                isHungry = true;
            }
        }

        // Play variables update
        if (playCooldown >= 0) {
            playCooldown -= Time.deltaTime;
        } else {
            hasPlayed = false;
        }

        // Poop variables update
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
                    Instantiate (Poop, oldPos, Quaternion.identity);
					audio.PlayOneShot (pooped);
                    needsToPoop = false;
                }
            }
        } else {
            ChargeMove();
        }

		if (happiness <= 0) {
			gameManager.loseLife();
			audio.PlayOneShot (doggoFail);
			Destroy (gameObject);
		}

	}

    private bool Despawn() {
        if (foodEaten >= reqFood) {
			gameManager.IncreaseScore (30);
            // include animations for this
            Destroy(gameObject);
            return true;
        }
        return false;
    }

	public bool feed() {
		if (isHungry) {
            foodEaten++;
            Despawn();
			isHungry = false;
            happiness = 4;
			needsToPoop = true;
            poopTimer = Random.Range(0, poopTimerMax);
			gameManager.IncreaseScore (10);
			animator.ResetTrigger ("greenFud");
			animator.ResetTrigger ("yellowFud");
			animator.ResetTrigger ("redFud");
			animator.ResetTrigger ("cryEmote");
			animator.SetTrigger ("heartEmote");
			return true;
		}
		return false;
	}

	public void play() {
        if (!hasPlayed) {
            playCooldown = playCooldownMax;
            hasPlayed = true;
            if (happiness < 4) 
                happiness++;
			animator.ResetTrigger ("greenFud");
			animator.ResetTrigger ("yellowFud");
			animator.ResetTrigger ("redFud");
			animator.ResetTrigger ("cryEmote");
            animator.SetTrigger("dogPlay");
        }
	}
}