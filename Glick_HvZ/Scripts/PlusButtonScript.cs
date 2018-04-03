using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusButtonScript : MonoBehaviour {

	public GameObject spawnObj;
	public GameObject ground;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TaskOnClick (){
		float rX = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.x / 2, ground.GetComponent<MeshCollider>().bounds.size.x / 2);
		float rZ = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.z / 2, ground.GetComponent<MeshCollider>().bounds.size.z / 2);
		GameObject newObj = Instantiate (spawnObj, new Vector3 (rX, spawnObj.transform.position.y, rZ), Quaternion.identity);
	}
}
