using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {

	public Light[] m_Lights;
	public bool m_LightOn = true;
	public Renderer m_LampRenderer;
	public Material m_LightOnMat;
	public Material m_LightOffMat;
	
	public void OnClick(){
		m_LightOn = !m_LightOn;
		foreach (Light L in m_Lights) {
			L.enabled = m_LightOn;
		}
		if (m_LightOn) {
			Material[] mats;
			mats = m_LampRenderer.materials;
			mats [5] = m_LightOnMat;
			m_LampRenderer.materials = mats;
			AkSoundEngine.PostEvent ("ObjectLamp_On",gameObject);
		} else {
			Material[] mats;
			mats = m_LampRenderer.materials;
			mats [5] = m_LightOffMat;
			m_LampRenderer.materials = mats;
			AkSoundEngine.PostEvent ("ObjectLamp_Off", gameObject);
		}
	}
}
