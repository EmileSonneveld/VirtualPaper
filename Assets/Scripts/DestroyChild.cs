using UnityEngine;
using System.Collections;

public class DestroyChild : MonoBehaviour {

	public Transform test;

	public void DestroyCh(){
		//transform.parent.GetComponent<BHom> ().goToAnchor = false;
		Destroy (transform.GetChild(1).GetChild(0).gameObject);
		//Debug.Log ("Appelle ?");
	}
}
