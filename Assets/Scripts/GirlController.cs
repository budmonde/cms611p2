using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MovingObject {

	private Constants carryingObject = Constants.none;
    private string horizontalControls = "R_Horizontal";
    private string verticalControls = "R_Vertical";
	private Animator girlAnimator;

	public AudioClip itemPickup;
	public AudioClip itemDropoff;
	public AudioClip itemTrashed;
	new AudioSource audio;

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

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void interactWithObject (Transform transform) {
		switch (transform.tag) {
		case "Pet":
			Debug.Log ("found pet!");
			PetController pet = (PetController)transform.GetComponent (typeof(PetController));
			if (carryingObject == Constants.none) {
				Debug.Log ("play!");
				pet.play ();
				girlAnimator.SetTrigger ("girlPlay");
			} else if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = Constants.none;
				girlAnimator.SetTrigger ("girlIdle");
				audio.PlayOneShot (itemDropoff);
			}
			break;
		case "Poop":
			Debug.Log ("found poop :(");
			if (carryingObject == Constants.none) {
				carryingObject = Constants.poop;
				girlAnimator.SetTrigger ("girlPickUpPoop");
				audio.PlayOneShot (itemPickup);
				Destroy (transform.gameObject);
			}
			break;
		case "Kitchen":
			Debug.Log ("found kitchen");
			if (carryingObject == Constants.none) {
				if (decrementKitchenFood ()) {
					carryingObject = Constants.food;
					girlAnimator.SetTrigger ("girlPickUpFud");
					audio.PlayOneShot (itemPickup);
				}
			}
			break;
		case "Trash":
			Debug.Log ("found trash");
			if (carryingObject != Constants.none) {
				carryingObject = Constants.none;
				girlAnimator.SetTrigger ("girlIdle");
				audio.PlayOneShot (itemTrashed);
			}
			break;
		}
	}
}

