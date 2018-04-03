using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPink : MonoBehaviour {

	public bool canWarp = false;
	public GameObject manager;
	public GameObject ground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canWarp) {
			float rX = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.x / 2, ground.GetComponent<MeshCollider> ().bounds.size.x / 2);
			float rZ = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.z / 2, ground.GetComponent<MeshCollider> ().bounds.size.z / 2);
			transform.position = new Vector3 (rX, 0.1f, rZ);
			canWarp = false;
		}
	}
}

