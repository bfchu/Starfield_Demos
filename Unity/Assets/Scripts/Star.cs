using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;

	public float scaleMin;
	public float scaleMax;

	public int startLine;
	public int finishLine;

	private float speed;
	private int speedAdjust = 60;
	
	void Start () {
		//spawn this star with a random velocity, 
		speed = Random.Range (minSpeed, maxSpeed)/speedAdjust;
		//scale its size by its ratio to maxSpeed.
		float distance = speed * speedAdjust / maxSpeed;
		float scale = Mathf.Max (scaleMin, scaleMax * distance);
		transform.localScale *= scale;
	}


	void moveStar(){
		transform.position = new Vector3(transform.position.x + speed, 0, transform.position.z);
	}

	void resetPosition(){
		transform.position = new Vector3 (startLine, 0, transform.position.z);
	}


	void Update(){
		moveStar ();

		if (transform.position.x > finishLine) {
			resetPosition();
		}
	}

}
