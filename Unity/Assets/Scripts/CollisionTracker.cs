using UnityEngine;
using System.Collections;

public class CollisionTracker : MonoBehaviour {

	public GameObject currentTarget = null;

	void OnTriggerEnter(Collider other){
		if (currentTarget == null) {
			currentTarget = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other){
		if (currentTarget == other.gameObject) {
			currentTarget = null;
		}
	}

}