using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AgeOfPaperManage : MonoBehaviour {

    public Transform listHouse;
    public Transform listTree;
    public Transform listBHom;

    /*public List<Transform> listHouse;
    public List<Transform> listTree;
    public List<Transform> listBHom;*/

    private float timer = 0;
    public float refrechTime = 1;
    public bool alrAMuder;

    public int nHouse, nTree, nBHom, numberOfCurrentBHom;

    private BHomInfo currentBHomInfo;

    private CkeckAll ckeckAll;

    void Start ()
    {
        /*listHouse = instanceList(listHouseTra);
        listTree = instanceList(listTreeTra);
        listBHom = instanceList(listBHomTra);*/

        ckeckAll = transform.GetComponent<CkeckAll>();
    }

    /*private List<Transform> instanceList (Transform listTrans)
    {
        if (listTrans.childCount > 0)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < listTrans.childCount; i++)
                list.Add(listTrans.GetChild(i));

            return list;
        } else return null;
    }*/

    /*public void AddHouseToList()
    {
        listHouse.Add(listHouseTra.GetChild(listHouseTra.childCount - 1));
    }

    public void AddTreeToList()
    {
        listTree.Add(listTreeTra.GetChild(listTreeTra.childCount - 1));
    }*/

    void Update()
    {
        if (Input.GetKeyDown("a"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (timer < refrechTime)  //---Every 1 segonde
            timer += Time.deltaTime;
        else
        {
            timer = 0;

            UpDateVar();

            for (numberOfCurrentBHom = 0; numberOfCurrentBHom < nBHom; numberOfCurrentBHom++)
            {
                currentBHomInfo = listBHom.GetChild(numberOfCurrentBHom).GetComponent<BHomInfo>();
                switch (currentBHomInfo.actionToDo)
                {
                    case 1:
                        ckeckAll.setMoveHouseaTree(listBHom.GetChild(numberOfCurrentBHom));
                        break;
                    case 2:
                        ckeckAll.cutTree(listBHom.GetChild(numberOfCurrentBHom));
                        break;
                    case 3:
                        ckeckAll.killBHom(listBHom.GetChild(numberOfCurrentBHom));
                        break;
                    case 4:
                        ckeckAll.checkReproductionCondition(listBHom.GetChild(numberOfCurrentBHom));
                        break;
                    case 5:
                        ckeckAll.IsAChild(listBHom.GetChild(numberOfCurrentBHom));
                        break;
                    default:

                        break;
                }
            }

        }
    }

    private void UpDateVar()  //-----Up date variable if they are different of the reality-----
    {
        if (listHouse.childCount != nHouse)
            nHouse = listHouse.childCount;
        if (listTree.childCount != nTree)
            nTree = listTree.childCount;
        if (listBHom.childCount != nBHom)
            nBHom = listBHom.childCount;
    }

    public void AssignToBHom(Transform listOfElements, int nByElement, int nElements)
    {
        if (checkAssignment(nByElement))
        {
            if (currentBHomInfo.hisTreeCut != null)
            {
                if (currentBHomInfo.hisTreeCut.GetComponent<Tree>().cutter1 == listBHom.GetChild(numberOfCurrentBHom))
                    currentBHomInfo.hisTreeCut.GetComponent<Tree>().cutter1 = null;
                else if (currentBHomInfo.hisTreeCut.GetComponent<Tree>().cutter2 == listBHom.GetChild(numberOfCurrentBHom))
                    currentBHomInfo.hisTreeCut.GetComponent<Tree>().cutter2 = null;
                currentBHomInfo.hisTreeCut = null;
            }

            if (currentBHomInfo.hisBHomKill != null)
            {
                currentBHomInfo.hisBHomKill.GetComponent<BHomInfo>().hisBHomMurder = null;
                currentBHomInfo.hisBHomKill = null;
            }
                
            if (!listBHom.GetChild(numberOfCurrentBHom).GetComponent<NavMeshAgent>().enabled)
                listBHom.GetChild(numberOfCurrentBHom).GetComponent<NavMeshAgent>().enabled = true;
        }
        else
        {
            //---Call bip bip---
            AssignTreeOrBHom(nByElement);
        }
    }

    private bool checkAssignment(int nByElement)
    {
        bool check = false;
        if (nByElement == 2)
            check = ckeckAll.checkHouse(listBHom.GetChild(numberOfCurrentBHom));
        else if (nByElement == 3)
            check = ckeckAll.checkForATreeToEat(listBHom.GetChild(numberOfCurrentBHom));
        return check;
    }

    private void AssignTreeOrBHom(int nByElement) //-----Assign tree to cut or bhom to kill-----
    {
        if (nByElement == 2) //---if this is a tree---
        {
            if (currentBHomInfo.hisHouse == null)
            {
                if (listTree.childCount > 0 && currentBHomInfo.hisTreeCut == null)
                {                                    
                    if (ckeckAll.wouldWait(5, 5, listBHom.GetChild(numberOfCurrentBHom)))
                    {
                        ckeckAll.setCutterToTree(listTree.GetChild(nTree - 1), listBHom.GetChild(numberOfCurrentBHom));
                        if (currentBHomInfo.hisTreeCut != null)
                        {
                            currentBHomInfo.cutter = true;
                            currentBHomInfo.actionToDo = 2;
                        }
                    }
                }
            }
        }
        else if (nByElement == 3)
        {
            if (!alrAMuder)
                if (currentBHomInfo.hisTreeEat == null)
                {
                    if (listBHom.childCount > 1 && currentBHomInfo.hisBHomKill == null)
                        if (ckeckAll.wouldWait(5, 5, listBHom.GetChild(numberOfCurrentBHom)))
                        {
                            currentBHomInfo.kepper = true;
                            currentBHomInfo.actionToDo = 3;
                            alrAMuder = true;
                        }
                }
        }
    }



    



}
