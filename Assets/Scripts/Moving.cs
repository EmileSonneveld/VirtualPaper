using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour {

    public Transform houseList;
    public Transform treeList;
    public Transform prayer;

    public bool believe;

    private int target = 0;
    private int curTarget = 0;
    public int maxTarget = 3;

    public bool trasportingFood;

    private bool arldimNFruit = true;
    private bool justeBeforeIsMoving = false;
    private Transform tree;
    private Transform house;
    private NavMeshAgent nMA;
    public float maxTime = 3;
    private float time;
    

	void Start () {
        nMA = GetComponent<NavMeshAgent>();        
        house = ChoseChild(houseList, transform);
        tree = ChoseChild(treeList, house);
        time = maxTime;
    }	

	void FixedUpdate () {
        if (house != null) {
            if (house.GetComponent<House>().deplacing) {
                house.GetComponent<House>().deplacing = false;
                //house.GetComponent<House>().nPeople = 0;-------------------------------------
                house = ChoseChild(houseList, transform);
                tree = ChoseChild(treeList, house);
            }

            Move();
        }
        else {
            TransformTree();
        }
    }

    private void Move() {
        if (believe && (maxTarget < 4))
            maxTarget = 4;
        else if (!believe && (maxTarget > 3))
            maxTarget = 3;



        if (nMA.velocity == Vector3.zero) {

            if (trasportingFood && (target == 1))
                trasportingFood = false;

            time -= Time.deltaTime;
            if (time < 0) { 
                if (curTarget == target)
                    target++;
                time = maxTime;

            }
            else {
                
                if ((tree != null) && (target == 2) && (house != null))
                {
                    //-----------Set Animation de prendre pomme-----------
                    if (!arldimNFruit)
                    {
                        tree.gameObject.GetComponent<Tree>().nFruit--;
                        arldimNFruit = true;
                        trasportingFood = true;
                    }
                }
            }
            if (target >= maxTarget)
                target = 1;
            if ((house != null) && (tree != null))
                if (target == 1) {
                    nMA.SetDestination(house.transform.GetChild(0).position);
                }
                else if (target == 2) {
                    if (tree.gameObject.GetComponent<Tree>().nFruit > 0)
                        nMA.SetDestination(tree.transform.GetChild(0).position);
                    else tree = ChoseChild(treeList, house);
                }
                else if (target == 3) {
                    nMA.SetDestination(prayer.position);
                }
        }

        if (nMA.velocity != Vector3.zero) {
            

            if (target >= maxTarget)
                target = 0;

            if (curTarget != target)
                curTarget = target;

            if ((target == 2) && (tree != null))
                if (arldimNFruit)
                    arldimNFruit = false;
        }
    }

    private Transform ChoseChild (Transform elementList, Transform argument) {
        int lastelement = 0;
        for (int i = 1; i < elementList.childCount; i++) {
            if (elementList.name == "ListTree") {
                if ((elementList.GetChild(i).GetComponent<Tree>().nFruit > 0) && 
                    ((Vector3.Distance(elementList.GetChild(i).position, argument.position))) <
                     (Vector3.Distance(elementList.GetChild(lastelement).position, argument.position)))
                    lastelement = i;
            }
            else if (elementList.name == "ListHouse") {
                if ((elementList.GetChild(i).GetComponent<House>().nPeople < 2) &&
                    ((Vector3.Distance(elementList.GetChild(i).position, argument.position)) <
                     (Vector3.Distance(elementList.GetChild(lastelement).position, argument.position))))
                {
                    lastelement = i;
                }
            }
        }
        if (lastelement == 0)
            return null;
        else {
            if (elementList.name == "ListHouse")
                elementList.GetChild(lastelement).GetComponent<House>().nPeople++;
            return elementList.GetChild(lastelement);
        }
    }

    private void TransformTree()
    {
        tree = ChoseChild(treeList, transform);
        nMA.SetDestination(tree/*.GetChild(0)*/.position);

        if (nMA.velocity != Vector3.zero) {
            justeBeforeIsMoving = true;
        }

        if ((nMA.velocity == Vector3.zero) && justeBeforeIsMoving) {

            GameObject newHouse;
            newHouse = Instantiate(houseList.GetChild(1).gameObject, tree.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            newHouse.transform.parent = houseList;

            house = newHouse.transform;

            DestroyImmediate(tree.gameObject);

            tree = ChoseChild(treeList, house);
        }
    }
}
