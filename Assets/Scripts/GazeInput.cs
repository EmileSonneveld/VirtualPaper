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
			if (m_Hit.transform.gameObject.tag == "Interactive") {
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
			}

		} 
		else 
		{
			//if last frame we were on interactive, call hoveroff
			if (m_HoverOnInteractive)
				m_HoverOnInteractive = false;
				m_CursorAnim.SetBool ("hoverOn", false);

		}
	}
}
