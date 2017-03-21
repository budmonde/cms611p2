using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public float maxTime = 300f;
    public int lives = 5;
    public float time;
    public int doggos;

    private Text lifeText;
    private Text levelText;
    private GameObject levelImage;
    public float levelStartDelay = 2f;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }
	
    void InitGame() {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeText.text = "Lives: " + lives;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Start Game";
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        time = 0;
        boardScript.SetupScene(doggos);
    }

    void HideLevelImage() {
        levelImage.SetActive(false);
    }

    void Update() {
        time += Time.deltaTime;
        if (isGameOver()) {
            Invoke("GameOver", levelStartDelay);
        }
    }

	public void incrementUnhappyPetCounter() {
		lives--;
        lifeText.text = "Lives: " + lives;
	}

	private bool isGameOver() {
        if (time >= maxTime || lives <= 0) {
            return true;
        } else {
            return false;
        }
	}
    private void GameOver() {
        levelText.text = "Game Over";
        levelImage.SetActive(true);
        enabled = false;
    }
}
