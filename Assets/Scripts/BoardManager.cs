using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int difficulty = 0;
	public int columns = 8;
	public int rows = 8;
	public Count furnitureCount = new Count (10, 15);
	public GameObject door;
	public GameObject Trash;

	// these will contain prefabs
	public GameObject[] floorTiles;
	public GameObject[] furnitureTiles;
	public GameObject[] wallTiles;
	public GameObject[] petTiles;

	// boardHolder is just to make game hierarchy look nicer
	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();
	private float doggoFreq;

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
					toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);

			}
		}
	}

	// Generates random position for an object to be placed
	// prevents 2 objects from being spawned in the same location
	Vector3 RandomPosition () {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectsAtRandom (GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		// spawn as many objects as objectCount
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	//the only public method - what the gameManager calls
	public void SetupScene (int doggos) {
		BoardSetup ();
		InitializeList ();
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
		LayoutObjectsAtRandom (furnitureTiles, furnitureCount.minimum, furnitureCount.maximum);
		InvokeRepeating ("CreateDoggo", 0.0f, doggoFreq);
		Instantiate (door, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		Instantiate (Trash, new Vector3 (0, 5, 0f), Quaternion.identity);
	}

	void CreateDoggo() {
		Instantiate (petTiles [0], new Vector3 (8, 9, 0f), Quaternion.identity);
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

