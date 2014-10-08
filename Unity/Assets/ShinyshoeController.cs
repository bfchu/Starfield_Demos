using UnityEngine;
using System.Collections;

public class ShinyshoeController : MonoBehaviour {

	public float swingPower = 10; //affects the ammount of rotation applied.
	public float swingSpeed = 1;
	public float amplitude = 1;
	public float musicChange1;
	public float musicChange2;
	private float musicEnd = 65.88;
	// Use this for initialization
	void Start () {
		
	}

	void swingStep(){
		//rotate the shiny shoe back and forth using the sine function and swingPower;
		transform.rotation = new Quaternion (90, 0, Mathf.Sin (Time.time * swingSpeed)*swingPower, 90);
		if (Time.time >= musicChange1) {
			//begin the kicking.
			transform.position = new Vector3(0.0f,1.0f, Mathf.Sin(Time.time - musicChange1) * amplitude);
		}
	}

	// Update is called once per frame
	void Update () {
		swingStep ();
	}
}
