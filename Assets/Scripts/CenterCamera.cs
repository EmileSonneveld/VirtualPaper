using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;

public class CenterCamera : MonoBehaviour {

	private bool _FirstUpdate = false;

	// Use this for initialization
	void Start () {
		RecenterCamera();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			RecenterCamera ();
		}
	}

	void LateUpdate () {
		if (!_FirstUpdate) {
			_FirstUpdate = true;
			RecenterCamera();
		}
	}

	public void RecenterCamera(){
		InputTracking.Recenter ();
	}
}
