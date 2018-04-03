using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : Vehicle {

	public float seekingWeight;
	GameObject target;

	public GameObject manager;

	public Material tVectorColor;

	public Vector3 humanTargetPos = new Vector3(0,0,0);

	// Use this for initialization
	protected override void Start () {
		manager = GameObject.Find ("Manager");
		base.Start ();
	}


	protected override void CalcSteeringForces () {
		Vector3 ultimate = Vector3.zero;
		Vector3 closestHumanPos = new Vector3(1000, 1000, 1000);
		GameObject[] hArr = GameObject.FindGameObjectsWithTag ("human");

		if (hArr.Length > 0){
			for (int i = 0; i < hArr.Length; i++) {
				if (Mathf.Abs((transform.position - hArr[i].transform.position).magnitude) < Mathf.Abs((transform.position - closestHumanPos).magnitude)){
					closestHumanPos = hArr[i].transform.position;
					humanTargetPos = hArr[i].transform.position;
				}
			}
			if (Mathf.Abs ((transform.position - closestHumanPos).sqrMagnitude) > 4f) {
				ultimate += base.Pursue (closestHumanPos);
			}
			else {
				ultimate += Seek (closestHumanPos);
			}
		}
		else {
			ultimate += Wander ();
		}

		GameObject[] zArr = GameObject.FindGameObjectsWithTag ("zombie");
		for (int i = 0; i < zArr.Length; i++){
			if (Mathf.Abs((transform.position - zArr[i].transform.position).sqrMagnitude) < 0.1f){
				ultimate += Separation (zArr[i]);
			}
		}

		ultimate += JAvoid ();
		ultimate = Vector3.ClampMagnitude (ultimate, maxForce);
		ultimate += NoOOB ();
		acceleration += ultimate;
		acceleration.y = 0;
	}

	void OnRenderObject (){
		if (manager.GetComponent<ExerciseManager>().debugEnabled){
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

			tVectorColor.SetPass (0);
			GL.Begin (GL.LINES);
			GL.Vertex (transform.position);
			GL.Vertex (humanTargetPos);
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
