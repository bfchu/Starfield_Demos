using UnityEngine;
using System.Collections;

public class ShoeTrailer : MonoBehaviour {

	public float trailSpeed = 5;
	public float lifeTime;

	private float tick;
	
	void Start () {
		rigidbody.velocity = new Vector3 (trailSpeed, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		tick += Time.deltaTime;
		if (tick > lifeTime){
			Destroy (gameObject);
		}

	}
}
