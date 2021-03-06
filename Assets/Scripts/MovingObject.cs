﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    // Object declarations
    public GameObject collisionBox;
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
	private Animator tableAnimator;

    public float moveTime = 0.1f;
    public static int kitchenCounter = 0;

    private float speed;
    private float moveCd = 0;

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
		tableAnimator = GameObject.Find ("Table(Clone)").GetComponent<Animator> ();
        speed = 1f / moveTime;

    }

    protected bool CanMove() {
        if (moveCd >= 0) {
            moveCd -= Time.deltaTime;
            return false;
        } else {
            return true;
        }
    }

   protected Transform Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (start == end) {
            return transform;
        }

        if (hit.transform == null) {
            Vector3 target = new Vector3(end.x, end.y, 0);
            GameObject instance = Instantiate(collisionBox, target, Quaternion.identity);
            instance.transform.parent = this.transform;
            StartCoroutine(SmoothMovement(end, instance));
            moveCd = moveTime * 3;
            return null;
        } else {
            return hit.transform;
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end, GameObject instance) {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        Destroy(instance);
    }

    protected void incrementKitchenFood() {
		if (kitchenCounter == 0) {
			tableAnimator.SetTrigger ("tableFud");
		}
        kitchenCounter += 10;
    }

    protected bool decrementKitchenFood() {
        if (kitchenCounter > 0) {
            kitchenCounter--;
			if (kitchenCounter == 0) {
				tableAnimator.SetTrigger ("tableIdle");
			}
            return true;
        }
        return false;
    }
}
