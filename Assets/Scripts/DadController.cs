using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MovingObject {

    private Constants carryingObject = Constants.none;	
    private string horizontalControls = "L_Horizontal";
    private string verticalControls = "L_Vertical";

	public AudioClip itemPickup;
	AudioSource audio;

    protected override void Start() {
        base.Start();
		audio = GetComponent<AudioSource> ();
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
                Move(horizontal, vertical, out hit);
            }
        }
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other) {
		if (other == null) {
			return;
		}
		GameObject obj = other.gameObject;
		switch (obj.tag) {
		case "pet":
			Debug.Log ("found pet!"); //TODO: assumed pet's bool methods: pet.feed() pet.done()
			PetController pet = (PetController)obj.GetComponent (typeof(PetController));
			if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = Constants.none;
			} else if (pet.isDone) {
				Destroy (obj);
			}
			break;
		case "poop":
			Debug.Log ("found poop :(");
			if (!isCarrying ()) {
				carryingObject = Constants.poop;
				Destroy (obj);
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "kitchen":
			Debug.Log ("found kitchen");
			if (carryingObject == Constants.kitchen) {
				incrementKitchenFood ();
                    carryingObject = Constants.none;
			} else if (!isCarrying ()) {
				audio.PlayOneShot (itemPickup);
				if (decrementKitchenFood ()) {
                    carryingObject = Constants.food;
				}
			}
			break;
		case "door":
			Debug.Log ("found door");
			if (!isCarrying ()) {
                carryingObject = Constants.delivery;
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "trash":
			Debug.Log ("found trash");
            carryingObject = Constants.none;
			break;
		}
	}

	public bool isCarrying() {
        return carryingObject != Constants.none;
	}

	public Constants getCarryingObject() {
		if (isCarrying ()) {
			return carryingObject;
		} else {
			return Constants.none;
		}
	}
}

