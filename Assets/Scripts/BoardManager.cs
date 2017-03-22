using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public int difficulty = 0;
	public int columns = 6;
	public int rows = 6;
	public GameObject door;
	public GameObject table;
	public GameObject trash;
    public GameObject pet;
	public GameObject wallTile;
	public GameObject[] floorTiles;
    public LayerMask blockingLayer;

	private float doggoFreq;
	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();

	// Creates list of all possible locations on the board
	void InitializeList () {
		gridPositions.Clear ();
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	// Places a game object tile at each location on the board
	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

                // places a wall tile if the location is an edge location
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = wallTile;

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	//the only public method - what the gameManager calls
	public void SetupScene () {
		BoardSetup ();
		InitializeList ();
		Instantiate (door, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		Instantiate (table, new Vector3 (0, rows - 1, 0f), Quaternion.identity);
		Instantiate (trash, new Vector3 (columns - 1, 0, 0f), Quaternion.identity);

		switch (difficulty) {
		case 0:
			doggoFreq = 20f;
			break;
		case 1:
			doggoFreq = 10f;
			break;
		case 2:
			doggoFreq = 5f;
			break;
		default:
			doggoFreq = 60f;
			break;
		}
		InvokeRepeating ("DoggoSpawner", 0.0f, doggoFreq);
	}

    private void DoggoSpawner() {
        StartCoroutine(CreateDoggo());
    }

	IEnumerator CreateDoggo() {
        // calculation to check whether spawn location is empty of not
        Vector3 start = new Vector3((float)columns - 1, (float)rows - 1, 0);
        Vector3 end = new Vector3((float)(columns - 2), (float)rows - 1, 0);
        BoxCollider2D doorCollider = GameObject.Find("Door(Clone)").GetComponent<BoxCollider2D>();
        doorCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        doorCollider.enabled = true;

        // if not empty, yield until it is
        if (hit.transform != null)
            yield return null;
        else
            Instantiate (pet, end, Quaternion.identity);
	}

	public int GetColumns() {
		return this.columns;
	}

	public int GetRows() {
		return this.rows;
	}

	public void SetDifficulty(int level) {
		difficulty = level;
	}
}