using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	public Transform target;
	public bool freezeX = true;
	public bool faceCam = false;
	public bool invert = false;

	private Quaternion targetRot;

	void Start(){
		target = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	void Update () {
		transform.LookAt(target);
		//Quaternion rot = Quaternion.LookRotation (target.position - transform.position);
		//transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime * 6.0f);

		if (faceCam) {
			GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
			transform.LookAt(player.transform);
		}
		if(freezeX)
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		if(!invert)
			transform.Rotate (0, 180f, 0);
		//transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
