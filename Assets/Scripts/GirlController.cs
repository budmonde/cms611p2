using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MovingObject {
	private string carryingObject = ""; //poop, food, brush, 

	//	void Start() {
	//	}

	void Update() {
		int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		int vertical = (int) (Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0) {
			vertical = 0;
		} else if (vertical != 0) {
			horizontal = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			RaycastHit2D hit;
			Move(horizontal, vertical, out hit);
		}
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other) {
		if (other == null) {
			return;
		}
		GameObject obj = other.gameObject;
		switch (obj.tag) {
//		case "pet":
//			Debug.Log ("found pet!"); //TODO: assumed pet's bool methods: pet.feed() pet.done()
//			PetController pet = (PetController)obj.GetComponent (typeof(PetController));
//			if (carryingObject == "food" && pet.feed ()) {
//				carryingObject = "";
//			} else if (carryingObject == "brush") {
//				pet.groom ();
//			} else if (pet.play()) {
//				//some animation here
//			}
//			break;
		case "poop":
			Debug.Log ("found poop :(");
			if (!isCarrying ()) {
				carryingObject = "poop";
				Destroy (obj);
			}
			break;
		case "groomingContainer":
			Debug.Log ("found brush");
			if (!isCarrying ()) {
				carryingObject = "brush";
				Destroy (obj);
			} else if (carryingObject == "brush") {
				carryingObject = "";
			}
			break;			
		case "kitchen":
			Debug.Log ("found kitchen");
			if (!isCarrying ()) {
				if (decrementKitchenFood ()) {
					carryingObject = "food";
				}
			}
			break;
		case "trash":
			Debug.Log ("found trash");
			carryingObject = "";
			break;
		}
	}

	public bool isCarrying() {
		return carryingObject.Length < 1;
	}

	public string getCarryingObject() {
		if (isCarrying ()) {
			return carryingObject;
		} else {
			return "";
		}
	}



}

