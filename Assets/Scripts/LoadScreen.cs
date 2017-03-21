using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {
	public void clickStartGame(){
		Application.LoadLevel ("Difficulty");
	}

	public void clickPlayAgain(){
		Application.LoadLevel ("Start");
	}

	public void clickInstructions(){
		Application.LoadLevel ("Instructions");
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
