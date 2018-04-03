using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSelectScript : MonoBehaviour {

	public Camera[] cameras = new Camera[2];
	Dropdown selector;

	// Use this for initialization
	void Start () {
		selector = GetComponent<Dropdown> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (selector.value == 0){
			cameras [0].gameObject.SetActive(true);
			cameras [1].gameObject.SetActive(false);
		}
		else if (selector.value == 1){
			cameras [0].gameObject.SetActive(false);
			cameras [1].gameObject.SetActive(true);
		}
	}
}
