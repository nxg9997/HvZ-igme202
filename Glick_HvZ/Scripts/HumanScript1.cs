using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanScript1 : Vehicle {

	public float seekingWeight;
	public float fleeingWeight;

	public GameObject seekTarget;
	public GameObject zombie;

	public bool canCollide = true;

	public bool allowDebug = true;

	GameObject manager;

	// Use this for initialization
	protected override void Start () {
		seekTarget = GameObject.Find ("target");
		manager = GameObject.Find ("Manager");
		//fleeTarget = GameObject.Find ("zombie(Clone)");
		base.Start ();
	}


	protected override void CalcSteeringForces () {
		Vector3 ultimate = Vector3.zero;
		//ultimate += base.Seek (seekTarget.transform.position) * seekingWeight;
		GameObject[] zArr = GameObject.FindGameObjectsWithTag("zombie");
		for (int i = 0; i < zArr.Length; i++) {
			if (Mathf.Abs ((transform.position - zArr[i].transform.position).magnitude) < radius) {
				ultimate += base.Evade (zArr[i].transform.position);
			}
		}
		GameObject[] hArr = GameObject.FindGameObjectsWithTag ("human");
		for (int i = 0; i < hArr.Length; i++){
			if (Mathf.Abs((transform.position - hArr[i].transform.position).sqrMagnitude) < 0.1f){
				ultimate += Separation (hArr[i]);
			}
		}
		ultimate += Wander ();
		ultimate += JAvoid ();
		ultimate = Vector3.ClampMagnitude (ultimate, maxForce);
		ultimate += NoOOB ();
		acceleration += ultimate;
		acceleration.y = 0;

		if (canCollide){
			for (int i = 0; i < zArr.Length; i++) {
				if (CircleCollision(gameObject, zArr[i])) {
					gameObject.tag = "invisHuman";
					canCollide = false;
					allowDebug = false;
					GameObject newZed = Instantiate (zombie, transform.position, transform.rotation);
					newZed.GetComponent<ZombieScript> ().acceleration = acceleration;
					SkinnedMeshRenderer[] smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
					foreach (SkinnedMeshRenderer s in smr){
						s.enabled = false;
					}
					break;
				}
			}
		}

	}

	void OnRenderObject (){
		if (manager.GetComponent<ExerciseManager>().debugEnabled && allowDebug){
			fVectorColor.SetPass (0);
			GL.Begin (GL.LINES);
			//GL.Color (Color.green);
			GL.Vertex (transform.position);
			GL.Vertex (transform.position + transform.forward);
			GL.End ();

			rVectorColor.SetPass (0);
			GL.Begin (GL.LINES);
			//GL.Color (Color.blue);
			GL.Vertex (transform.position);
			GL.Vertex (transform.position + transform.right);
			GL.End ();

			futurePosVectorColor.SetPass (0);
			GL.Begin (GL.LINES);
			Vector3 futurePos = transform.position + (transform.forward * 2f);
			for (float theta = 0.0f; theta < (Mathf.PI * 2); theta += 0.01f) {
				Vector3 point = new Vector3 (Mathf.Cos (theta) * 0.1f + futurePos.x, futurePos.y, Mathf.Sin (theta) * 0.1f + futurePos.z);
				GL.Vertex (point);
			}
			GL.End ();
		}
	}
}
