using UnityEngine;
using System.Collections;

public class CursorScale : MonoBehaviour {

	public Transform m_Camera;
	public float m_SizeDistOrigin;
	public float m_InitalScale;

	private Vector3 m_OriginalScale;

	// Use this for initialization
	void Awake () {
		m_OriginalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		scale ();
	}

	private void scale()
	{
		float dist = Vector3.Distance (m_Camera.position, transform.position);
		float factor = dist / m_SizeDistOrigin;
		transform.localScale = m_OriginalScale*factor*m_InitalScale;
	}
}
