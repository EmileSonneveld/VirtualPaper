﻿using UnityEngine;
using System.Collections;

public class DragAndDropEmpty : MonoBehaviour {

	public float m_SpaceAboveDragpoint = 0.5f;
	public float m_Speed = 2f;
	public float m_TargetScale = 1.2f;
	public GameObject m_SpawnObject;

	public GameObject m_EmptySprite;
	public GameObject m_TreeInstance;

	private Transform m_ObjectList;
	private bool m_PickedUp = false;
	private Transform m_Camera;
	private Transform m_Cursor;
	private Transform m_StartPos;
	private Vector3 m_TargetPos;
	private FaceCamera m_FaceCamera;

	// Use this for initialization
	void Start () {
		m_StartPos = transform;
		m_TargetPos = transform.position;
		m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		m_Cursor = GameObject.FindGameObjectWithTag("Cursor").transform;
		m_FaceCamera = GetComponent<FaceCamera> ();
		m_FaceCamera.enabled = false;
		m_ObjectList = GameObject.FindGameObjectWithTag("ListTree").transform;

		m_EmptySprite.SetActive (true);
		m_TreeInstance.SetActive (false);
	}

	void Update() {
		if (m_PickedUp) {
			//change layer to ignore raycast
			transform.gameObject.layer = LayerMask.NameToLayer( "Ignore Raycast" );
			m_FaceCamera.enabled = true;
			transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(m_TargetScale, m_TargetScale, m_TargetScale), Time.deltaTime * 2f);
		} else {
			//make sure layer is default
			transform.gameObject.layer = LayerMask.NameToLayer( "Default" );
			m_FaceCamera.enabled = false;
		}

		transform.position = Vector3.MoveTowards (transform.position, m_TargetPos, Time.deltaTime * m_Speed);
	}

	public void OnClick(){
		if(!m_PickedUp)
			m_PickedUp = true;
			m_EmptySprite.SetActive (false);
			m_TreeInstance.SetActive (true);
		//AkSoundEngine.PostEvent ("UIGrab", gameObject);
	}
	public void UpdateTargetPos(){
		m_TargetPos = m_Cursor.position;
	}

	public void Drop(){
		//AkSoundEngine.PostEvent ("UIDrop", gameObject);
		m_FaceCamera.enabled = false;
		m_PickedUp = false;
		gameObject.tag = "Untagged";

		//instantiate the real object
		GameObject newObject = Instantiate (m_SpawnObject, m_TargetPos, Quaternion.identity) as GameObject;
		newObject.transform.parent = m_ObjectList;

		//destroy pickup placeholder object
		Destroy(transform.gameObject);
	}
}
