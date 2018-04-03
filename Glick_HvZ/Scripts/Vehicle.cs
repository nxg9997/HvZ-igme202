using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour {

	public Vector3 position;
	public Vector3 direction;
	public Vector3 velocity;
	public Vector3 acceleration;

	public float mass;
	public float maxSpeed;
	public float maxForce;
	public float radius;

	public float maxAvoidDistance;

	public Material fVectorColor;
	public Material rVectorColor;
	public Material futurePosVectorColor;

	public bool debugEnabled = false;

	public GameObject terrain;

	// Use this for initialization
	protected virtual void Start () {
		//float height = terrain.GetComponent<Terrain> ().SampleHeight (transform.position);
		transform.position = new Vector3 (transform.position.x, -.09f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		CalcSteeringForces ();
		UpdatePosition ();
		SetTrasform ();

		float terrainheight;
	}

	protected Vector3 Seek (Vector3 targetPos) {
		Vector3 desiredVelocity = targetPos - position;
		desiredVelocity.Normalize ();
		desiredVelocity = desiredVelocity * maxSpeed;

		return (desiredVelocity - velocity);
	}

	protected Vector3 Flee (Vector3 targetPos) {
		Vector3 desiredVelocity = position - targetPos;
		desiredVelocity.Normalize ();
		desiredVelocity = desiredVelocity * maxSpeed;

		return (desiredVelocity - velocity);
	}

	protected Vector3 Pursue (Vector3 targetPos){
		return Seek (targetPos * 2f);
	}

	protected Vector3 Evade (Vector3 targetPos){
		return Flee (targetPos * 2f);
	}

	abstract protected void CalcSteeringForces();

	void UpdatePosition () {
		position = gameObject.transform.position;
		velocity += acceleration;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		position += velocity;
		direction = velocity.normalized;
		acceleration = Vector3.zero;
	}

	void SetTrasform () {
		gameObject.transform.forward = direction;
		position = new Vector3 (position.x, 0.1f, position.z);
		transform.position = position;
	}

	void ApplyForce (Vector3 force) {}

	#region Bad Avoid Methods
	protected Vector3 Avoid(){
		GameObject[] gArr = GameObject.FindGameObjectsWithTag ("avoid");
		//go thru each game object that has to be avoided
		for (int i = 0; i < gArr.Length; i++) {
			//check if the object is in front of the vehicle
			if (Vector3.Dot ( gArr [i].transform.position, gameObject.transform.forward) > 0) {
				//Debug.Log ("in front of me");
				//check if the vehicle will eventually collide with the object
				float dot = Vector3.Dot (gameObject.transform.right, gArr [i].transform.position);
				if (AABBCollision(GetRect(), gArr[i])){
					//Debug.Log ("i gotta avoid");
					//check which direction the vehicle should turn
					if (dot < 0){
						Debug.Log ("turn right");
						return (transform.right) - velocity;
					}
					else{
						Debug.Log ("turn left");
						return (transform.right * -1f) - velocity;
					}
				}
			}
		}


		return Vector3.zero;
	}

	protected Vector3 Avoid(int any){
		Vector3 ahead = transform.position + transform.forward * maxAvoidDistance;
		Vector3 force = Vector3.zero;

		GameObject[] gArr = GameObject.FindGameObjectsWithTag ("avoid");
		GameObject threat = null;
		float threatDistance = 1000;
		for (int i = 0; i < gArr.Length; i++) {
			if (CircleCollision(gameObject, gArr[i], transform.forward, maxAvoidDistance)/*(ahead - gArr [i].transform.position).sqrMagnitude < Mathf.Pow (gArr [i].GetComponent<Radius> ().radius, 2)*/) {
				float dot = Vector3.Dot (gArr [i].transform.position, gameObject.transform.forward);
				Debug.Log ("threat!");
				if (threat == null) {
					threat = gArr [i];
				}
				else if (threatDistance > dot) {
					threat = gArr [i];
					threatDistance = dot;
				}
			}
		}
		if (threat != null) {
			force.x = ahead.x - threat.transform.position.x;
			force.z = ahead.z - threat.transform.position.z;

			force.Normalize ();
			force *= maxForce;

			Debug.Log ("X: " + force.x + ", Y: " + force.y + ", Z: " + force.z);
		}

		return force - velocity;
	}
	#endregion

	//

	protected Vector3 JAvoid(){
		GameObject[] gArr = GameObject.FindGameObjectsWithTag ("avoid");
		List<GameObject> currentObstacles = new List<GameObject>();

		for (int i = 0; i < gArr.Length; i++)
		{
			if(LocationCheck(gArr[i].transform.position, gArr[i].transform.forward.sqrMagnitude))
			{
				currentObstacles.Add(gArr[i]);
			}
		}

		foreach(GameObject obj in currentObstacles)
		{
			Vector3 distToObstacle = obj.transform.position - transform.position;
			float dot = Vector3.Dot(transform.right, distToObstacle);

			if (dot >= 0)
			{
				return (-1 * transform.right) * maxSpeed;
			}
			else if(dot < 0)
			{
				return transform.right * maxSpeed;
			}
		}

		return Vector3.zero;
	}

	private bool LocationCheck(Vector3 obsPosition, float obsRadius)
	{
		Vector3 futureLocation = transform.forward * maxAvoidDistance;

		Vector3 distToObstacle = obsPosition - transform.position;

		if(distToObstacle.sqrMagnitude < futureLocation.sqrMagnitude)
		{
			if(Vector3.Dot(distToObstacle, transform.right) < obsRadius + radius)
			{
				if(Vector3.Dot(distToObstacle, transform.forward) > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	//

    public Vector3 Wander ()
    {
		Vector3 cCenter = (transform.position + transform.forward.normalized);

		float rAng = Random.Range (0f, 360f);
		//rAng = Mathf.PerlinNoise (cCenter.x, Time.deltaTime);

		Vector3 target = new Vector3 (0, 0, 1f);
		target = Quaternion.AngleAxis (rAng, Vector3.up) * target;
		Debug.Log (target);
		target.Normalize ();
		Vector3 ctarget = cCenter + target;
		ctarget.y = 0;
		//Debug.Log (ctarget);

		return Seek(ctarget);
    }

	public Vector3 NoOOB (){
		if (transform.position.z >= 5f || transform.position.z <= -5f || transform.position.x >= 5f || transform.position.x <= -5f){
			return Seek (Vector3.zero);
		}
		return Vector3.zero;
	}

	public Vector3 Separation (GameObject obj){
		return Flee (obj.transform.position);
	}

	bool AABBCollision(GameObject a, GameObject b){
		return false;
	}

	bool AABBCollision(Rect a, Rect b){

		if (b.xMin < a.xMax && a.xMin < b.xMax && b.yMin < a.yMax && a.yMin < b.yMax) {
			return true;
		}

		return false;
	}

	bool AABBCollision(Rect a, GameObject b){
		Rect bRect = new Rect(new Vector2(b.transform.position.x - b.GetComponent<Radius>().radius, b.transform.position.z - b.GetComponent<Radius>().radius), new Vector2(b.GetComponent<Radius>().radius * 2f, b.GetComponent<Radius>().radius * 2f));

		return AABBCollision (a, bRect);
	}

	Rect GetRect(){
		return new Rect (new Vector2 (transform.position.x - GetComponent<Radius> ().radius, transform.position.z - GetComponent<Radius> ().radius), new Vector2 (GetComponent<Radius> ().radius * 2f, maxAvoidDistance + GetComponent<Radius> ().radius));
	}

	public bool CircleCollision(GameObject a, GameObject b){
		Vector3 centerA = a.transform.position;
		float radiusA = a.GetComponent<Radius>().radius;

		Vector3 centerB = b.transform.position;
		float radiusB = b.GetComponent<Radius>().radius;

		Vector3 distance = centerA - centerB;

		if (distance.sqrMagnitude < (Mathf.Pow (radiusA, 2) + Mathf.Pow (radiusB, 2))) {
			return true;
		}

		return false;
	}

	bool CircleCollision(GameObject a, GameObject b, Vector3 forward, float extend){
		Vector3 centerA = a.transform.position + (forward * extend);
		float radiusA = a.GetComponent<Radius>().radius;

		Vector3 centerB = b.transform.position;
		float radiusB = b.GetComponent<Radius>().radius;

		Vector3 distance = centerA - centerB;

		if (distance.sqrMagnitude < (Mathf.Pow (radiusA, 2) + Mathf.Pow (radiusB, 2))) {
			return true;
		}

		return false;
	}
}
