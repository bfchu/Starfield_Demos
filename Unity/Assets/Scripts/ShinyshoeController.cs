using UnityEngine;
using System.Collections;

public class ShinyshoeController : MonoBehaviour {

	public float swingPower = 10; //affects the ammount of rotation applied.
	public float swingSpeed = 1;
	public float amplitude = 1;
	public float musicChange1; //time stamp of music changes.
	public float musicChange2;

	public GameObject backdrop;
	private Color shinyColor = new Color(219,88,0,50);
	private Color BLACK = new Color(0,0,0,255);
	private Color TRANSPARENCY = new Color (0, 0, 0, 0);
	private float sweepClamp = 7;
	private float sweepTime = 0;
	private float sweepSpeed = 1.5f;

	public GameObject shinyTrail1;
	public GameObject shinyTrail2;
	public GameObject shinyTrail3;
	private float trail1Offset = 0.6f;
	private float trail2Offset = 0;
	private float trail3Offset = -0.6f;

	private float musicEnd = 65f;
	private float creditsDelay = 2.0f;
	private float escapeSpeed = 0.1f;
	private Quaternion trailsRotations = new Quaternion(90,0,0,90);
	private float shinyTime = 0;

	public GUIText endText1;
	public GUIText endText2;
	public GUIText endText3;
	public GUIText endPrompt;
	

	void Start () {
		backdrop.renderer.material.color = TRANSPARENCY;
		endText1.text = "";
		endText2.text = "";
		endText3.text = "";
		endPrompt.text = "";
	}

	void swingStep(){
		//rotate the shiny shoe back and forth using the sine function and swingPower;
		transform.rotation = new Quaternion (90, 0, Mathf.Sin (shinyTime * swingSpeed)*swingPower, 90);
		if (shinyTime >= musicChange1) {
			//begin the kicking.
			transform.position = new Vector3(0.0f,1.0f, Mathf.Sin(shinyTime - musicChange1) * amplitude);
			spawnShinyTrails ();
			if(shinyTime >= musicChange2){
				sweepTime += Time.deltaTime;
				float lerp = Mathf.PingPong(sweepTime*sweepSpeed, 1)/sweepClamp;
				backdrop.renderer.material.color = new Color(lerp *shinyColor.r,lerp * shinyColor.g, 0.0f,lerp);
			}
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
		backdrop.renderer.material.color = Color.black;

		//After a bit, start displaying the credits.
		if (shinyTime > musicEnd + creditsDelay) {
			endText1.text = "Shiny Shoe Starfield";
			if(shinyTime > musicEnd + creditsDelay + 1){
				endText2.text = "by Brian Chu";
				endText3.text = "Music: Airtime by Gus Wellman";
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
