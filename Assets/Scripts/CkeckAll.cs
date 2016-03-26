using UnityEngine;
using System.Collections;

public class CkeckAll : MonoBehaviour {

    public Transform listHouse;
    public Transform listTree;
    public Transform listBHom;
    public GameObject prefabBHom;

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
        /*for (int i = 0; i < listBHom.childCount; i++)
            for (int j = 0; j < listBHom.childCount; j++)
                if (listBHom.GetChild(i) != listBHom.GetChild(j))
                    Physics.IgnoreCollision(listBHom.GetChild(i).GetChild(0).GetComponent<Collider>(), listBHom.GetChild(j).GetChild(0).GetComponent<Collider>(), true);*/
    }

    void foreachBHom ()  //-----For each BHom in the scene-----
    {
        nBHom = listBHom.childCount;
        for (int i = 0; i < listBHom.childCount; i++)
        {
           
            currentBHom = listBHom.GetChild(i);

            check();
        }
    }

    void check() //-----Check if the current BHom can have a house and assign this house-----
    {
        if (!currentBHom.GetComponent<BHomInfo>().getIsAChild())
        {
            if (!currentBHom.GetComponent<BHomInfo>().keeping)
            {                
                if (currentBHom.GetComponent<BHomInfo>().hisTreeCut == null)
                {
                    if (currentBHom.GetComponent<BHomInfo>().hisHouse == null)
                    {
                        //if (wouldWait(10))
                        checkHouse();
                    }
                    else
                    {
                        if (currentBHom.GetComponent<BHomInfo>().hisTreeEat != null)
                        {
                            checkReproductionCondition(/*And else set the setMoveHouseaTree() function*/);
                        }
                        else if (!noMoreTree)
                        {
                            checkForATreeToEat();
                            if (currentBHom.GetComponent<BHomInfo>().hisTreeEat == null)
                            {
                                if ((currentBHom.GetComponent<BHomInfo>().hisBHomKill == null) && (wouldWait(20, 5)))
                                {
                                    killBHom();
                                }
                                else if (currentBHom.GetComponent<BHomInfo>().hisBHomKill != null)
                                {
                                    killBHom();
                                }
                            }
                        }
                    }
                }
                else if (currentBHom.GetComponent<BHomInfo>().hisTreeCut != null)
                {
                    if (!currentBHom.GetComponent<BHomInfo>().cutting && (wouldWait(15, 5)))
                        cutTree();
                }
            }
            else killBHom();
        }
        else //---If is a child---
        {
            if (currentBHom.localScale.x < 1)
                currentBHom.localScale = new Vector3(currentBHom.localScale.x + 0.1f, currentBHom.localScale.y + 0.1f, currentBHom.localScale.z + 0.1f);
            else
            {
                currentBHom.GetComponent<BHomInfo>().setIsAChild(false);
                currentBHom.GetComponent<NavMeshAgent>().enabled = true;
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
                isPlacedByGod(listHouse.GetChild(currentCheckHouse));

                findHouse = true;
            }
            else if (listHouse.GetChild(currentCheckHouse).GetComponent<House>().p2 == null)
            {
                currentBHom.GetComponent<BHomInfo>().hisHouse = listHouse.GetChild(currentCheckHouse);
                listHouse.GetChild(currentCheckHouse).GetComponent<House>().p2 = currentBHom;
                isPlacedByGod(listHouse.GetChild(currentCheckHouse));
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
        if (currentBHom.GetComponent<BHomInfo>().mToHouse)
        {
            if (currentBHom.GetComponent<BHomInfo>().hisHouse != null)
                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisHouse.GetChild(0).position))
                {
                    currentBHom.GetComponent<BHomInfo>().mToHouse = false;
                    currentBHom.GetComponent<BHomInfo>().mToTree = true;
                }
        }
        else if (currentBHom.GetComponent<BHomInfo>().mToTree)
        {
            if (currentBHom.GetComponent<BHomInfo>().hisTreeEat != null)
            {
                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisTreeEat.GetChild(0).position))
                {
                    currentBHom.GetComponent<BHomInfo>().mToHouse = true;
                    currentBHom.GetComponent<BHomInfo>().mToTree = false;
                    currentBHom.GetComponent<BHomInfo>().setNFood(currentBHom.GetComponent<BHomInfo>().getNFood() + 1);
                }
            } else currentBHom.GetComponent<BHomInfo>().mToTree = false;
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
                    if (currentBHom.GetComponent<BHomInfo>().believe)
                        currentBHom.GetComponent<BHomInfo>().believe = false;
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
                        if (currentBHom.GetComponent<BHomInfo>().believe)
                            currentBHom.GetComponent<BHomInfo>().believe = false;
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
                        currentBHom.GetComponent<BHomInfo>().hisBHomKill = null;
                        isPlacedByGod(listTree.GetChild(i));
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p2 == null)
                    {
                        alrTree = true;
                        listTree.GetChild(i).GetComponent<Tree>().p2 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTreeEat = listTree.GetChild(i);
                        currentBHom.GetComponent<BHomInfo>().hisBHomKill = null;
                        isPlacedByGod(listTree.GetChild(i));
                    }
                    else if (listTree.GetChild(i).GetComponent<Tree>().p3 == null)
                    {
                        alrTree = true;
                        listTree.GetChild(i).GetComponent<Tree>().p3 = currentBHom;
                        currentBHom.GetComponent<BHomInfo>().hisTreeEat = listTree.GetChild(i);
                        currentBHom.GetComponent<BHomInfo>().hisBHomKill = null;
                        isPlacedByGod(listTree.GetChild(i));
                    }
                }
            }
        }
    }

    private void killBHom()  //-----The current bhom chose and kill a bhom compare is religion-----
    {
        if (!currentBHom.GetComponent<BHomInfo>().isAMurder)
        {
            if (currentBHom.GetComponent<BHomInfo>().hisBHomKill == null)
            {
                choseBHom(currentBHom.GetComponent<BHomInfo>().believe);
                if (currentBHom.GetComponent<BHomInfo>().hisBHomKill == null)
                    choseBHom(!currentBHom.GetComponent<BHomInfo>().believe);
            }

            if (!currentBHom.GetComponent<BHomInfo>().keeping && currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisBHomKill.position))
            {
                currentBHom.GetComponent<BHomInfo>().hisBHomKill.position = currentBHom.GetChild(0).GetChild(1).position;
                currentBHom.GetComponent<BHomInfo>().hisBHomKill.parent = currentBHom.GetChild(0).GetChild(1);
                currentBHom.GetComponent<BHomInfo>().Keeper();
                currentBHom.GetComponent<BHomInfo>().hisBHomKill.GetComponent<BHomInfo>().Victim();

            }
            else if (currentBHom.GetComponent<BHomInfo>().keeping)
            {
                if (!currentBHom.GetComponent<BHomInfo>().believe)
                {
                    if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().AnchorThrow.position))
                        currentBHom.GetComponent<BHomInfo>().KillByThrow();
                }
                else
                {
                    if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().Sacrefice.position))
                        currentBHom.GetComponent<BHomInfo>().KillBySacrifice();
                }
            }
        }
        else  //---Set deplacement of the murder to his house (or the first house if he did'nt have house)---
        {
            if (currentBHom.GetComponent<BHomInfo>().hisHouse != null)
            {
                if (!currentBHom.GetComponent<BHomInfo>().isMoving)
                    currentBHom.GetComponent<BHomInfo>().isMoving = true;
                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisHouse.GetChild(0).position))
                    currentBHom.GetComponent<BHomInfo>().resetVarAfterKill();
            }
            else
            {
                if (!currentBHom.GetComponent<BHomInfo>().isMoving)
                    currentBHom.GetComponent<BHomInfo>().isMoving = true;
                if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(listHouse.GetChild(0).GetChild(0).position))
                    currentBHom.GetComponent<BHomInfo>().resetVarAfterKill();
            }
        }
    }

    private void choseBHom(bool believe)  //-----Chose a bhom who not killing a bhom and different of the paremeter (believe)-----
    {
        int i = 0;
        bool chosed = false;
        i = 0;
        do
        {
            if ((listBHom.GetChild(i).GetComponent<BHomInfo>().believe != believe) && (listBHom.GetChild(i) != currentBHom))
                if (listBHom.GetChild(i).GetComponent<BHomInfo>().hisBHomMurder == null)
                {
                    currentBHom.GetComponent<BHomInfo>().hisBHomKill = listBHom.GetChild(i);
                    listBHom.GetChild(i).GetComponent<BHomInfo>().hisBHomMurder = currentBHom;
                    chosed = true;
                }
            i++;
        } while ((i < listBHom.childCount) && !chosed);
    }

    private void isPlacedByGod(Transform elem)  //-----Check if the element (tree or house) where the bhom are just assign is placed by god-----
    {
        if (elem.GetComponent<placeByGod>().getPlaceByGod())
            if (!currentBHom.GetComponent<BHomInfo>().believe)
                currentBHom.GetComponent<BHomInfo>().believe = true;
    }

    private bool wouldWait(int value, int believeAddValue)  //-----Manage the waiting before do an action-----
    {
        int waitingTime;
        if (currentBHom.GetComponent<BHomInfo>().believe)
        {
            waitingTime = value + believeAddValue;
            currentBHom.GetComponent<BHomInfo>().setPraying(true);
            currentBHom.GetComponent<NavMeshAgent>().enabled = false;
        }
        else waitingTime = value;

        if (currentBHom.GetComponent<BHomInfo>().isMoving)
            currentBHom.GetComponent<BHomInfo>().isMoving = false;

        if (currentBHom.GetComponent<BHomInfo>().getWaitToDo() < waitingTime)
        {
            currentBHom.GetComponent<BHomInfo>().incrassWait(1);
            return false;
        }
        else
        {
            currentBHom.GetComponent<BHomInfo>().setWaitToDo(0);
            currentBHom.GetComponent<NavMeshAgent>().enabled = true;
            if (currentBHom.GetComponent<BHomInfo>().believe)
                currentBHom.GetComponent<BHomInfo>().setPraying(false);
            return true;
        }            
    }

    private void checkReproductionCondition()  //-----Check all condition before reproduction-----
    {
        if (currentBHom.GetComponent<BHomInfo>().getNFood() > 14)
        {
            if ((currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p1 != null) && (currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p2 != null))
            {
                if (currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p1 == currentBHom)
                {
                    Debug.Log("perso 1 is done");
                    if (currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p2.GetComponent<BHomInfo>().getNFood() > 14)
                    {
                        reproduction(currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p2);
                    }
                }
                else if (currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p2 == currentBHom)
                {
                    Debug.Log("perso 2 is done");
                    if (currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p1.GetComponent<BHomInfo>().getNFood() > 14)
                    {
                        reproduction(currentBHom.GetComponent<BHomInfo>().hisHouse.GetComponent<House>().p1);
                    }
                }
            }
        }
        else setMoveHouseaTree();
    }

    private void reproduction(Transform orther)  //-----make the reproduction-----
    {
        if (currentBHom.GetComponent<BHomInfo>().arriveToDestnation(currentBHom.GetComponent<BHomInfo>().hisHouse.GetChild(0).position))
        {
            currentBHom.GetComponent<NavMeshAgent>().enabled = false;
            if (!currentBHom.GetComponent<BHomInfo>().getReadyToReproduction())
                currentBHom.GetComponent<BHomInfo>().setReadyToReproduction(true);
            else
            {
                currentBHom.GetComponent<BHomInfo>().setNFood(0);
                orther.GetComponent<BHomInfo>().setNFood(0);
                if (orther.GetComponent<BHomInfo>().getReadyToReproduction())
                    orther.GetComponent<BHomInfo>().setReadyToReproduction(false);

                GameObject newBHom = Instantiate(prefabBHom, currentBHom.GetComponent<BHomInfo>().hisHouse.GetChild(0).position, currentBHom.GetComponent<BHomInfo>().hisHouse.rotation) as GameObject;
                newBHom.transform.parent = currentBHom.transform.parent;
                newBHom.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                newBHom.GetComponent<BHomInfo>().setIsAChild(true);                
            }
        }
    }
}