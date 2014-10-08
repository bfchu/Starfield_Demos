using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;

	public float scaleMin;
	public float scaleMax;
	
	void Start () {
		//spawn this star with a random velocity, 
		float speed = Random.Range (minSpeed, maxSpeed);
		rigidbody.velocity = transform.right * speed;
		//scale its size by its ratio to maxSpeed.
		float distance = rigidbody.velocity.x / maxSpeed;
		float scale = Mathf.Max (scaleMin, scaleMax * distance);
		transform.localScale *= scale;


	}

}
