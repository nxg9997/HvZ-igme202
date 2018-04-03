using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetButtonScript : MonoBehaviour {

	public Button startButton;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TaskOnClick(){
		Debug.Log ("reset!");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
