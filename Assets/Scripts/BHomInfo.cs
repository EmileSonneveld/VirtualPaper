using UnityEngine;
using System.Collections;

public class BHomInfo : MonoBehaviour {

    //--------Const--------

    public Transform AnchorThrow;
    public Transform Sacrefice;

    private NavMeshAgent navMeshA;
    private Animator animFront;
    private Animator animBack;

    //--------Var--------

    public Transform hisHouse;  //---House-Tree---
    public Transform hisTreeEat;
    public Transform hisTreeCut;

    public Transform hisBHomKill;  //---BHom to Kill---
    public Transform hisBHomMurder;  //---BHom who kill hi---

    public int nCutter;

    public bool believe;

    public bool isMoving;  //---Displacements---
    public bool mToTree;
    public bool mTHouse = true;

    public bool cutting;  //---Actions---
    public bool keeping;
    public bool victim;

    public bool needHouse; //---Needs---
    public bool needTree;

    private string prefixeAnim;  //---Animations---
    private string fixeAnim;    

    void Start()
    {
        navMeshA = transform.GetComponent<NavMeshAgent>();
        animFront = transform.GetChild(0).GetComponent<Animator>();
        animBack = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        amination();
    }

    public void disableCollier()
    {

    }

    public bool arriveToDestnation(Vector3 destination) //-----Set and Check if the player is in destination-----
    {
        if (Vector3.Distance(transform.position, destination) < 0.08)
        {
            if (isMoving)
                isMoving = false;

            //navMeshA.Stop();
            navMeshA.SetDestination(destination);

            return true;
        }
        else
        {
            if (!isMoving)
                isMoving = true;
            navMeshA.SetDestination(destination);

            return false;
        }
    }

    private void amination()  //-----Set Animation compare to current need and the current action of the BHom
    {
        checkMood();
        setMood();
        if (!victim && !keeping)
            if (isMoving)
            {
                animFront.Play(prefixeAnim + fixeAnim + "_marche");
                animBack.Play(prefixeAnim + fixeAnim + "_marche");
            }
            else if (cutting)
            {
                if (nCutter == 1)
                {
                    animFront.Play(prefixeAnim + "coupe");
                    animBack.Play(prefixeAnim + "coupe");
                }
                else if (nCutter == 2)
                {
                    animFront.Play(prefixeAnim + "coupe_flip");
                    animBack.Play(prefixeAnim + "coupe_flip");
                }
            }
            else
            {
                animFront.Play(prefixeAnim + fixeAnim);
                animBack.Play(prefixeAnim + fixeAnim);
            }
    }

    private void setMood()  //-----Set prefixeAnim and fixeAnim-----
    {
        if (believe)
            prefixeAnim = "violet_";
        else prefixeAnim = "brun_";

        if (needTree && needHouse)
        {
            fixeAnim = "enerve";
        }
        else if (!needTree && !needHouse)
        {
            fixeAnim = "content";
        }
        else if (needTree || needHouse)
        {
            fixeAnim = "neutre";
        }

    }

    private void checkMood()  //-----Set needs bool compared to needs-----
    {
        if (hisHouse == null)
            needHouse = true;
        else needHouse = false;

        if (hisTreeEat == null)
            needTree = true;
        else needTree = false;
    }

    public void Victim()  //-----Set var, animation and transform of a victim Bhom-----
    {
        navMeshA.Stop();
        victim = true;
        animFront.Play(prefixeAnim + "victime");
        animBack.Play(prefixeAnim + "victime");
        navMeshA.enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(90, 270, 270);
    }

    public void Keeper()  //-----Set var and animation of a murder BHom-----
    {
        keeping = true;
        animFront.Play(prefixeAnim + "pickup");
        animBack.Play(prefixeAnim + "pickup");
    }

    public void KillBySacrifice()  //-----Set Anim of the sacrifice-----
    {
        animFront.Play("violet_sacrifier");
        animBack.Play("violet_sacrifier");
        resetVarAfterKill();
    }

    public void KillByThrow()  //-----Set Anim of the throw-----
    {
        animFront.Play("brun_jetter");
        animBack.Play("brun_jetter");
        resetVarAfterKill();
    }

    private void resetVarAfterKill()  //-----Reset variable after BHom have made a murder-----
    {
        hisBHomKill = null;
        keeping = false;
    }

}
