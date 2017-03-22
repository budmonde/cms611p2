using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour {
	public void clickStartGame(){
		SceneManager.LoadScene ("Main");
	}

	public void clickPlayAgain(){
		SceneManager.LoadScene ("Start");
	}

	public void clickInstructions(){
		SceneManager.LoadScene ("Instructions");
	}

	public void clickEasyGame(){
		Debug.Log ("easy game");
	}

	public void clickMediumGame(){
		Debug.Log ("medium game");
	}

	public void clickHardGame(){
		Debug.Log ("hard game");
	}
}
