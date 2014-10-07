using UnityEngine;
using System.Collections;
/* Starcatcher finds stars that have exited the boundary and returns them to the left edge of the screen.
 * It should also re-randomize their velocity and change their dependant values such as scale.
 */
public class StarCatcher : MonoBehaviour {
	public int startLine;

	void OnTriggerExit(Collider star){
		star.transform.position = new Vector3 (startLine, 0, star.transform.position.z);
	}
}
