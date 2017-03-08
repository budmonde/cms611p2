using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MovingObject {
	public bool isCarryingPoop = false;

	void Start() {
	}

	void Update() {
		int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		int vertical = (int) (Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0) {
			vertical = 0;
		} else if (vertical != 0) {
			horizontal = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			Move(horizontal, vertical);
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
			Debug.Log ("found pet!");
			break;
		case "poop":
			Debug.Log ("found poop :(");
			isCarryingPoop = true;
			Destroy (obj);
			break;
		case "food":
			Debug.Log("found food");
			break;
		}
	}



}

