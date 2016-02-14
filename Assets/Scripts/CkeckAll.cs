using UnityEngine;
using System.Collections;

public class CkeckAll : MonoBehaviour {

    public Transform listHouse;
    public Transform listTree;
    public Transform listBHom;

    public int lastHouseUse;
    public int lastTreeUse;

    private float timer;

    private int nBHom;

    private Transform currentBHom;

    private bool noMoreTree;

	void Start () {
        lastHouseUse = 0;
        lastTreeUse = 0;
    }
	
	void FixedUpdate () {
        if (timer < 1.0f)
            timer += Time.fixedDeltaTime;
        else
        {
            timer = 0;

            //---Call funtion
            foreachBHom();
        }
	}

    void ingoreColl()
    {
        for (int i = 0; i < listBHom.childCount; i++)
        {
            //---disable collider
        }
    }

    void foreachBHom ()  //-----For each BHom in the scene-----
    {
        nBHom = listBHom.childCount;
        for (int i = 0; i < nBHom; i++)
        {
            lastHouseUse = 0;
            lastTreeUse = 0;
            currentBHom = listBHom.GetChild(i);

            check();
        }
    } 

    void check() //-----Check if the current BHom can have a house and assign this house-----
    {
        if (currentBHom.GetComponent<BHomInfo>().hisHouse == null)
        {
            checkHouse();
        }
        else
        {
            if (currentBHom.GetComponent<BHomInfo>().hisTree != null)
            {
                setMoveHouseaTree();
            }
            else if (!noMoreTree)
            {
                checkForATreeToEat();
            }
        }
    }

    private void checkHouse()  //-----Check and Set the first house in the list-----
    {
        int currentCheckHouse = 0;
        int nHouse = listHouse.childCount;
        bool findHouse = false;

        do
        {
            if (listHouse.GetChild(currentCheckHouse).GetComponent<House>().p1 == null)
            {
                currentBHom.GetComponent<BHomInfo>().hisHouse = listHouse.GetChild(currentCheckHouse);
                listHouse.GetChild(currentCheckHouse).GetComponent<House>().p1 = currentBHom;
                findHouse = true;
            }
            else if (listHouse.GetChild(currentCheckHouse).GetComponent<House>().p2 == null)
            {
                currentBHom.GetComponent<BHomInfo>().hisHouse = listHouse.GetChild(currentCheckHouse);
                listHouse.GetChild(currentCheckHouse).GetComponent<House>().p2 = currentBHom;
                findHouse = true;
            }
            else
            {
                //---No house
                cutTree();
            }

            currentCheckHouse++;

        } while ((currentCheckHouse < nHouse) && (!findHouse));
    }

    private void setMoveHouseaTree()  //-----Define the destination and set the movement-----
    {
        if (currentBHom.GetComponent<BHomInfo>().mTHouse)
        {
            if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisHouse.position))
            {
                currentBHom.GetComponent<BHomInfo>().mTHouse = false;
                currentBHom.GetComponent<BHomInfo>().mToTree = true;
            }
        }
        else if (currentBHom.GetComponent<BHomInfo>().mToTree)
        {
            if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTree.position))
            {
                currentBHom.GetComponent<BHomInfo>().mTHouse = true;
                currentBHom.GetComponent<BHomInfo>().mToTree = false;
            }
        }
    }

    private void cutTree()  //-----Start to chose a tree and began to cut it-----
    {
        if (!noMoreTree)
        {
            if (currentBHom.GetComponent<BHomInfo>().hisTree != null)
            {
                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTree.position))
                {
                    currentBHom.GetComponent<BHomInfo>().cutting = true;
                }
            }
            else
            {
                Transform tree = checkForATreeToCut(currentBHom.position);
                if (tree != null)
                {
                    if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(tree.position))
                    {
                        currentBHom.GetComponent<BHomInfo>().cutting = true;
                    }
                }
                else noMoreTree = true;
            }
        }
    }

    private Transform checkForATreeToCut(Vector3 pos) //-----Check for the closest tree to cut-----
    {
        int nTree = listTree.childCount;
        if (nTree > 0)
        {
            Transform lastTree = listTree.GetChild(0);

            for (int i = 1; i < nTree; i++)
            {
                if (Vector3.Distance(pos, listTree.GetChild(lastTreeUse).position) < Vector3.Distance(pos, listTree.GetChild(lastTreeUse).position))
                {
                    lastTree = listTree.GetChild(lastTreeUse);
                }
            }
            return lastTree;
        }
        else return null;
    }

    private void checkForATreeToEat()  //-----Check for the first in the list to eat-----
    {
        bool alrHouse = false;
        int nTree = listTree.childCount;
        if (nTree > 0)
        {
            for (int i = 0; i < nTree; i++)
            {
                if (!alrHouse)
                {
                    if (listTree.GetChild(i).GetComponent<Tree>().p1 == null)
                    {
                        alrHouse = true;
                        listTree.GetChild(i).GetComponent<Tree>().p1 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTree = listTree.GetChild(i);
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p2 == null)
                    {
                        alrHouse = true;
                        listTree.GetChild(i).GetComponent<Tree>().p2 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTree = listTree.GetChild(i);
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p3 == null)
                    {
                        alrHouse = true;
                        listTree.GetChild(i).GetComponent<Tree>().p3 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTree = listTree.GetChild(i);
                    }
                }
            }
        }
    }
}
