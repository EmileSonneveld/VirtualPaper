using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgeOfPaperManage : MonoBehaviour {

    public Transform listHouseTra;
    public Transform listTreeTra;
    public Transform listBHomTra;

    public List<Transform> listHouse;
    public List<Transform> listTree;
    public List<Transform> listBHom;

    private float timer = 0;

    public int nHouse, nTree, nBHom, numberOfCurrentBHom;

    private BHomInfo currentBHomInfo;

    private CkeckAll ckeckAll;

    void Start ()
    {
        listHouse = instanceList(listHouseTra);
        listTree = instanceList(listTreeTra);
        listBHom = instanceList(listBHomTra);

        ckeckAll = transform.GetComponent<CkeckAll>();
    }

    private List<Transform> instanceList (Transform listTrans)
    {
        if (listTrans.childCount > 0)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < listTrans.childCount; i++)
                list.Add(listTrans.GetChild(i));

            return list;
        } else return null;
    }

    void Update()
    {
        if (timer < 1.0f)  //---Every 1 segonde
            timer += Time.deltaTime;
        else
        {
            timer = 0;

            UpDateVar();

            for (numberOfCurrentBHom = 0; numberOfCurrentBHom < nBHom; numberOfCurrentBHom++)
            {
                currentBHomInfo = listBHom[numberOfCurrentBHom].GetComponent<BHomInfo>();
                switch (currentBHomInfo.actionToDo)
                {
                    case 1:
                            ckeckAll.setMoveHouseaTree(listBHom[numberOfCurrentBHom]);
                        break;
                    default:

                        break;
                }
            }

        }
    }

    private void UpDateVar()  //-----Up date variable if they are different of the reality-----
    {
        if (listHouseTra.childCount != nHouse)
            nHouse = listHouseTra.childCount;
        if (listTreeTra.childCount != nTree)
            nTree = listTreeTra.childCount;
        if (listBHomTra.childCount != nBHom)
            nBHom = listBHomTra.childCount;
    }

    public Transform AssignToBHom(List<Transform> listOfElements, int nByElement)
    {
        if ((Mathf.FloorToInt((numberOfCurrentBHom + 1) / nByElement)) <= nHouse)
            return listOfElements[Mathf.FloorToInt((numberOfCurrentBHom + 1) / nByElement)];
        else
        {
            //---Call bip bip---
            //---Call Function preparation of cut---


            return null;
        }
    }

    private void AssignTreeOrBHom(int nByElement)
    {
        if (nByElement == 2) //---if this is a tree---
        {
            if (listTree.Count > 0)
            {
                currentBHomInfo.cutter = true;
                ckeckAll.setCutterToTree(listTree[nTree], listBHom[numberOfCurrentBHom]);
            }
        }
        else if (nByElement == 3)
        {
            
        }
    }



    



}
