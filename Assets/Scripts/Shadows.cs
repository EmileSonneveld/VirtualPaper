using UnityEngine;
using System.Collections;

public class Shadows : MonoBehaviour {

	private SpriteRenderer m_SpriteRenderer;

	// Use this for initialization
	void Start () {
		m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		m_SpriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

		m_SpriteRenderer.receiveShadows = true;
	}
}
