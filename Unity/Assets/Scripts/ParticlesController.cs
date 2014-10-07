using UnityEngine;
using System.Collections;

public class ParticlesController : MonoBehaviour {
	
	private ParticleSystem.Particle[] stars;

	public int numStars;
	public Vector3 spawnValues;
	public float minSpeed;
	public float maxSpeed;
	public float scaleMin;
	public float scaleMax;

	public bool spawnerEnabled = true;
	public float starSpawnInterval = 0.2f;
	public int starSpawnBatchSize = 5;
	public GUIText StarCounterDisplay;


	private Quaternion starsRotation = new Quaternion (90, 0, 0, 90);
	private float starSpawnTimer = 0.0f;

	public GameObject starDestroyer; //the off-screen endzone where we pick stars to remove.
	private CollisionTracker starDestroyerScript;

	//frame rate counter 
	public GUIText frameRateDisplay;
	private const int FRAME_RATE_TARGET = 60;
	public float filterStrength = 20;
	private float fpsDisplayInterval = 0.5f;
	private float fpsTimeTracker = 0;
	private float frameTime = 0;
	private float currentFPS = 0;
	private float lastFPS = 0;
	

	void Start () {
		spawnInitialStars ();
		starDestroyerScript = starDestroyer.GetComponent ("CollisionTracker") as CollisionTracker;
	}

	void spawnInitialStars(){
		//spawn a collection of stars at random locations with random speeds, scale their size and brightness with their ratio to maxSpeed.
		stars = new ParticleSystem.Particle[numStars];
		for (int ii = 0; ii < numStars; ++ii) {
			//give them a random position;
			stars[ii].position = new Vector3(Random.Range (-spawnValues.x, spawnValues.x),0.0f, Random.Range (-spawnValues.z, spawnValues.z));
			//spawn this star with a random velocity, 
			float speed = Random.Range (minSpeed, maxSpeed);
			stars[ii].velocity = transform.right * speed;
			//scale its size and color by its ratio to maxSpeed.
			float distance = stars[ii].velocity.x / maxSpeed;
			float scale = Mathf.Max (scaleMin, scaleMax * distance);
			stars[ii].color = new Color(1,1,1,1);
			stars[ii].size = scale;
			//addStar(Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.z, spawnValues.z));
		}
	}

	void addStar(float xPos, float yPos){
		Vector3 spawnPosition = new Vector3 (xPos, 0.0f, yPos);
		//stars [stars.Length].position = spawnPosition;

	}

	void removeStar(){
		//find a star that is off-screen and remove it.
		if (starDestroyerScript.currentTarget != null) {
			//TODO: remove star;
			//Destroy (starDestroyerScript.currentTarget.gameObject);
			//Debug.Log ("kabooom!");
		}
	}

	// @citation: Frame counting code inspired from http://stackoverflow.com/questions/4787431/check-fps-in-js
	void frameTrackerUpdate(){
		frameTime += (Time.deltaTime - frameTime) / filterStrength;
		fpsTimeTracker += Time.deltaTime;
		if (fpsTimeTracker >= fpsDisplayInterval) {
			currentFPS = 1 / frameTime;
			fpsTimeTracker = 0;
			lastFPS = currentFPS;
		}
		frameRateDisplay.text = currentFPS + " fps";
	//	frameRateDisplay.text = Mathf.Round(currentFPS) + " fps";
	}


	void Update () {

		//Star spawner
		if(spawnerEnabled){
			starSpawnTimer += Time.deltaTime;
			if (starSpawnTimer > starSpawnInterval) {

				if( currentFPS >= FRAME_RATE_TARGET){ //frame rate is above or at target.
					if(!(lastFPS > currentFPS)){ //frame rate is either stable or improving, and above target
						//add stars in batch
						starSpawnTimer = 0;
						for(int kk = 0; kk < starSpawnBatchSize; ++kk){
							addStar(-spawnValues.x, Random.Range (-spawnValues.z, spawnValues.z));
						}
					} else { //framerate is declining, but above target
						//add just one star this time.
						addStar(-spawnValues.x, Random.Range (-spawnValues.z, spawnValues.z));
						starSpawnTimer = 0;
					}
				} else { //frame rate is under target.
					//removeStar();
				}
			}

		}
		StarCounterDisplay.text = "Stars: " + stars.Length;
		frameTrackerUpdate ();
	}

}
