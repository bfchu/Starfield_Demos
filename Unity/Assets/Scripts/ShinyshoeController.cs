using UnityEngine;
using System.Collections;

public class ShinyshoeController : MonoBehaviour {

	public float swingPower = 10; //affects the ammount of rotation applied.
	public float swingSpeed = 1;
	public float amplitude = 1;
	public float musicChange1;
	public float musicChange2;

	public GameObject shinyTrail1;
	private float trail1Offset = 0.6f;
	public GameObject shinyTrail2;
	private float trail2Offset = 0;
	public GameObject shinyTrail3;
	private float trail3Offset = -0.6f;

	private float musicEnd = 65f;
	private float creditsDelay = 2.0f;
	private float escapeSpeed = 0.1f;
	private Quaternion trailsRotations = new Quaternion(90,0,0,90);
	private float shinyTime = 0;

	public GUIText endText1;
	public GUIText endText2;
	public GUIText endPrompt;
	

	void Start () {

		endText1.text = "";
		endText2.text = "";
		endPrompt.text = "";
	}

	void swingStep(){
		//rotate the shiny shoe back and forth using the sine function and swingPower;
		transform.rotation = new Quaternion (90, 0, Mathf.Sin (shinyTime * swingSpeed)*swingPower, 90);
		if (shinyTime >= musicChange1) {
			//begin the kicking.
			transform.position = new Vector3(0.0f,1.0f, Mathf.Sin(shinyTime - musicChange1) * amplitude);
			spawnShinyTrails ();
		}


	}

	void spawnShinyTrails(){
		Vector3 spawnPosition = new Vector3 (transform.position.x + 1, 0.0f, transform.position.z + trail1Offset);
		Instantiate(shinyTrail1,spawnPosition,trailsRotations);
		Vector3 spawnPosition2 = new Vector3 (transform.position.x + 1, 0.0f, transform.position.z + trail2Offset);
		Instantiate(shinyTrail2,spawnPosition2,trailsRotations);
		Vector3 spawnPosition3 = new Vector3 (transform.position.x + 1, 0.0f, transform.position.z + trail3Offset);
		Instantiate(shinyTrail3,spawnPosition3,trailsRotations);
	}

	void escapeSequence(){
		//The ship slides back towards the center and takes off.
		transform.position = new Vector3 (transform.position.x - escapeSpeed, 1.0f, transform.position.z * 0.98f);
		if (shinyTime > musicEnd + creditsDelay) {
			endText1.text = "Shiny Shoe Starfield";
			if(shinyTime > musicEnd + creditsDelay + 1){
				endText2.text = "by Brian Chu";
				if(shinyTime > musicEnd + creditsDelay*2){
					endPrompt.text = "Press 'R' to replay, press 'Space Bar' to start benchmarking demo.";
					if(Input.GetKeyDown(KeyCode.R))
						Application.LoadLevel(Application.loadedLevel);
					if(Input.GetKeyDown(KeyCode.Space))
						Application.LoadLevel("Prefabs_benchmarking");
				}
			}
		}

	}
	
	void Update () {
		shinyTime += Time.deltaTime;
		if (shinyTime < musicEnd) {
			swingStep ();
		} else {
			escapeSequence();
		}
	}
}
