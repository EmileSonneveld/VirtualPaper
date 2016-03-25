using UnityEngine;
using System.Collections;

public class AnimationInteractionBHom : MonoBehaviour {

    public Transform pencil;
    public Transform defaultPencilposition;

    public void attachedPencil() {
        pencil.parent =  transform.GetChild(2);
        pencil.localPosition = new Vector3(0, 0, 1);
        pencil.localEulerAngles = new Vector3(0, 0, -65);
    }

    public void replacePencil() {
        pencil.parent = defaultPencilposition;
        pencil.localPosition = new Vector3(-30.5f, 80, 0);
        pencil.localEulerAngles = Vector3.zero;
    }

    public void DestroyCh() {
        Destroy(transform.GetChild(1).GetChild(0).gameObject);
        transform.parent.GetComponent<BHomInfo>().isAMurder = true;        
    }

    public void cuttingProgressTree() {
        if (transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree>().cuttingProgress > 15) {
            if (!transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree2House>().alrFall)
                transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree2House>().SetAnimation(true);
        }
        else transform.parent.GetComponent<BHomInfo>().hisTreeCut.GetComponent<Tree>().incrassCuttingProgress(1);
    }

    public void animationSpeedZero()
    {
        transform.GetComponent<Animator>().speed = 0;
        transform.GetChild(0).GetComponent<Animator>().speed = 0;
    }
}
