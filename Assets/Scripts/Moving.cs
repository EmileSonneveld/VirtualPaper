﻿using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {

    public Transform house;
    public Transform treeList;
    public Transform prayer;

    public bool believe;

    public int target = 0;
    public int curTarget = 0;
    public int maxTarget = 3;


    private int treeNChild = 0;
    private NavMeshAgent nMA;
    

	void Start () {
        nMA = GetComponent<NavMeshAgent>();
        //nMA.SetDestination(house.position);
    }
	

	void FixedUpdate ()
    {

        if (believe && (maxTarget < 4))
            maxTarget = 4;
        else if (!believe && (maxTarget > 3))
            maxTarget = 3;
        if (nMA.velocity == Vector3.zero)
        {
            if (curTarget == target)
                target++;

            if (target >= maxTarget)
                target = 1;

            if (target == 1)
            {
                nMA.SetDestination(house.position);
            }
            else if (target == 2)
            {
                if (treeList.GetChild(treeNChild).gameObject.GetComponent<Tree>().nFruit > 0)
                    nMA.SetDestination(treeList.GetChild(treeNChild).transform.position);
                else treeNChild = ChoseTree();
                /*
                if ()
                    treeList.GetChild(treeNChild).gameObject.GetComponent<Tree>().nFruit--;*/
            }
            else if (target == 3)
            {
                nMA.SetDestination(prayer.position);
            }

        }

        if (nMA.velocity != Vector3.zero)
        {
            if (target >= maxTarget)
                target = 0;

            /*if (target == 2)
                treeList.GetChild(treeNChild).gameObject.GetComponent<Tree>().nFruit--;*/

            if (curTarget != target)
                curTarget = target;
        }
    }

    private int ChoseTree ()
    {
        int tree = 0;
        for (int i = 0; i < treeList.childCount; i++)
        {
            if ((treeList.GetChild(i).GetComponent<Tree>().nFruit <= 0) && (Vector3.Distance(treeList.GetChild(i).transform.position, transform.position) < Vector3.Distance(treeList.GetChild(tree).transform.position, transform.position)))
                tree = i;
        }
        return tree;
    }
}