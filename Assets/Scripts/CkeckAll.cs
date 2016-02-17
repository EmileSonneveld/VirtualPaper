using UnityEngine;
using System.Collections;

public class CkeckAll : MonoBehaviour {

    public Transform listHouse;
    public Transform listTree;
    public Transform listBHom;

    private float timer;

    private int nBHom;

    private Transform currentBHom;

    public bool noMoreTree;
	
	void FixedUpdate ()
    {
        if (timer < 1.0f)  //---Every 1 segonde
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
            currentBHom = listBHom.GetChild(i);

            check();
        }
    } 

    void check() //-----Check if the current BHom can have a house and assign this house-----
    {
        if ((!currentBHom.GetComponent<BHomInfo>().cutting) && (currentBHom.GetComponent<BHomInfo>().hisTreeCut == null))
        {
            if (currentBHom.GetComponent<BHomInfo>().hisHouse == null)
            {
                checkHouse();
            }
            else
            {
                if (currentBHom.GetComponent<BHomInfo>().hisTreeEat != null)
                {
                    setMoveHouseaTree();
                }
                else if (!noMoreTree)
                {
                    checkForATreeToEat();
                }
            }
        }
        else if ((currentBHom.GetComponent<BHomInfo>().cutting) || (currentBHom.GetComponent<BHomInfo>().hisTreeCut != null))
        {
            cutTree();
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
            if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisHouse.GetChild(0).position))
            {
                currentBHom.GetComponent<BHomInfo>().mTHouse = false;
                currentBHom.GetComponent<BHomInfo>().mToTree = true;
            }
        }
        else if (currentBHom.GetComponent<BHomInfo>().mToTree)
        {
            if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTreeEat.GetChild(0).position))
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
            if ((currentBHom.GetComponent<BHomInfo>().hisTreeEat != null) && ((currentBHom.GetComponent<BHomInfo>().hisTreeEat.GetComponent<Tree>().cutter1 == null) || (currentBHom.GetComponent<BHomInfo>().hisTreeEat.GetComponent<Tree>().cutter2 == null)))
            {
                setCutterToTree(currentBHom.GetComponent<BHomInfo>().hisTreeEat);
                currentBHom.GetComponent<BHomInfo>().hisTreeCut = currentBHom.GetComponent<BHomInfo>().hisTreeEat;

                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTreeCut.GetChild(currentBHom.GetComponent<BHomInfo>().nCutter).position))
                {
                    currentBHom.GetComponent<BHomInfo>().cutting = true;                    
                }
            }
            else
            {
                Transform tree = null;
                if (currentBHom.GetComponent<BHomInfo>().hisTreeCut == null)
                    tree = checkForATreeToCut(currentBHom.position);
                if ((tree != null) || (currentBHom.GetComponent<BHomInfo>().hisTreeCut != null))
                {
                    if (currentBHom.GetComponent<BHomInfo>().hisTreeCut == null)
                    {
                        currentBHom.GetComponent<BHomInfo>().hisTreeCut = tree;
                        setCutterToTree(tree);
                    }

                    if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTreeCut.GetChild(currentBHom.GetComponent<BHomInfo>().nCutter).position))
                    {
                        currentBHom.GetComponent<BHomInfo>().cutting = true;
                    }
                }
                else noMoreTree = true;
            }
        }
    }

    private void setCutterToTree(Transform tree)  //-----Set the cutters of a tree-----
    {
        if (tree.GetComponent<Tree>().cutter1 == null)
        {
            tree.GetComponent<Tree>().cutter1 = currentBHom;
            currentBHom.GetComponent<BHomInfo>().nCutter = 1;
        }
        else if (tree.GetComponent<Tree>().cutter2 == null)
        {
            tree.GetComponent<Tree>().cutter2 = currentBHom;
            currentBHom.GetComponent<BHomInfo>().nCutter = 2;
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
                if (Vector3.Distance(pos, listTree.GetChild(i).position) < Vector3.Distance(pos, lastTree.position))  //---Check if the current cheched tree is closer than the previous closest---
                {
                    if ((listTree.GetChild(i).GetComponent<Tree>().cutter1 == null) || (listTree.GetChild(i).GetComponent<Tree>().cutter2 == null))  //---Check if the tree is already cutting by max two BHom---
                        lastTree = listTree.GetChild(i);
                }
            }
            return lastTree;
        }
        else return null;
    }

    private void checkForATreeToEat()  //-----Check for the first in the list to eat-----
    {
        bool alrTree = false;
        int nTree = listTree.childCount;
        if (nTree > 0)
        {
            for (int i = 0; i < nTree; i++)
            {
                if (!alrTree)
                {
                    if (listTree.GetChild(i).GetComponent<Tree>().p1 == null)
                    {
                        alrTree = true;
                        listTree.GetChild(i).GetComponent<Tree>().p1 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTreeEat = listTree.GetChild(i);
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p2 == null)
                    {
                        alrTree = true;
                        listTree.GetChild(i).GetComponent<Tree>().p2 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTreeEat = listTree.GetChild(i);
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p3 == null)
                    {
                        alrTree = true;
                        listTree.GetChild(i).GetComponent<Tree>().p3 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTreeEat = listTree.GetChild(i);
                    }
                }
            }
        }
    }
}
