using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MovingObject {

    // Config
	private Animator girlAnimator;
	public AudioClip itemPickup;
	public AudioClip itemDropoff;
	public AudioClip itemTrashed;
	new AudioSource audio;

    // Controller Constants
    private string horizontalControls = "R_Horizontal";
    private string verticalControls = "R_Vertical";

    // Carrying object
	private Constants carryingObject = Constants.none;

    protected override void Start() {
		girlAnimator = gameObject.GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		base.Start();
    }
		
	void Update() {
		if (CanMove()) {
			int horizontal = (int)(Input.GetAxisRaw(horizontalControls));
			int vertical = (int)(Input.GetAxisRaw(verticalControls));
			if (horizontal != 0) {
				vertical = 0;
			}
			if (horizontal != 0 || vertical != 0) {
				RaycastHit2D hit;
				Transform transform = Move(horizontal, vertical, out hit);
				if (transform)
					interactWithObject(transform);
			}
		}
	}

	private void interactWithObject (Transform transform) {
		switch (transform.tag) {
		case "Pet":
			PetController pet = (PetController)transform.GetComponent (typeof(PetController));
			if (carryingObject == Constants.none) {
				pet.play ();
				girlAnimator.SetTrigger ("girlPlay");
			} else if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = Constants.none;
				girlAnimator.SetTrigger ("girlIdle");
				audio.PlayOneShot (itemDropoff);
			}
			break;
		case "Poop":
			if (carryingObject == Constants.none) {
				Destroy (transform.gameObject);
				carryingObject = Constants.poop;
				girlAnimator.SetTrigger ("girlPickUpPoop");
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "Kitchen":
			if (carryingObject == Constants.none) {
				if (decrementKitchenFood ()) {
					carryingObject = Constants.food;
					girlAnimator.SetTrigger ("girlPickUpFud");
					audio.PlayOneShot (itemPickup);
				}
			}
			break;
		case "Trash":
			if (carryingObject != Constants.none) {
				carryingObject = Constants.none;
				girlAnimator.SetTrigger ("girlIdle");
				audio.PlayOneShot (itemTrashed);
			}
			break;
		}
	}
}

