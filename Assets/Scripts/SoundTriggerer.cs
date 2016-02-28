using UnityEngine;
using System.Collections;

public class SoundTriggerer : MonoBehaviour {

	public void ChopSound()
	{
		AkSoundEngine.PostEvent ("CutTree", gameObject);
                
        if ((transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree>().cuttingProgress > 15) && (!transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree2House>().alrFall))
            transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree2House>().SetAnimation(true);
        else transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree>().cuttingProgress++;
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

    public void animationSpeedZero()
    {
        transform.GetComponent<Animator>().speed = 0;
        transform.GetChild(0).GetComponent<Animator>().speed = 0;
    }

}
