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

        if (timer < 1.0f)  //---Every 1 segonde
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

    public Transform AssignToBHom(Transform listOfElements, int nByElement, int nElements)
    {
        if ((Mathf.FloorToInt((numberOfCurrentBHom) / nByElement)) < nElements)
        {
            if (currentBHomInfo.hisTreeCut != null)
                currentBHomInfo.hisTreeCut = null;

            if (currentBHomInfo.hisBHomKill != null)
                currentBHomInfo.hisBHomKill = null;

            return listOfElements.GetChild(Mathf.FloorToInt((numberOfCurrentBHom) / nByElement));
        }
        else
        {
            //---Call bip bip---
            AssignTreeOrBHom(nByElement);

            return null;
        }
    }

    private void AssignTreeOrBHom(int nByElement)
    {
        if (nByElement == 2) //---if this is a tree---
        {
            if (listTree.childCount > 0)                
            {
                ckeckAll.setCutterToTree(listTree.GetChild(nTree - 1), listBHom.GetChild(numberOfCurrentBHom));
                if (currentBHomInfo.hisTreeCut != null)
                    if (ckeckAll.wouldWait(5, 5, listBHom.GetChild(numberOfCurrentBHom)))
                    {
                        currentBHomInfo.cutter = true;
                        currentBHomInfo.actionToDo = 2;
                    }
            }
        }
        else if (nByElement == 3)
        {
            if (listBHom.childCount > 1)
                if (ckeckAll.wouldWait(5, 5, listBHom.GetChild(numberOfCurrentBHom)))
                {
                    currentBHomInfo.kepper = true;
                    currentBHomInfo.actionToDo = 3;
                    //ckeckAll.killBHom(listBHom[numberOfCurrentBHom]);
                }
        }
    }



    



}
