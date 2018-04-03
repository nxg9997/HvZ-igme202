using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour {

	public GameObject manager;

	public Button[] plusArr = new Button[3];

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TaskOnClick(){
		Debug.Log ("start!");
		manager.GetComponent<ExerciseManager> ().canBegin = true;
		GetComponent<Button> ().interactable = false;
		for (int i = 0; i < plusArr.Length; i++){
			plusArr [i].interactable = true;
		}
	}
}
