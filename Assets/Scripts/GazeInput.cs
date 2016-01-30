using UnityEngine;
using System.Collections;

public class GazeInput : MonoBehaviour {

	public Transform m_Camera;
	public Transform m_CursorSprite;
	public float m_Range = 2f;

	private Ray m_Ray;
	private RaycastHit m_Hit;
	private Animator m_CursorAnim;
	private bool m_HoverOnInteractive = false;

	private bool m_PickedUpObject = false;		//true as soon as object is picked up
	private bool m_HitDropZone = false;	//true while object is picked up and cursor has hit a surface where it can be dropped
	private DragAndDropSprite m_HitObjectDADS;

	// Use this for initialization
	void Start () {
		m_CursorAnim = m_CursorSprite.gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//setup raycast
		m_Ray.origin = m_Camera.position;
		m_Ray.direction = m_Camera.forward;

		if (Physics.Raycast (m_Ray, out m_Hit, m_Range))
		{
			//place cursor at hitpoint
			m_CursorSprite.position = m_Hit.point;

			//check if hit was interactive
			if (!m_PickedUpObject && (m_Hit.transform.gameObject.tag == "Interactive" || m_Hit.transform.gameObject.tag == "Pickup")) {
				if (!m_HoverOnInteractive) {
					m_HoverOnInteractive = true;
					m_CursorAnim.SetBool ("hoverOn", true);
				}
			} else {
				m_HoverOnInteractive = false;
				m_CursorAnim.SetBool ("hoverOn", false);
			}

			//check if clicked while over interactive object
			if (m_HoverOnInteractive && (Input.GetKeyDown ("space") || Input.GetMouseButtonDown(0))) {
				m_Hit.transform.gameObject.SendMessage("OnClick");

				//if hit pickup, stor ref to Drag And Drop script
				if (m_Hit.transform.gameObject.tag == "Pickup") {
					m_HitObjectDADS = m_Hit.transform.gameObject.GetComponent<DragAndDropSprite>();		
					m_PickedUpObject = true;
				}
			}

			if (!m_HitDropZone & m_PickedUpObject) {
				m_HitObjectDADS.UpdateTargetPos ();
				Debug.Log ("moved no dropzone");
			}

			//check if we're over a dropzone
			if (m_PickedUpObject && m_Hit.transform.gameObject.tag == "DropZone") { 
				m_HitDropZone = true;
				//if we're holding an object update it's position
				if (m_PickedUpObject) {
					m_HitObjectDADS.UpdateTargetPos ();
					Debug.Log ("moved within dropzone");
					//if we click, drop the object
					if (Input.GetKeyDown ("space") || Input.GetMouseButtonDown (0)) {
						m_PickedUpObject = false;
						m_HitObjectDADS.Drop ();
						m_HitDropZone = false;
						AkSoundEngine.PostEvent ("UIDrop", gameObject);
					}
				}
			} 

		} 
		else 
		{
			//if last frame we were on interactive, call hoveroff
			if (m_HoverOnInteractive)
				m_HoverOnInteractive = false;
				m_CursorAnim.SetBool ("hoverOn", false);

			if (m_HitDropZone)
				m_HitDropZone = false;

		}
	}
}
