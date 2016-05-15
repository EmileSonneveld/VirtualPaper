using UnityEngine;
using System.Collections;

public class BHomInfo : MonoBehaviour {

    //--------Const--------

    public Transform AnchorThrow;
    public Transform Sacrefice;

    [HideInInspector]
    public NavMeshAgent navMeshA;
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
    public bool isAMurder;

    public bool isMoving;  //---Displacements---
    public bool mToTree;   //-Move to tree-
    public bool mToHouse = true; //-Move to house-

    public bool cutting, cutter;  //---Actions---
    public bool keeping, kepper;
    public bool willKeeping;
    public bool victim;
    public bool praying;

    public bool needHouse; //---Needs---
    public bool needTree;

    private string prefixeAnim;  //---Animations---
    private string fixeAnim;

    public int waitToDo; //---Waiting time---

    public int nFood;
    public bool readyToReproduction;
    public bool isAChild;

    public int actionToDo = 0; // 0 : nothing, 1 : move house <-> tree, 2 : cutTree, 3 : kill, 4 reproduction, 5 : grow up

    void Start()
    {
        navMeshA = transform.GetComponent<NavMeshAgent>();
        animFront = transform.GetChild(0).GetComponent<Animator>();
        animBack = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

    }

    void Update()
    {
        amination();
    }

    public void disableCollier()
    {

    }

    public bool arriveToDestnation(Vector3 destination) //-----Set and Check if the player is in destination-----
    {
        if (Vector3.Distance(transform.position, destination) < 0.05)
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

            if (!navMeshA.enabled)
            {
                navMeshA.enabled = true;
                if (getPraying())
                    setPraying(false);
            }

            if (getPraying())
                setPraying(false);

            navMeshA.SetDestination(destination);

            return false;
        }
    }

    private void amination()  //-----Set Animation compare to current need and the current action of the BHom-----
    {
        checkMood();
        setMood();
        if (!victim && !keeping)
        {
            if (isMoving)
            {
                animFront.Play(prefixeAnim + fixeAnim + "_marche");
                animBack.Play(prefixeAnim + fixeAnim + "_marche");
            }
            else if (praying)
            {
                animFront.Play("violet_priere");
                animBack.Play("violet_priere");
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
        else if (isAMurder && isMoving)
        {
            animFront.Play(prefixeAnim + fixeAnim + "_marche");
            animBack.Play(prefixeAnim + fixeAnim + "_marche");
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

        if (hisTreeEat.GetComponent<Tree>().p1 == transform) {
            hisTreeEat.GetComponent<Tree>().p1 = null;
        }
        else if (hisTreeEat.GetComponent<Tree>().p2 == transform)
        {
            hisTreeEat.GetComponent<Tree>().p2 = null;
        }
        else if (hisTreeEat.GetComponent<Tree>().p3 == transform)
        {
            hisTreeEat.GetComponent<Tree>().p3 = null;
        }
    }

    public void Keeper()  //-----Set var and animation of a murder BHom-----
    {
        keeping = true;
        willKeeping = false;
        animFront.Play(prefixeAnim + "pickup");
        animBack.Play(prefixeAnim + "pickup");
    }

    public void KillBySacrifice()  //-----Set Anim of the sacrifice-----
    {
        animFront.Play("violet_sacrifier");
        animBack.Play("violet_sacrifier");
        //resetVarAfterKill();
    }

    public void KillByThrow()  //-----Set Anim of the throw-----
    {
        animFront.Play("brun_jetter");
        animBack.Play("brun_jetter");
        //resetVarAfterKill();
    }

    public void resetVarAfterKill()  //-----Reset variable after BHom have made a murder-----
    {
        hisBHomKill = null;
        keeping = false;
        isAMurder = false;
        actionToDo = 1;
    }





    /*-------------------------------------------------------
    ----------------------Getter Setter----------------------
    -------------------------------------------------------*/

    public int getWaitToDo()  //-----Return the value of wait to do-----
    {
        return waitToDo;
    } 

    public void setWaitToDo(int value) //-----Set the value of wait to do-----
    {
        waitToDo = value;
    }

    public void incrassWait(int value)  //-----Diminue of value waitToDo-----
    {
        waitToDo += value;
    }

    public bool getPraying() {
        return praying;
    }

    public void setPraying(bool value) {
        praying = value;
    }

    public int getNFood()
    {
        return nFood;
    }

    public void setNFood(int value)
    {
        nFood = value;
    }

    public bool getReadyToReproduction()
    {
        return readyToReproduction;
    }

    public void setReadyToReproduction(bool value)
    {
        readyToReproduction = value;
    }

    public bool getIsAChild()
    {
        return isAChild;
    }

    public void setIsAChild(bool value)
    {
        isAChild = value;
    }
}
