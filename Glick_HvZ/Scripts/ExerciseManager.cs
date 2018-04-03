using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseManager : MonoBehaviour {

	public GameObject ground;

	public GameObject human;
	public GameObject zombie;
	public GameObject target;
	public GameObject obstacle;

	public int humanAmount;
	public int obstacleAmount;
	public int zedAmount;
	public float radius;

	public bool canSpawnObstacles;

	public GameObject[] humanArr;

	public bool debugEnabled = false;
	public bool canBegin = false;
	public bool canPlay = false;

	public Slider zSlider;
	public Slider hSlider;
	public Slider tSlider;

	public Text zCountText;
	public Text hCountText;
	public Text tCountText;

	public GameObject followCam;

	public Toggle debugToggle;

	// Use this for initialization
	void Start () {
		
		target.GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (canBegin){Begin ();}
		if (canPlay) {
			HvS ();
			UpdateCounts ();
			/*if (Input.GetKeyDown (KeyCode.F1)) {
				debugEnabled = !debugEnabled;
			}*/
			debugEnabled = CheckToggle ();
		}
	}

	void Begin () {
		canBegin = false;
		GetSliderValues ();
		humanArr = new GameObject[humanAmount];
		Spawn ();
		if (canSpawnObstacles) {
			SpawnObstacles ();
		}
		canPlay = true;
	}

	void GetSliderValues () {
		zedAmount = (int)(Mathf.Round(100f * zSlider.value) / 2);
		humanAmount = (int)(Mathf.Round(100f * hSlider.value) / 2);
		obstacleAmount = (int)(Mathf.Round(100f * tSlider.value) / 2);
	}

	void UpdateCounts () {
		zCountText.text = "Zombie Count: " + GameObject.FindGameObjectsWithTag ("zombie").Length;
		hCountText.text = "Human Count: " + GameObject.FindGameObjectsWithTag ("human").Length;
		tCountText.text = "Tree Count: " + GameObject.FindGameObjectsWithTag ("avoid").Length;
	}

	bool CheckToggle (){
		if (debugToggle.isOn){return true;}
		return false;
	}

	void Spawn (){
		for (int i = 0; i < humanAmount; i++) {
			float rX = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.x / 2, ground.GetComponent<MeshCollider>().bounds.size.x / 2);
			float rZ = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.z / 2, ground.GetComponent<MeshCollider>().bounds.size.z / 2);
			GameObject newHuman = Instantiate (human, new Vector3 (rX, human.transform.position.y, rZ), Quaternion.identity);
			humanArr [i] = newHuman;
		}

		for (int i = 0; i < zedAmount; i++) {
			float ZRX = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.x / 2, ground.GetComponent<MeshCollider> ().bounds.size.x / 2);
			float ZRZ = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.z / 2, ground.GetComponent<MeshCollider> ().bounds.size.z / 2);
			GameObject newZombie = Instantiate (zombie, new Vector3 (ZRX, human.transform.position.y, ZRZ), Quaternion.identity);
			followCam.GetComponent<SmoothFollow> ().target = newZombie.transform;
		}

		if (zedAmount == 0){
			float ZRX = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.x / 2, ground.GetComponent<MeshCollider> ().bounds.size.x / 2);
			float ZRZ = Random.Range (-ground.GetComponent<MeshCollider> ().bounds.size.z / 2, ground.GetComponent<MeshCollider> ().bounds.size.z / 2);
			GameObject newZombie = Instantiate (zombie, new Vector3 (ZRX, human.transform.position.y, ZRZ), Quaternion.identity);
			followCam.GetComponent<SmoothFollow> ().target = newZombie.transform;
		}
	}

	void HvS (){
		foreach (GameObject h in humanArr) {
			if (Mathf.Abs ((h.transform.position - target.transform.position).magnitude) < radius) {
				Debug.Log ("Teleport!");
				/*float rX = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.x / 2, ground.GetComponent<MeshCollider>().bounds.size.x / 2);
				float rZ = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.z / 2, ground.GetComponent<MeshCollider>().bounds.size.z / 2);
				target.transform.position = new Vector3 (rX, target.transform.position.y, rZ);*/
				target.GetComponent<TeleportPink> ().canWarp = true;
			}
		}
	}

	void SpawnObstacles(){
		for (int i = 0; i < obstacleAmount; i++) {
			float rX = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.x / 2, ground.GetComponent<MeshCollider>().bounds.size.x / 2);
			float rZ = Random.Range(-ground.GetComponent<MeshCollider>().bounds.size.z / 2, ground.GetComponent<MeshCollider>().bounds.size.z / 2);
			GameObject newObstacle = Instantiate (obstacle, new Vector3 (rX, human.transform.position.y, rZ), Quaternion.identity);
		}
	}
}
