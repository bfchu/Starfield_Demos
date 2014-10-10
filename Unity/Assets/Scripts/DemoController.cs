using UnityEngine;
using System.Collections;

public class DemoController : MonoBehaviour {
	
	public GameObject stars;
	public int numStars;
	public Vector3 spawnValues;
	public bool spawnerEnabled = true;
	public float starSpawnInterval = 0.2f;
	public int starSpawnBatchSize = 5;
	public GUIText StarCounterDisplay;
	
	private int currentStars;
	private Quaternion starsRotation = new Quaternion (90, 0, 0, 90);
	private float starSpawnTimer = 0.0f;

	//star remover
	public GameObject starDestroyer; //the off-screen endzone where we pick stars to remove.
	private CollisionTracker starDestroyerScript;

	//frame rate counter 
	public GUIText frameRateDisplay;
	private const float FRAME_RATE_TARGET = 0.017f; //in milliseconds
	public float filterStrength = 20;
	private float fpsDisplayInterval = 0.5f;
	private float fpsTimeTracker = 0;
	private float frameTime = 0;
	private float currentFPS = 0;
	private float lastDeltaTime = 0;
	

	void Start () {
		spawnInitialStars ();
		starDestroyerScript = starDestroyer.GetComponent ("CollisionTracker") as CollisionTracker;
	}

	void spawnInitialStars(){
		//spawn a collection of stars at random locations.
		for (int ii = 0; ii < numStars; ++ii) {
			addStar(Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.z, spawnValues.z));
		}
	}

	void addStar(float xPos, float yPos){
		Vector3 spawnPosition = new Vector3 (xPos, 0.0f, yPos);
		Instantiate (stars, spawnPosition, starsRotation );
		currentStars++;
	}

	void removeStar(){
		//find a star that is off-screen and remove it.
		if (starDestroyerScript.currentTarget != null) {
			Destroy (starDestroyerScript.currentTarget.gameObject);
			currentStars--;
		}
	}


	// @citation: Frame counting code inspired from http://stackoverflow.com/questions/4787431/check-fps-in-js
	void frameTrackerUpdate(){
		frameTime += (Time.deltaTime - frameTime) / filterStrength;
		fpsTimeTracker += Time.deltaTime;
		if (fpsTimeTracker >= fpsDisplayInterval) {
			currentFPS = 1 / frameTime;
			fpsTimeTracker = 0;
			lastDeltaTime = Time.deltaTime;
		}
		frameRateDisplay.text = currentFPS + " fps";
	//	frameRateDisplay.text = Mathf.Round(currentFPS) + " fps";
	}


	void Update () {

		//Star spawner
		if(spawnerEnabled){
			starSpawnTimer += Time.deltaTime;
			if (starSpawnTimer > starSpawnInterval) {
				if(Time.deltaTime <= FRAME_RATE_TARGET){
					//add stars in batch
					starSpawnTimer = 0;
					for(int kk = 0; kk < starSpawnBatchSize; ++kk){
						addStar(-spawnValues.x, Random.Range (-spawnValues.z, spawnValues.z));
					}
				} else {
					removeStar();
					starSpawnTimer = 0;
				}
			}

		}
		StarCounterDisplay.text = "Stars: " + currentStars;
		frameTrackerUpdate ();
	}

}
