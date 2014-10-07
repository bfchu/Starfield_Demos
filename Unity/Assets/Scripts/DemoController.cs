using UnityEngine;
using System.Collections;

public class DemoController : MonoBehaviour {
	
	public GameObject stars;
	public int numStars;
	public Vector3 spawnValues;
//	public float minSpeed;
//	public float maxSpeed;
	public bool spawnerEnabled = true;
	public float starSpawnInterval = 0.2f;
	public int starSpawnBatchSize = 5;

	private Quaternion starsRotation = new Quaternion (90, 0, 0, 90);
	private float starSpawnTimer = 0.0f;

	//frame rate counter 
	public GUIText frameRateDisplay;
	private const int FRAME_RATE_TARGET = 60;
	public float filterStrength = 20;
	private float fpsDisplayInterval = 0.5f;
	private float fpsTimeTracker = 0;
	private float frameTime = 0;
	private float currentFPS = 0;
	

	void Start () {
		spawnInitialStars ();
	}

	void spawnInitialStars(){
		//spawn a collection of stars at random locations with random speeds, scale their size and brightness with their ratio to maxSpeed.
		for (int ii = 0; ii < numStars; ++ii) {
			addStar(Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.z, spawnValues.z));
		}
	}

	void addStar(float xPos, float yPos){
		Vector3 spawnPosition = new Vector3 (xPos, 0.0f, yPos);
		Instantiate (stars, spawnPosition, starsRotation );
	}

	// @citation: Frame counting code inspired from http://stackoverflow.com/questions/4787431/check-fps-in-js
	void frameTrackerUpdate(){
		frameTime += (Time.deltaTime - frameTime) / filterStrength;
		fpsTimeTracker += Time.deltaTime;
		if (fpsTimeTracker >= fpsDisplayInterval) {
			currentFPS = 1 / frameTime;
			fpsTimeTracker = 0;
		}

		frameRateDisplay.text = Mathf.Round(currentFPS) + " fps";
	}


	void Update () {

		//Star spawner
		if(spawnerEnabled){
			starSpawnTimer += Time.deltaTime;
			if (starSpawnTimer > starSpawnInterval) {
				starSpawnTimer = 0;
				for(int kk = 0; kk < starSpawnBatchSize; ++kk){
					addStar(-spawnValues.x, Random.Range (-spawnValues.z, spawnValues.z));
				}
			}
		}

		frameTrackerUpdate ();
	}

}
