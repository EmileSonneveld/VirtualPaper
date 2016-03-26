using UnityEngine;
using System.Collections;

public class SoundTriggerer : MonoBehaviour {

	public void ChopSound()
	{
		AkSoundEngine.PostEvent ("CutTree", gameObject);
    }
	public void ThrowSound(){
		AkSoundEngine.PostEvent ("ThrowPeople", gameObject);
	}
	public void SacrificeSound(){
		AkSoundEngine.PostEvent ("Sacrifice", gameObject);
	}
	public void Footstep(){
		AkSoundEngine.PostEvent ("PeopleWalk", gameObject);
	}
}
