using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour
{

    public Transform houseList;
    public Transform treeList;
    public Transform prayer;

    public bool believe;

    public float scale = 0.2f;

    public bool movingBefore;
    public bool walktToHouse = true;
    public bool walkToTree = false;
    public bool walkToTemple = false;

    private int target = 0;
    private int curTarget = 0;
    public int maxTarget = 3;

    public bool transportingFood;
    public int nFoodToChild = 3;

    private bool arldimNFruit = true;
    private bool justeBeforeIsMoving = false;
    private Transform tree;
    private Transform house;
    private NavMeshAgent nMA;
    public float maxTime = 3;
    public float time;

    public bool dontMove;

    private bool alrdepositeFruit = true;

    public float timerScale = 11;

    void Start()
    {
        nMA = GetComponent<NavMeshAgent>();
        house = ChoseChild(houseList, transform);
        tree = ChoseChild(treeList, house);
        time = maxTime;
    }

    void FixedUpdate()
    {
        if (transform.localScale.x < scale)
        {
            if (timerScale > 0)
                timerScale -= Time.deltaTime;
            if ((timerScale <= 5) && (timerScale > 4.5f))
            {
                transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
                timerScale = 4.4f;
            }
            else if (timerScale <= 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
                transform.GetComponent<Moving>().dontMove = false;
            }
        }
        if (house != null)
        {
            if (!dontMove)
                if (house.GetComponent<House>().deplacing)
                {
                    house.GetComponent<House>().deplacing = false;
                    house.GetComponent<House>().nPeople = 0;
                    house = ChoseChild(houseList, transform);
                    tree = ChoseChild(treeList, house);
                }
            if (!dontMove)
                Move2();
            else if (house.GetComponent<House>().BHomOnHome >= 2)
            {
                dontMove = false;
                house.GetComponent<House>().houseFood -= 4;
                house.GetComponent<House>().BHomOnHome = 0;
                GameObject newBHom;
                newBHom = Instantiate(transform.parent.GetChild(0).gameObject, house.GetChild(0).position, Quaternion.Euler(0, 0, 0)) as GameObject;
                newBHom.transform.parent = transform.parent;
                newBHom.transform.localScale = new Vector3(scale / 4, scale / 4, scale / 4);
                newBHom.transform.GetComponent<Moving>().house = null;
                newBHom.transform.GetComponent<Moving>().tree = null;

            }
        }
        else if (!dontMove)
        {
            TransformTree();
        }
    }

    private void Move()
    {
        if (believe && (maxTarget < 4))
            maxTarget = 4;
        else if (!believe && (maxTarget > 3))
            maxTarget = 3;

        if (nMA.velocity == Vector3.zero)
        {
            if (house.GetComponent<House>().houseFood < nFoodToChild)
                time -= Time.deltaTime;
            if (time < 0)
            {
                if (curTarget == target)
                    target++;
                time = maxTime;

            }
            else
            {
                if ((tree != null) && (target == 2) && (house != null))
                {
                    //-----------Set Animation de prendre pomme-----------
                    if (!arldimNFruit)
                    {
                        tree.gameObject.GetComponent<Tree>().nFruit--;
                        arldimNFruit = true;
                        transportingFood = true;
                    }
                }
                else if ((tree != null) && (target == 1) && (house != null))
                {
                    if (!alrdepositeFruit)
                    {
                        alrdepositeFruit = true;
                        transportingFood = false;
                        house.GetComponent<House>().houseFood++;
                    }
                }
            }

            if (target >= maxTarget)
                target = 1;

            if ((house != null) && (tree != null))
                if (target == 1)
                {
                    nMA.SetDestination(house.transform.GetChild(0).position);
                }
                else if (target == 2)
                {
                    if (tree.gameObject.GetComponent<Tree>().nFruit > 0)
                        nMA.SetDestination(tree.transform.GetChild(0).position);
                    else tree = ChoseChild(treeList, house);
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

            if (curTarget != target)
                curTarget = target;

            if ((target == 2) && (tree != null))
                if (arldimNFruit)
                    arldimNFruit = false;

            if ((target == 1) && (tree != null) && (house != null))
                if (alrdepositeFruit)
                    alrdepositeFruit = false;

        }
    }

    public float test;

    private void Move2()
    {
        test = nMA.speed;
        if ((Vector3.Distance(transform.position, nMA.destination)) < (nMA.speed / 3))
        {

            if (!movingBefore)
                movingBefore = true;

            time -= Time.deltaTime;
            if ((time < 0) && movingBefore)
            {
                if (walktToHouse)
                {
                    nMA.SetDestination(house.GetChild(0).position);
                }
                else if (walkToTree)
                {
                    if (tree.gameObject.GetComponent<Tree>().nFruit > 0)
                        nMA.SetDestination(tree.transform.GetChild(0).position);
                    else tree = ChoseChild(treeList, house);

                    nMA.SetDestination(tree.GetChild(0).position);

                    if (transportingFood)
                    {
                        transportingFood = false;
                        house.GetComponent<House>().houseFood++;

                        if (house.GetComponent<House>().houseFood > nFoodToChild)
                        {
                            dontMove = true;
                            house.GetComponent<House>().BHomOnHome++;
                        }
                    }
                }
                else if (walkToTemple)
                {
                    nMA.SetDestination(prayer.position);
                }
            }
        }
        else if ((Vector3.Distance(transform.position, nMA.destination)) > (nMA.speed / 3))
        {

            if (movingBefore)
            {
                movingBefore = false;
                time = maxTime;

                if (walktToHouse)
                {
                    walkToTree = true;
                    walktToHouse = false;

                    if (!transportingFood)
                    {
                        transportingFood = true;
                        tree.gameObject.GetComponent<Tree>().nFruit--;
                    }
                }
                else if (walkToTree)
                {
                    walkToTree = false;
                    if (believe)
                        walkToTemple = true;
                    else walktToHouse = true;
                }
                else if (walkToTemple)
                {
                    walkToTemple = false;
                    walktToHouse = true;
                }
            }
        }
    }

    private Transform ChoseChild(Transform elementList, Transform argument)
    {
        int lastelement = 0;
        for (int i = 1; i < elementList.childCount; i++)
        {
            if (elementList.name == "ListTree")
            {
                if ((elementList.GetChild(i).GetComponent<Tree>().nFruit > 0) &&
                    ((Vector3.Distance(elementList.GetChild(i).position, argument.position))) <
                     (Vector3.Distance(elementList.GetChild(lastelement).position, argument.position)))
                    lastelement = i;
            }
            else if (elementList.name == "ListHouse")
            {
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
        else
        {
            if (elementList.name == "ListHouse")
            {
                elementList.GetChild(lastelement).GetComponent<House>().nPeople++;
            }
            return elementList.GetChild(lastelement);
        }
    }

    private void TransformTree()
    {
        tree = ChoseChild(treeList, transform);
        nMA.SetDestination(tree/*.GetChild(0)*/.position);

        if (nMA.velocity != Vector3.zero)
        {
            justeBeforeIsMoving = true;
        }

        if ((nMA.velocity == Vector3.zero) && justeBeforeIsMoving)
        {

            GameObject newHouse;
            newHouse = Instantiate(houseList.GetChild(1).gameObject, tree.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            newHouse.transform.parent = houseList;

            house = newHouse.transform;

            DestroyImmediate(tree.gameObject);

            tree = ChoseChild(treeList, house);
        }
    }
}
