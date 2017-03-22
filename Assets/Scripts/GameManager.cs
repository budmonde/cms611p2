using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public float maxTime = 300f;
    public float time = 0f;
    public int lives = 5;
	public int score = 0;

    public float levelEndDelay = 1f;
    private Text lifeText;
    private GameObject levelImage;

    void Awake() {
        // Make sure there are no more than one GameManager instances
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        // Initialize the game
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }
	
    void Update() {
        time += Time.deltaTime;
        int displayTime = (int)maxTime - (int)time;
        lifeText.text = "Time: " + (int)displayTime + "\nLives: " + lives + "\nScore: " + score;
        if (isGameOver()) {
            Invoke("GameOver", levelEndDelay);
        }
    }
    private void InitGame() {
        // Set the start text
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeText.text = "Time: " + maxTime + "\nLives: " + lives + "\nScore: " + score;

        // Setup the Scene
        boardScript.SetupScene();
    }

	public void IncreaseScore(int increase) {
		score += increase;
	}

	public void loseLife() {
		lives--;
	}

	private bool isGameOver() {
        if (time >= maxTime || lives <= 0) {
            return true;
        } else {
            return false;
        }
	}
    private void GameOver() {
        enabled = false;
        SceneManager.LoadScene("End");
    }
}
